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

namespace Siftables
{
    public class MyApp : BaseApp
    {
        override public void Setup()
        {
            foreach (Cube cube in Cubes)
            {
                cube.FillScreen(Colors.White);
            }
        }

        public override void Tick()
        {
            base.Tick();
            foreach (Cube c in this.Cubes)
            {
                int num = c.Neighbors.Count;
                if (num == 0) c.FillScreen(Colors.Black);
                else if (num == 1) c.FillScreen(Colors.Red);
                else if (num == 2) c.FillScreen(Colors.Yellow);
                else if (num == 3) c.FillScreen(Colors.Blue);
                else if (num == 4) c.FillScreen(Colors.Green);
            }
        }
    }
}
