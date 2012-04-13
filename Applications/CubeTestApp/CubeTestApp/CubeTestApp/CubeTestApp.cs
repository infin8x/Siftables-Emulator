using System;
using System.Collections.Generic;
using Siftables.Sifteo;

namespace CubeTestApp
{
    public class CubeTestApp : BaseApp
    {
        
    }

    public class CubeWrapper
    {
        private readonly CubeTestApp app;
        public Cube cube;
        private bool justPressed;

        public CubeWrapper(CubeTestApp app, Cube cube)
        {
            this.app = app;
            this.cube = cube;
            
            this.cube.ButtonEvent += this.OnPress;
            this.cube.TiltEvent += this.OnTilt;
            this.cube.FlipEvent += this.onFlip;
        }

        private void OnPress(Cube cube, bool press)
        {
            if (!justPressed)
            {
                Console.WriteLine("Button was pressed");
            }

            justPressed = !justPressed;
        }

        private void OnTilt(Cube cube, bool tilt)
        {
            if (!justTilted)
            {
                Console.WriteLine("Cube was tilted");
            }

            justTilted = !justTilted;
        }

        private void OnFlip(Cube cube, bool flip)
        {
            if (!justFlipped)
            {
                Console.WriteLine("Cube was flipped");
            }

            justFlipped = !justFlipped;
        }
    }
}

}