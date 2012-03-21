using System;
using System.Windows;
using System.Windows.Controls;
using Siftables.Sifteo;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;

namespace Siftables.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public const int NumInitialCubes = 6;

        #region BindingDefinitions
        public ObservableCollection<CubeViewModel> CubeViewModels { get; set; } 

        public RelayCommand SnapToGridCommand { get; private set; }
        public RelayCommand LoadAFileCommand { get; private set; }
        public RelayCommand ReloadAFileCommand { get; private set; }
        public RelayCommand<EventArgs> ChangeNumberOfCubesCommand { get; private set; }

        public RelayCommand RefreshNeighborsCommand { get; private set; }

        private String _status;
        public String Status
        {
            get { return _status; }

            set
            {
                if (_status == value) { return; }
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }
        public const String ReadyStatus = "Ready";

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion

        public AppRunner AppRunner { get; private set; }

        private ImageSources _imageSources;

        public MainWindowViewModel()
        {
            #region RelayCommandDefinitions
            #region ChangeNumberOfCubesCommand
            ChangeNumberOfCubesCommand = new RelayCommand<EventArgs>(e =>
                {
                    Status = "Changing number of cubes";
                    var args = e as RoutedPropertyChangedEventArgs<double>;
                    var numToChange = Convert.ToInt32(Math.Abs(CubeViewModels.Count - args.NewValue));
                    if (args.NewValue < CubeViewModels.Count) // removing cubes
                    {
                        for (var i = 0; i < numToChange; i++) {
                            CubeViewModels.RemoveAt(CubeViewModels.Count - 1);
                        }
                        CalculateNeighbors();
                    }
                    else if (args.NewValue > CubeViewModels.Count) // adding cubes
                    {
                        for (var i = 0; i < numToChange; i++) {
                            var cubeViewModel = new CubeViewModel();
                            cubeViewModel.CubeModel.NotifyCubeMoved += (sender, arguments) => CalculateNeighbors();
                            CubeViewModels.Add(cubeViewModel);

                            if (AppRunner.IsRunning)
                            {
                                cubeViewModel.ImageSources = _imageSources;
                            }
                        }
                        Status = ReadyStatus;
                        SnapToGridCommand.Execute(null);
                        if (AppRunner.IsRunning)
                        {
                            AppRunner.App.AssociateCubes(CubeSet);
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
                        try
                        {
                            using (var fileStream = openFileDialog.File.OpenRead())
                            {
                                AppRunner.LoadApplication(fileStream);
                            }

                            Status = "Loading image resources...";
                            _imageSources = new ImageSources(openFileDialog.File.Directory.FullName + "/assets/images");
                            AppRunner.App.Images = _imageSources.GetImageSet();
                            
                            foreach (var cubeViewModel in CubeViewModels)
                            {
                                cubeViewModel.ImageSources = _imageSources;
                            }

                            Status = openFileDialog.File.Name + " was loaded.";
                            AppRunner.StartExecution(CubeSet, Application.Current.MainWindow.Dispatcher);
                        }
                        catch (TypeLoadException)
                        {
                            MessageBox.Show(openFileDialog.File.Name + " does not contain a subclass of BaseApp.");
                            Status = "No application found.";
                        }
                    }
                    else
                    {
                        Status = "Program loader was closed.";
                    }
                });
            #endregion
            #region ReloadAFileCommand
            ReloadAFileCommand = new RelayCommand(() =>
            {
                throw new NotImplementedException();
            });
            #endregion
            #endregion

            #region CreateCubes
            CubeViewModels = new ObservableCollection<CubeViewModel>();
            for (var i = 0; i < NumInitialCubes; i++) {
                var cubeViewModel = new CubeViewModel();
                cubeViewModel.CubeModel.NotifyCubeMoved += (sender, arguments) => CalculateNeighbors();
                CubeViewModels.Add(cubeViewModel);
            }
            SnapToGridCommand.Execute(null);
            #endregion

            AppRunner = AppRunner.GetInstance();
            Status = ReadyStatus;
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
                            aC.Neighbors.RIGHT = bC;
                            bC.Neighbors.LEFT = aC;
                        }
                        else
                        {
                            aC.Neighbors.LEFT = bC;
                            bC.Neighbors.RIGHT = aC;
                        }
                    }
                    if ((Math.Abs(aTop - bTop) <= (Neighbors.GAP_TOLERANCE + Cube.dimension)) && (Math.Abs(aLeft - bLeft) <= (Cube.dimension - Neighbors.SHARED_EDGE_MINIMUM)))
                    {
                        if (aTop < bTop)
                        {
                            aC.Neighbors.TOP = bC;
                            bC.Neighbors.BOTTOM = aC;
                        }
                        else
                        {
                            aC.Neighbors.BOTTOM = bC;
                            bC.Neighbors.TOP = aC;
                        }
                    }
                }
            }
        }

    }
}
