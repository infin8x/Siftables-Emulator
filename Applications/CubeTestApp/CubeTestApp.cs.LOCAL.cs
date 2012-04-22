using Sifteo;

namespace CubeTestApp
{
    public class CubeTestApp : BaseApp
    {
        public override void Setup()
        {
            foreach (Cube cube in CubeSet)
            {
                cube.FillScreen(Color.White);
                cube.Paint();

                cube.NotifyButtonPressed += OnPress;
                cube.NotifyCubeTilt += OnTilt;
                cube.NotifyCubeFlip += OnFlip;
            }
        }

        private void OnPress(Cube cube, bool pressed)
        {
            cube.FillScreen(new Color(255, 0, 0)); // red
            cube.Paint();
        }

        private void OnTilt(Cube cube, Cube.Side direction)
        {
            switch (direction)
            {
                case Cube.Side.TOP:
                    cube.FillScreen(new Color(0, 200, 0)); // increasingly dark green
                    break;

                case Cube.Side.BOTTOM:
                    cube.FillScreen(new Color(0, 150, 0));
                    break;

                case Cube.Side.LEFT:
                    cube.FillScreen(new Color(0, 100, 0));
                    break;

                case Cube.Side.RIGHT:
                    cube.FillScreen(new Color(0, 50, 0));
                    break;
                default:
                    cube.FillScreen(Color.White); // extremely light green
                    break;
            }

            cube.Paint();
        }

        private void OnFlip(Cube cube, bool newOrientationIsUp)
        {
            cube.FillScreen(new Color(0, 0, 255)); // blue
            cube.Paint();
        }

        public override void Tick()
        {
        }
    }
}