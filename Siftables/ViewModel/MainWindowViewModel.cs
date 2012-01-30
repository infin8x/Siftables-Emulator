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

namespace Siftables.ViewModel
{
    public class MainWindowViewModel
    {
        #region BindingDefinitions
        private ObservableCollection<Cube> _cubes;

        public ObservableCollection<Cube> Cubes
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

        #endregion

        public MainWindowViewModel()
        {
            Cubes = new ObservableCollection<Cube>();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Cube cube = new Cube();
                    Canvas.SetLeft(cube, 200 * j);
                    Canvas.SetTop(cube, 200 * i);

                    Cubes.Add(cube);
                }
            }

            ChangeNumberOfCubesCommand = new RelayCommand<EventArgs>(e =>
                {
                    RoutedPropertyChangedEventArgs<double> args = e as RoutedPropertyChangedEventArgs<double>;
                    int numToChange = Convert.ToInt32(Math.Abs(Cubes.Count - args.NewValue));
                    if (args.NewValue < Cubes.Count) // removing cubes
                    {
                        for (int i = 0; i < numToChange; i++) { Cubes.RemoveAt(Cubes.Count - 1); }

                    }
                    else if (args.NewValue > Cubes.Count) // adding cubes
                    {
                        for (int i = 0; i < numToChange; i++) { Cubes.Add(new Cube()); }
                    }
                });

            SnapToGridCommand = new RelayCommand(() =>
              {
                  for (int i = 0; i < Math.Ceiling(Cubes.Count / 4.0); i++)
                  {
                      for (int j = 0; j < 4; j++)
                      {
                          try
                          {
                              //Canvas.SetLeft(cubes[(i * 4) + j], 200 * j);
                              //Canvas.SetTop(cubes[(i * 4) + j], 200 * i);
                          }
                          catch (ArgumentOutOfRangeException exception)
                          {
                              break;
                          }
                      }
                  }
              });

            LoadAFileCommand = new RelayCommand(() =>
                {
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
                        MessageBox.Show("File " + openFileDialog.File.Name + " was selected.");
                    }
                    else
                    {
                        MessageBox.Show("Cancel was pressed.");
                    }
                });
        }
    }
}
