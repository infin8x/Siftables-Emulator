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
        List<OldCube> cubes = new List<OldCube>();
        //int numCubes = 0;
        public MainWindow()
        {
            InitializeComponent();
            //Dispatcher.BeginInvoke(() =>
            //{
            //    double width = workspace.ActualWidth;
            //    double height = workspace.ActualHeight;
            //    for (int i = 0; i < 2; i++)
            //    {
            //        for (int j = 0; j < 3; j++)
            //        {
            //            double left = (width / 3) * j;
            //            double top = (height / 2) * i;
            //            cubes.Add(new OldCube(workspace, left, top));
            //            numCubes++;
            //        }
            //    }
            //});
        }
    }
}
