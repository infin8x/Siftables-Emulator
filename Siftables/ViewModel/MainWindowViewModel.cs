using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using Sifteo;

namespace Siftables.ViewModel
{
    public class MainWindowViewModel : ViewModelNotifier
    {
        public const int NumInitialCubes = 6;
        public String ReadyStatus { get { return Strings.ReadyStatus; } }

        public ObservableCollection<CubeViewModel> CubeViewModels { get; private set; }
        public ObservableCollection<SoundViewModel> ActiveSounds { get; private set; }
        public Collection<SoundViewModel> InactiveSounds { get; private set; }

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

        public AppRunner AppRunner { get; private set; }

        private ImageSources _imageSources;
        public SoundSources SoundSources;

        public string SnapToGridText
        {
            get { return Strings.SnapToGrid; }
        }
        public string LoadAProgramText
        {
            get { return Strings.LoadAProgram; }
        }
        public string NumOfCubesText { get { return Strings.NumOfCubes; } }

        public string PauseOrResumeText
        {
            get
            {
                return AppRunner.IsRunning ? Strings.Pause : Strings.Resume;
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
                    Status = Strings.ChangingNumberStatus;
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
                    Status = Strings.SnappingStatus;
                    for (var i = 0; i < Math.Ceiling(CubeViewModels.Count / 4.0); i++)
                    {
                        for (var j = 0; j < 4; j++)
                        {
                            if ((i * 4) + j > CubeViewModels.Count - 1)
                            {
                                NeighborCalculator.CalculateNeighbors(CubeViewModels);
                                Status = ReadyStatus;
                                return;
                            }
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
                        AppRunner.StopExecution();
                    }

                    Status = Strings.SelectAppStatus;

                    var openFileDialog = new OpenFileDialog
                                             {
                                                 Filter = "C# Library (*.dll)|*.dll|All Files (*.*)|*.*",
                                                 FilterIndex = 1,
                                                 Multiselect = false
                                             };
                    if (openFileDialog.ShowDialog() == true)
                    {
                        Status = Strings.LoadingStatus;
                        using (var fileStream = openFileDialog.File.OpenRead())
                        {
                            try
                            {
                                AppRunner.LoadApplication(fileStream);
                            }
                            catch (TypeLoadException e)
                            {
                                Status = String.Format(Strings.LoadingError + ": {0}", e.Message);
                            }
                        }
                        if (AppRunner.IsLoaded)
                        {
                            Status = Strings.LoadingImagesStatus;
                            if (openFileDialog.File.Directory != null)
                            {
                                _imageSources =
                                    new ImageSources(openFileDialog.File.Directory.FullName + "\\assets\\images");
                                SoundSources =
                                    new SoundSources(openFileDialog.File.Directory.FullName + "\\assets\\sounds", ActiveSounds, InactiveSounds);

                                AppRunner.App.Images = _imageSources.GetImageSet();

                                foreach (var cubeViewModel in CubeViewModels)
                                {
                                    cubeViewModel.ImageSources = _imageSources;
                                }

                                Status = openFileDialog.File.Name + " " + Strings.LoadingSuccess;
                                AppRunner.StartExecution(CubeSet, Application.Current.MainWindow.Dispatcher, SoundSources.SoundSet);
                                AppRunner.NotifyApplicationException += DisplayException;
                                NotifyPropertyChanged("PauseOrResumeText");
                                NotifyPropertyChanged("CanPauseOrResume");
                            }
                            else
                            {
                                Status = Strings.LoadingFail;
                            }
                        }
                    }
                    else
                    {
                        Status = Strings.LoaderClosed;
                    }
                });
            #endregion
            #region PauseOrResumeCommand

            PauseOrResumeCommand = new RelayCommand(() =>
            {
                if (AppRunner.IsRunning)
                {
                    AppRunner.PauseExecution();
                    SoundSources.PauseAllSounds(ActiveSounds, InactiveSounds);
                }
                else
                {
                    AppRunner.ResumeExecution();
                    SoundSources.ResumeAllSounds(ActiveSounds, InactiveSounds);
                }
                NotifyPropertyChanged("PauseOrResumeText");
            });
            #endregion

            AppRunner = AppRunner.GetInstance();

            CubeViewModels = new ObservableCollection<CubeViewModel>();
            AddNewCubes(NumInitialCubes);
            SnapToGridCommand.Execute(null);
            ActiveSounds = new ObservableCollection<SoundViewModel>();
            InactiveSounds = new Collection<SoundViewModel>();
            Status = ReadyStatus;
        }

        public void DisplayException(Exception e)
        {
            Status = String.Format(Strings.AppAbortedStatus + " ({0}: {1})", e.GetType(), e.Message);
            AppRunner.StopExecution();
            NotifyPropertyChanged("CanPauseOrResume");
            NotifyPropertyChanged("PauseOrResumeText");
            MessageBox.Show(String.Format(Strings.AppAbortedMessage + ":\n{0}", e), Strings.AppAbortedCaption, MessageBoxButton.OK);
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