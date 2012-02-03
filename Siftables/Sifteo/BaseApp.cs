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
using System.Collections.Generic;

namespace Siftables.Sifteo
{
    public class BaseApp
    {

        private CubeSet Cubes;
        public int FrameRate;

        public BaseApp(CubeSet cubes)
        {
            this.Cubes = cubes;
            this.FrameRate = 20;
        }

        public void Setup()
        {
            // Override this method with information to initialize the application
            foreach (Cube cube in this.Cubes)
            {
                cube.FillScreen(Colors.Red);
            }
        }

        public void Tick()
        {
            // Override this method with periodic changes that are made when the application is running
            Random rand = new Random();
            foreach (Cube cube in Cubes)
            {
                cube.FillScreen(Color.FromArgb(255, (byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)));
                cube.FillRect(Color.FromArgb(255, (byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)), 25, 25, 25, 25);
                cube.FillRect(Color.FromArgb(255, (byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)), 78, 25, 25, 25);
                cube.FillRect(Color.FromArgb(255, (byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)), 25, 85, 78, 15);
            }
        }
    }
}
