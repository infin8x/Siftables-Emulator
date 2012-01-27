using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;
using System.Runtime.InteropServices;

namespace Siftables
{
    public partial class MainWindow : UserControl
    {
        List<Cube> cubes;
        public MainWindow()
        {
            InitializeComponent();
            cubes = new List<Cube>(9);
            workspace.Loaded += new RoutedEventHandler(MainWindow_Loaded);

            

            numberOfCubesUpDown.ValueChanged += new RoutedPropertyChangedEventHandler<double>(numberOfCubesUpDown_ValueChanged);
            //numberOfCubesSlider.DataBinding = cubes.Count;
        }

        void numberOfCubesUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int numToChange = Convert.ToInt32(Math.Abs(e.OldValue - e.NewValue));
            if (e.NewValue < e.OldValue) // removing cubes
            { 
                cubes.RemoveRange(cubes.Count - numToChange, numToChange);
            }
            else if (e.NewValue > e.OldValue) // adding cubes
            {

            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Cube cube = new Cube();
                    Canvas.SetLeft(cube, 200 * j);
                    Canvas.SetTop(cube, 200 * i);
                    workspace.Children.Add(cube);
                    cubes.Add(cube);
                }
            }
            numberOfCubesUpDown.Value = cubes.Count;
        }

        private void loadAProgramButton_Click(object sender, RoutedEventArgs e)
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
        }
    }
}
