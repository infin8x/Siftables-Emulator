using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using Sifteo;

namespace Siftables.ViewModel
{
    public class MainWindowViewModel : ViewModelNotifier
    {
        public const int NumInitialCubes = 6;

        #region BindingDefinitions
        public ObservableCollection<CubeViewModel> CubeViewModels { get; set; }
        public Collection<SoundViewModel> InactiveSounds { get; set; }
        public ObservableCollection<SoundViewModel> ActiveSounds { get; set; }

        public RelayCommand SnapToGridCommand { get; private set; }
        public RelayCommand LoadAFileCommand { get; private set; }
        public RelayCommand PauseOrResumeCommand { get; private set; }
        public RelayCommand<EventArgs> ChangeNumberOfCubesCommand { get; private set; }

        private String _status;
        public String Status
        {
            get { return _status; }

            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }
        public const String ReadyStatus = "Ready";

        #endregion

        public AppRunner AppRunner { get; private set; }

        private ImageSources _imageSources;
        private SoundSources _soundSources;

        public string PauseOrResumeText
        {
            get
            {
                return AppRunner.IsRunning ? "Pause" : "Resume";
            }
        }

        public bool PauseOrResumeEnabled
        {
            get { return AppRunner.IsRunning; }
        }

        public MainWindowViewModel()
        {
            #region ChangeNumberOfCubesCommand
            ChangeNumberOfCubesCommand = new RelayCommand<EventArgs>(e =>
                {
                    Status = "Changing number of cubes";
                    var args = e as RoutedPropertyChangedEventArgs<double>;
                    if (args != null)
                    {
                        var numToChange = Math.Abs(Convert.ToInt32(CubeViewModels.Count - args.NewValue));
                        if (args.NewValue < CubeViewModels.Count) // removing cubes
                        {
                            for (var i = 0; i < numToChange; i++) {
                                CubeViewModels.RemoveAt(CubeViewModels.Count - 1);
                            }
                            CalculateNeighbors();
                        }
                        else if (args.NewValue > CubeViewModels.Count) // adding cubes
                        {
                            AddNewCubes(numToChange);
                            Status = ReadyStatus;
                            SnapToGridCommand.Execute(null);
                            if (AppRunner.IsRunning)
                            {
                                AppRunner.App.AssociateCubes(CubeSet);
                            }
                        }
                    }
                    Status = ReadyStatus;
                });
            #endregion
            #region SnapToGridCommand
            SnapToGridCommand = new RelayCommand(() =>
                {
                    Status = "Snapping to grid";
                    for (var i = 0; i < Math.Ceiling(CubeViewModels.Count / 4.0); i++)
                    {
                        for (var j = 0; j < 4; j++)
                        {
                            if ((i * 4) + j > CubeViewModels.Count - 1) { CalculateNeighbors(); Status = ReadyStatus; return; }
                            CubeViewModels[(i * 4) + j].PositionX = 200 * j;
                            CubeViewModels[(i * 4) + j].PositionY = 200 * i;
                        }
                    }
                    CalculateNeighbors();
                    Status = ReadyStatus;
                });
            #endregion
            #region LoadAFileCommand
            LoadAFileCommand = new RelayCommand(() =>
                {
                    if (AppRunner.IsRunning)
                    {
                        AppRunner.StopExecution();
                    }

                    Status = "Select the application to run.";

                    var openFileDialog = new OpenFileDialog { Filter = "C# Library (*.dll)|*.dll|All Files (*.*)|*.*", FilterIndex = 1, Multiselect = false};
                    if (openFileDialog.ShowDialog() == true)
                    {
                        Status = "Loading application...";
                        using (var fileStream = openFileDialog.File.OpenRead())
                        {
                            try
                            {
                                AppRunner.LoadApplication(fileStream);
                            } catch (TypeLoadException e)
                            {
                                Status = "Application load failed: " + e.Message;
                            }
                        }
                        if (AppRunner.IsLoaded)
                        {
                            Status = "Loading image resources...";
                            if (openFileDialog.File.Directory != null)
                            {
                                _imageSources =
                                    new ImageSources(openFileDialog.File.Directory.FullName + "/assets/images");
                                _soundSources =
                                    new SoundSources(openFileDialog.File.Directory.FullName + "/assets/sounds");
                                _soundSources.NotifyNewSound += InitializeSound;

                                Sound.NotifyPauseAllSounds += PauseAllSounds;
                                Sound.NotifyResumeAllSounds += ResumeAllSounds;
                                Sound.NotifyStopAllSounds += StopAllSounds;
                                AppRunner.App.Images = _imageSources.GetImageSet();

                                foreach (var cubeViewModel in CubeViewModels)
                                {
                                    cubeViewModel.ImageSources = _imageSources;
                                }

                                Status = openFileDialog.File.Name + " was loaded.";
                                AppRunner.StartExecution(CubeSet, Application.Current.MainWindow.Dispatcher,
                                                            _soundSources.GetSoundSet());
                                NotifyPropertyChanged("PauseOrResumeText");
                            } else
                            {
                                Status = "Application loading failed.";
                            }
                        }
                    }
                    else
                    {
                        Status = "Program loader was closed.";
                    }
                });
            #endregion
            #region PauseOrResumeButton

            PauseOrResumeCommand = new RelayCommand(() =>
            {
                if (AppRunner.IsRunning)
                {
                    AppRunner.PauseExecution();
                    PauseAllSounds();
                } else
                {
                    AppRunner.ResumeExecution();
                    ResumeAllSounds();
                }
                NotifyPropertyChanged("PauseOrResumeText");
            });
            #endregion

            AppRunner = AppRunner.GetInstance();

            CubeViewModels = new ObservableCollection<CubeViewModel>();
            AddNewCubes(NumInitialCubes);
            SnapToGridCommand.Execute(null);

            Status = ReadyStatus;
            ActiveSounds = new ObservableCollection<SoundViewModel>();
            InactiveSounds = new Collection<SoundViewModel>();
        }

