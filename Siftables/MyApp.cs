using Siftables.Sifteo;
using System;

namespace Siftables
{
    public class MyApp : BaseApp
    {
        private int _currentCubeIndex;

        override public void Setup()
        {
            this._currentCubeIndex = 0;
            foreach (Cube cube in Cubes)
            {
                cube.FillScreen(Color.White);
            }
        }

        override public void Tick()
        {
            Random rand = new Random();
            int r = rand.Next(0, 256);
            int g = rand.Next(0, 256);
            int b = rand.Next(0, 256);
            Color color = new Color((byte) r, (byte) g, (byte) b);

            Cube nextCube = this.Cubes[this._currentCubeIndex];
            nextCube.FillScreen(color);

            this._currentCubeIndex = (this._currentCubeIndex + 1) % this.Cubes.Count;
        }
    }
}
