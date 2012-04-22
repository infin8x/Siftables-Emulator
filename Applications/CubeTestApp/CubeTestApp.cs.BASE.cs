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
            cube.FillScreen(new Color(0, 255, 0)); // green
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