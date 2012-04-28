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
        public ObservableCollection<CubeViewModel> CubeViewModels { get; private set; }

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
        public SoundSources SoundSources;

        public string PauseOrResumeText
        {
            get
            {
                return AppRunner.IsRunning ? "Pause" : "Resume";
            }
        }
        public bool CanPauseOrResume
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
                            for (var i = 0; i < numToChange; i++)
                            {
                                CubeViewModels.RemoveAt(CubeViewModels.Count - 1);
                            }
                            NeighborCalculator.CalculateNeighbors(CubeViewModels);
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
                            if ((i * 4) + j > CubeViewModels.Count - 1) { NeighborCalculator.CalculateNeighbors(CubeViewModels); Status = ReadyStatus; return; }
                            CubeViewModels[(i * 4) + j].PositionX = 200 * j;
                            CubeViewModels[(i * 4) + j].PositionY = 200 * i;
                        }
                    }
                    NeighborCalculator.CalculateNeighbors(CubeViewModels);
                    Status = ReadyStatus;
                });
            #endregion
            #region LoadAFileCommand
            LoadAFileCommand = new RelayCommand(() =>
                {
                    if (AppRunner.IsRunning)
                    {
                        try
                        {
                            AppRunner.StopExecution();
                        }
                        catch (Exception e)
                        {
                            Status = "Unable to stop application.";
                            MessageBox.Show(Application.Current.MainWindow, "Siftables Emulator was unable to stop the running application.\n\nTechnical Details: " + e, "Unhandled Exception", MessageBoxButton.OK);
                        }
                    }

                    Status = "Select the application to run.";

                    var openFileDialog = new OpenFileDialog { Filter = "C# Library (*.dll)|*.dll|All Files (*.*)|*.*", FilterIndex = 1, Multiselect = false };
                    if (openFileDialog.ShowDialog() == true)
                    {
                        Status = "Loading application...";
                        using (var fileStream = openFileDialog.File.OpenRead())
                        {
                            try
                            {
                                AppRunner.LoadApplication(fileStream);
                            }
                            catch (TypeLoadException e)
                            {
                                Status = "Unable to load application.";
                                MessageBox.Show(Application.Current.MainWindow, "Siftables Emulator was unable to load the application.\n\nTechnical Details: " + e, "Unhandled Exception", MessageBoxButton.OK);
                            }
                        }
                        if (AppRunner.IsLoaded)
                        {
                            Status = "Loading image resources...";
                            if (openFileDialog.File.Directory != null)
                            {
                                _imageSources =
                                    new ImageSources(openFileDialog.File.Directory.FullName + "/assets/images");
                                SoundSources =
                                    new SoundSources(openFileDialog.File.Directory.FullName + "/assets/sounds");

                                SoundSources.NotifyNewSound +=
                                    sound => SoundSources.InitializeSound(sound);
                                Sound.NotifyPauseAllSounds += () => SoundSources.PauseAllSounds();
                                Sound.NotifyResumeAllSounds += () => SoundSources.ResumeAllSounds();
                                Sound.NotifyStopAllSounds += () => SoundSources.StopAllSounds();

                                AppRunner.App.Images = _imageSources.GetImageSet();

                                foreach (var cubeViewModel in CubeViewModels)
                                {
                                    cubeViewModel.ImageSources = _imageSources;
                                }

                                Status = openFileDialog.File.Name + " was loaded.";
                                AppRunner.StartExecution(CubeSet, Application.Current.MainWindow.Dispatcher,
                                                            SoundSources.SoundSet);
                                AppRunner.NotifyApplicationException += DisplayException;
                                NotifyPropertyChanged("PauseOrResumeText");
                                NotifyPropertyChanged("CanPauseOrResume");
                            }
                            else
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
            #region PauseOrResumeCommand

            PauseOrResumeCommand = new RelayCommand(() =>
            {
                if (AppRunner.IsRunning)
                {
                    AppRunner.PauseExecution();
                    SoundSources.PauseAllSounds();
                } else
                {
                    AppRunner.ResumeExecution();
                    SoundSources.ResumeAllSounds();
                }
                NotifyPropertyChanged("PauseOrResumeText");
            });
            #endregion

            AppRunner = AppRunner.GetInstance();

            CubeViewModels = new ObservableCollection<CubeViewModel>();
            AddNewCubes(NumInitialCubes);
            SnapToGridCommand.Execute(null);

            Status = ReadyStatus;
        }

        public void DisplayException(Exception e)
        {
            Status = String.Format("Application aborted ({0}: {1})", e.GetType(), e.Message);
            AppRunner.StopExecution();
            NotifyPropertyChanged("CanPauseOrResume");
            NotifyPropertyChanged("PauseOrResumeText");
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
            cubeViewModel.CubeModel.NotifyCubeMoved += (sender, arguments) => NeighborCalculator.CalculateNeighbors(CubeViewModels);
            if (AppRunner.IsRunning)
            {
                cubeViewModel.ImageSources = _imageSources;
            }
            CubeViewModels.Add(cubeViewModel);
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

    }
}
