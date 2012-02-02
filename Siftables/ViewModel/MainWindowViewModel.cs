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

namespace Siftables.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region BindingDefinitions
        private CubeSet _cubes;

        public CubeSet Cubes
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

        public RelayCommand<EventArgs> ChangeNumberOfCubesCommand { get; private set; }

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

        public MainWindowViewModel()
        {
            #region RelayCommandDefinitions
            ChangeNumberOfCubesCommand = new RelayCommand<EventArgs>(e =>
                {
                    Status = "Changing number of cubes";
                    RoutedPropertyChangedEventArgs<double> args = e as RoutedPropertyChangedEventArgs<double>;
                    int numToChange = Convert.ToInt32(Math.Abs(Cubes.Count - args.NewValue));
                    if (args.NewValue < Cubes.Count) // removing cubes
                    {
                        for (int i = 0; i < numToChange; i++) { Cubes.RemoveAt(Cubes.Count - 1); }

                    }
                    else if (args.NewValue > Cubes.Count) // adding cubes
                    {
                        for (int i = 0; i < numToChange; i++) { Cubes.Add(new Cube()); }
                        Status = ReadyStatus;
                        SnapToGridCommand.Execute(null);
                    }
                    Status = ReadyStatus;
                });

            SnapToGridCommand = new RelayCommand(() =>
                {
                    Status = "Snapping to grid";
                    for (int i = 0; i < Math.Ceiling(Cubes.Count / 4.0); i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if ((i * 4) + j > Cubes.Count - 1) { Status = ReadyStatus;  return; }
                            Canvas.SetLeft(Cubes[(i * 4) + j], 200 * j);
                            Canvas.SetTop(Cubes[(i * 4) + j], 200 * i);
                        }
                    }
                    Status = ReadyStatus;
                });

            LoadAFileCommand = new RelayCommand(() =>
                {
                    Status = "Loading a file";
                    // Create an instance of the open file dialog box.
                    OpenFileDialog openFileDialog = new OpenFileDialog();

                    // Set filter options and filter index.
                    openFileDialog.Filter = "C# Files (.cs)|*.cs|All Files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;

                    openFileDialog.Multiselect = false;

                    // Call the ShowDialog method to show the dialog box.
                    bool? userClickedOK = openFileDialog.ShowDialog();

                    // Process input if the user clicked OK.
                    if (userClickedOK == true)
                    {
                        Status = openFileDialog.File.Name + " was loaded.";
                    }
                    else
                    {
                        Status = "Program loader was closed.";
                    }
                });
            #endregion

            #region CreateCubes
            Cubes = new CubeSet();
            for (int i = 0; i < 6; i++) { Cubes.Add(new Cube()); }
            SnapToGridCommand.Execute(null);
            #endregion

            Status = ReadyStatus;
        }
    }
}
