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

        private List<Cube> Cubes;
        public int FrameRate;

        public BaseApp(List<Cube> cubes)
        {
            this.Cubes = cubes;
            this.FrameRate = 20;
        }

        public void Setup()
        {
            // Override this method with information to initialize the application
            foreach (Cube cube in this.Cubes)
            {
                cube.FillScreen(Color.FromArgb(255, 0, 255, 0));
            }
        }

        public void Tick()
        {
            // Override this method with periodic changes that are made when the application is running
            Random rand = new Random();
            int cubeIndex = rand.Next(this.Cubes.Count);
            foreach (Cube cube in this.Cubes)
            {
                cube.FillScreen(Color.FromArgb(255, 0, 0, 0));
            }
            //this.Cubes.ToArray()[cubeIndex].FillScreen(Color.FromArgb(255, 255, 255, 255));
        }
    }
}
