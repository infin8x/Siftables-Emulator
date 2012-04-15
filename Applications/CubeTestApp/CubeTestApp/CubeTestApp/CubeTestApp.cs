using Sifteo;

namespace CubeTestApp
{
    public class CubeTestApp : BaseApp
    {
        public override void Setup()
        {
            foreach (var c in CubeSet)
            {
                c.FillScreen(Color.White);
            }
        }

        public override void Tick()
        {
            
        }
    }

    /*public class CubeWrapper
    {
        private readonly CubeTestApp app;
        public Cube cube;
        private bool justPressed;

        public CubeWrapper(CubeTestApp app, Cube cube)
        {
            this.app = app;
            this.cube = cube;
            
            this.cube.NotifyButtonPressed += this.OnPress;
//            this.cube.NotifyCubeTilt += this.OnTilt;
//            this.cube.NotifyCubeFlip += this.onFlip;
        }

        private void OnPress(Cube cube, bool pressed)
        {
            if (!justPressed)
            {
                cube.FillScreen(new Color(255, 0, 0)); // red
            }

            justPressed = !justPressed;
        }

//        private void OnTilt(Cube cube, Cube.Side direction)
//        {
//            if (!justTilted)
//            {
//                Console.WriteLine("Cube was tilted");
//            }
//
//            justTilted = !justTilted;
//        }
//
//        private void OnFlip(Cube cube, bool newOrientationIsUp)
//        {
//            if (!justFlipped)
//            {
//                Console.WriteLine("Cube was flipped");
//            }
//
//            justFlipped = !justFlipped;
//        }
    }*/
}