        private void AddNewCubes(int n)
        {
            for (var i = 0; i < n; i++)
            {
                AddNewCube();
            }
        }

        private void AddNewCube()
        {
            var cubeViewModel = new CubeViewModel();
            cubeViewModel.CubeModel.NotifyCubeMoved += (sender, arguments) => CalculateNeighbors();
            if (AppRunner.IsRunning)
            {
                cubeViewModel.ImageSources = _imageSources;
            }
            CubeViewModels.Add(cubeViewModel);
        }

        public void StopAllSounds()
        {
            foreach (var sound in ActiveSounds)
            {
                sound.Position = TimeSpan.Zero;
                InactiveSounds.Add(sound);
            }
            ActiveSounds.Clear();
        }

        public void ResumeAllSounds()
        {
            foreach (var sound in InactiveSounds)
            {
                ActiveSounds.Add(sound);
                sound.RestoreResumeSpot();
            }
            InactiveSounds.Clear();
        }

        public void PauseAllSounds()
        {
            foreach (var sound in ActiveSounds)
            {
                sound.SetResumeSpot();
                InactiveSounds.Add(sound);
            }
            ActiveSounds.Clear();
        }

        public void InitializeSound(Sound sound)
        {
            var soundViewModel = new SoundViewModel(sound, GetSoundPath(sound.Name).Replace(@"\", @"/"));
            InactiveSounds.Add(soundViewModel);
            soundViewModel.NotifyPlay += () =>
                                                {
                                                    InactiveSounds.Remove(soundViewModel);
                                                    ActiveSounds.Add(soundViewModel);
                                                };
            soundViewModel.NotifyPause += () =>
            {
                ActiveSounds.Remove(soundViewModel);
                if (!InactiveSounds.Contains(soundViewModel))
                {
                    InactiveSounds.Add(soundViewModel);
                }
            };
            soundViewModel.NotifyResume += () =>
            {
                InactiveSounds.Remove(soundViewModel);
                if (!ActiveSounds.Contains(soundViewModel))
                {
                    ActiveSounds.Add(soundViewModel);
                }
            };
        }

        public void AddSound(Sound sound)
        {
            ActiveSounds.Add(new SoundViewModel(sound, GetSoundPath(sound.Name)));
        }

        public string GetSoundPath(string name)
        {
            return _soundSources.GetSoundPath(name);
        }

        public CubeSet CubeSet
        {
            get
            {
                var cubeSet = new CubeSet();
                foreach (var cube in CubeViewModels)
                {
                    cubeSet.Add(cube.CubeModel);
                }

                return cubeSet;
            }
        }

        public void CalculateNeighbors()
        {
            var count = CubeViewModels.Count;
            // I'd like to eliminate this loop... but we have to reset everything before we can start processing neighbors
            for (var i = 0; i < count; i++)
            {
                var c = CubeViewModels[i].CubeModel;
                c.Neighbors = new Neighbors();
            }
            for (var i = 0; i < count - 1; i++)
            {
                var aV = CubeViewModels[i];
                for (var j = i + 1; j < count; j++)
                {
                    var bV = CubeViewModels[j];
                    // If anybody knows a better way to do this, please fix it.  The only way I could get
                    // the DP to expose its value is through its ToString method...
                    var aLeft = aV.PositionX;
                    var aTop = aV.PositionY;
                    var bLeft = bV.PositionX;
                    var bTop = bV.PositionY;
                    var aC = aV.CubeModel;
                    var bC = bV.CubeModel;
                    if ((Math.Abs(aLeft - bLeft) <= (Neighbors.GAP_TOLERANCE + Cube.dimension)) && (Math.Abs(aTop - bTop) <= (Cube.dimension - Neighbors.SHARED_EDGE_MINIMUM)))
                    {
                        if (aLeft < bLeft)
                        {
                            aC.Neighbors.Right = bC;
                            bC.Neighbors.Left = aC;
                        }
                        else
                        {
                            aC.Neighbors.Left = bC;
                            bC.Neighbors.Right = aC;
                        }
                    }
                    if ((Math.Abs(aTop - bTop) <= (Neighbors.GAP_TOLERANCE + Cube.dimension)) && (Math.Abs(aLeft - bLeft) <= (Cube.dimension - Neighbors.SHARED_EDGE_MINIMUM)))
                    {
                        if (aTop < bTop)
                        {
                            aC.Neighbors.Top = bC;
                            bC.Neighbors.Bottom = aC;
                        }
                        else
                        {
                            aC.Neighbors.Bottom = bC;
                            bC.Neighbors.Top = aC;
                        }
                    }
                }
            }
        }

    }
}
