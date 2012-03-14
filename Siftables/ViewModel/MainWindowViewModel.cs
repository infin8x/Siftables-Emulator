using System;
using System.Windows;
using System.Windows.Controls;
using Siftables.Sifteo;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.IO;

namespace Siftables.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region BindingDefinitions
        private ObservableCollection<CubeView> _cubes;

        public ObservableCollection<CubeView> Cubes
        {
            get { return _cubes; }

            set
            {
                if (_cubes == value) { return; }
                _cubes = value;
            }
        }

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

        private String _appLibraryPath;

        private bool _appLoaded;
        public bool AppLoaded
        {
            get { return this._appLoaded; }
            set { 
                this._appLoaded = value;
                NotifyPropertyChanged("AppLoaded");
            }
        }

        public MainWindowViewModel()
        {
            #region RelayCommandDefinitions
            #region ChangeNumberOfCubesCommand
            ChangeNumberOfCubesCommand = new RelayCommand<EventArgs>(e =>
                {
                    Status = "Changing number of cubes";
                    var args = e as RoutedPropertyChangedEventArgs<double>;
                    var numToChange = Convert.ToInt32(Math.Abs(Cubes.Count - args.NewValue));
                    if (args.NewValue < Cubes.Count) // removing cubes
                    {
                        for (int i = 0; i < numToChange; i++) {
                            Cubes.RemoveAt(Cubes.Count - 1);
                        }
                        CalculateNeighbors();
                    }
                    else if (args.NewValue > Cubes.Count) // adding cubes
                    {
                        for (int i = 0; i < numToChange; i++) {
                            var cv = new CubeView();
                            ((CubeViewModel)cv.LayoutRoot.DataContext).CubeModel.NotifyCubeMoved += (sender, arguments) => CalculateNeighbors();
                            Cubes.Add(cv); 
                        }
                        Status = ReadyStatus;
                        SnapToGridCommand.Execute(null);
                        if (AppRunner.IsRunning)
                        {
                            AppRunner.App.AssociateCubes(SiftCubeSet);
                        }
                    }
                    Status = ReadyStatus;
                });
            #endregion
            #region SnapToGridCommand
            SnapToGridCommand = new RelayCommand(() =>
                {
                    Status = "Snapping to grid";
                    for (int i = 0; i < Math.Ceiling(Cubes.Count / 4.0); i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if ((i * 4) + j > Cubes.Count - 1) { CalculateNeighbors(); Status = ReadyStatus; return; }
                            Canvas.SetLeft(Cubes[(i * 4) + j], 200 * j);
                            Canvas.SetTop(Cubes[(i * 4) + j], 200 * i);
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

                    var userClickedOk = openFileDialog.ShowDialog();

                    if (userClickedOk == true)
                    {
                        Status = "Loading application...";
                        try
                        {
                            AppRunner.LoadAssembly(openFileDialog.File.OpenRead());
                            Status = openFileDialog.File.Name + " was loaded.";
                            AppRunner.StartExecution(SiftCubeSet, Cubes[0].Dispatcher);
                            this._appLibraryPath = openFileDialog.File.FullName;
                            AppLoaded = true;
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
                if (AppRunner.IsRunning)
                {
                    AppRunner.StopExecution();
                }

                foreach(var cube in SiftCubeSet)
                {
                    cube.FillScreen(Color.Black);
                }

                Status = "Reloading application...";
                var assemblyStream = new FileStream(this._appLibraryPath, FileMode.Open);
                try
                {
                    AppRunner.LoadAssembly(assemblyStream);
                    Status = "Application was reloaded.";
                    AppRunner.StartExecution(SiftCubeSet, Cubes[0].Dispatcher);
                    AppLoaded = true;
                }
                catch (TypeLoadException)
                {
                    MessageBox.Show("Application does not contain a subclass of BaseApp.");
                    Status = "No application found.";
                }
            });
            #endregion
            #endregion

            #region CreateCubes
            Cubes = new ObservableCollection<CubeView>();
            for (int i = 0; i < 6; i++) {
                var cv = new CubeView();
                ((CubeViewModel)cv.LayoutRoot.DataContext).CubeModel.NotifyCubeMoved += (sender, arguments) => CalculateNeighbors();
                Cubes.Add(cv); 
            }
            SnapToGridCommand.Execute(null);
            #endregion

            AppRunner = AppRunner.GetInstance();
            Status = ReadyStatus;
            AppLoaded = false;
        }

        public CubeSet SiftCubeSet
        {
            get
            {
                var cubes = new CubeSet();
                foreach (var cube in Cubes)
                {
                    cubes.Add(((CubeViewModel)cube.LayoutRoot.DataContext).CubeModel);
                }

                return cubes;
            }
        }

        public void CalculateNeighbors()
        {
            var count = Cubes.Count;
            // I'd like to eliminate this loop... but we have to reset everything before we can start processing neighbors
            for (int i = 0; i < count; i++)
            {
                var c = ((CubeViewModel)Cubes[i].LayoutRoot.DataContext).CubeModel;
                c.Neighbors = new Neighbors();
            }
            for (int i = 0; i < count - 1; i++)
            {
                var aV = Cubes[i];
                for (int j = i + 1; j < count; j++)
                {
                    CubeView bV = Cubes[j];
                    // If anybody knows a better way to do this, please fix it.  The only way I could get
                    // the DP to expose its value is through its ToString method...
                    var aLeft = (int) Double.Parse(aV.GetValue(Canvas.LeftProperty).ToString());
                    var aTop = (int) Double.Parse(aV.GetValue(Canvas.TopProperty).ToString());
                    var bLeft = (int) Double.Parse(bV.GetValue(Canvas.LeftProperty).ToString());
                    var bTop = (int) Double.Parse(bV.GetValue(Canvas.TopProperty).ToString());
                    var aC = ((CubeViewModel)aV.LayoutRoot.DataContext).CubeModel;
                    var bC = ((CubeViewModel)bV.LayoutRoot.DataContext).CubeModel;
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
