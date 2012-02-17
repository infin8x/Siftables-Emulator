using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Siftables.Sifteo;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Threading;

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

        private AppRunner _appRunner;

        public AppRunner ARunner
        {
            get
            {
                return this._appRunner;
            }
            set
            {
                if (this._appRunner == value) { return; }
                else
                {
                    this._appRunner = value;
                }
            }
        }

        public MainWindowViewModel()
        {
            #region RelayCommandDefinitions
            #region ChangeNumberOfCubesCommand
            ChangeNumberOfCubesCommand = new RelayCommand<EventArgs>(e =>
                {
                    Status = "Changing number of cubes";
                    RoutedPropertyChangedEventArgs<double> args = e as RoutedPropertyChangedEventArgs<double>;
                    int numToChange = Convert.ToInt32(Math.Abs(Cubes.Count - args.NewValue));
                    if (args.NewValue < Cubes.Count) // removing cubes
                    {
                        for (int i = 0; i < numToChange; i++) {
                            Cubes.RemoveAt(Cubes.Count - 1);
                        }
                        this.CalculateNeighbors();
                    }
                    else if (args.NewValue > Cubes.Count) // adding cubes
                    {
                        for (int i = 0; i < numToChange; i++) {
                            CubeView cv = new CubeView();
                            ((CubeViewModel)cv.LayoutRoot.DataContext).CubeModel.NotifyCubeMoved += (sender, arguments) => { this.CalculateNeighbors(); };
                            Cubes.Add(cv); 
                        }
                        Status = ReadyStatus;
                        SnapToGridCommand.Execute(null);
                        if (ARunner.Running)
                        {
                            ARunner.App.AssociateCubes(SiftCubeSet);
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
                            if ((i * 4) + j > Cubes.Count - 1) { this.CalculateNeighbors(); Status = ReadyStatus; return; }
                            Canvas.SetLeft(Cubes[(i * 4) + j], 200 * j);
                            Canvas.SetTop(Cubes[(i * 4) + j], 200 * i);
                        }
                    }
                    this.CalculateNeighbors();
                    Status = ReadyStatus;
                });
            #endregion
            #region LoadAFileCommand
            LoadAFileCommand = new RelayCommand(() =>
                {
                    if (ARunner.Running)
                    {
                        ARunner.PauseExecution();
                    }
                    Status = "Loading a file";
                    // Create an instance of the open file dialog box.
                    OpenFileDialog openFileDialog = new OpenFileDialog();

                    // Set filter options and filter index.
                    openFileDialog.Filter = "C# Library (*.dll)|*.dll|All Files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;

                    openFileDialog.Multiselect = false;

                    // Call the ShowDialog method to show the dialog box.
                    bool? userClickedOK = openFileDialog.ShowDialog();

                    // Process input if the user clicked OK.
                    if (userClickedOK == true)
                    {
                        bool loaded = ARunner.LoadAssembly(openFileDialog.File.OpenRead());
                        if (loaded)
                        {
                            Status = openFileDialog.File.Name + " was loaded.";
                            ARunner.StartExecution(SiftCubeSet, Cubes[0].Dispatcher);
                        }
                        else
                        {
                            Status = openFileDialog.File.Name + " does not contain a subclass of BaseApp.";
                        }
                    }
                    else
                    {
                        Status = "Program loader was closed.";
                    }
                });
            #endregion
            // This does nothing more than restart the thread, so right now it looks like nothing is happening
            #region ReloadAFileCommand
            ReloadAFileCommand = new RelayCommand(() =>
            {
                ARunner.StopExecution();
                ARunner.StartExecution(SiftCubeSet, Cubes[0].Dispatcher);
            });
            #endregion
            #endregion

            #region CreateCubes
            Cubes = new ObservableCollection<CubeView>();
            for (int i = 0; i < 6; i++) {
                CubeView cv = new CubeView();
                ((CubeViewModel)cv.LayoutRoot.DataContext).CubeModel.NotifyCubeMoved += (sender, arguments) => { CalculateNeighbors(); };
                Cubes.Add(cv); 
            }
            SnapToGridCommand.Execute(null);
            #endregion

            ARunner = AppRunner.getInstance();
            Status = ReadyStatus;
        }

        public CubeSet SiftCubeSet
        {
            get
            {
                CubeSet cubes = new CubeSet();
                foreach (CubeView cube in Cubes)
                {
                    cubes.Add(((CubeViewModel)cube.LayoutRoot.DataContext).CubeModel);
                }

                return cubes;
            }
        }

        public void CalculateNeighbors()
        {
            int count = Cubes.Count;
            // I'd like to eliminate this loop... but we have to reset everything before we can start processing neighbors
            for (int i = 0; i < count; i++)
            {
                Cube c = ((CubeViewModel)Cubes[i].LayoutRoot.DataContext).CubeModel;
                c.Neighbors = new Neighbors();
            }
            for (int i = 0; i < count - 1; i++)
            {
                CubeView aV = Cubes[i];
                for (int j = i + 1; j < count; j++)
                {
                    CubeView bV = Cubes[j];
                    // If anybody knows a better way to do this, please fix it.  The only way I could get
                    // the DP to expose its value is through its ToString method...
                    int aLeft = (int) Double.Parse(aV.GetValue(Canvas.LeftProperty).ToString());
                    int aTop = (int) Double.Parse(aV.GetValue(Canvas.TopProperty).ToString());
                    int bLeft = (int) Double.Parse(bV.GetValue(Canvas.LeftProperty).ToString());
                    int bTop = (int) Double.Parse(bV.GetValue(Canvas.TopProperty).ToString());
                    Cube aC = ((CubeViewModel)aV.LayoutRoot.DataContext).CubeModel;
                    Cube bC = ((CubeViewModel)bV.LayoutRoot.DataContext).CubeModel;
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
