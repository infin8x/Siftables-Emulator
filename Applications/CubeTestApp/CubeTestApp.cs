using System;
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
                cube.NotifyShakeStarted += OnShake;
                cube.NotifyShakeStopped += OffShake;
            }
        }

        private void OnPress(Cube cube)
        {
            cube.FillScreen(new Color(255, 0, 0)); // red
            cube.Paint();
        }

        private void OnTilt(Cube cube, Cube.Side direction)
        {
            switch (direction)
            {
                case Cube.Side.TOP:
                    cube.FillRect(new Color(0, 200, 0), 54, 54, 20, 20); // increasingly dark green
                    break;

                case Cube.Side.BOTTOM:
                    cube.FillRect(new Color(0, 150, 0), 54, 54, 20, 20);
                    break;

                case Cube.Side.LEFT:
                    cube.FillRect(new Color(0, 100, 0), 54, 54, 20, 20);
                    break;

                case Cube.Side.RIGHT:
                    cube.FillRect(new Color(0, 50, 0), 54, 54, 20, 20);
                    break;
                default:
                    cube.FillRect(Color.White, 54, 54, 20, 20); // extremely light green
                    break;
            }

            cube.Paint();
        }

        private void OnFlip(Cube cube, bool newOrientationIsUp)
        {
            cube.FillRect(new Color(0, 0, 255), 54, 54, 20, 20); // blue
            cube.Paint();
        }

        private void OnShake(Cube cube)
        {
            Random rand = new Random();
            cube.FillRect(new Color(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)), 54, 54, 20, 20);
            cube.Paint();
        }

        private void OffShake(Cube cube, int duration)
        {
            cube.FillScreen(Color.White);
            cube.Paint();
        }

        public override void Tick()
        {
            foreach (var c in this.CubeSet)
            {
                var num = c.Neighbors.Count;
                switch (num)
                {
                    case 0:
                        c.FillScreen(Color.Black);
                        break;
                    case 1:
                        c.FillScreen(new Color(255, 255, 0)); // yellow
                        break;
                    case 2:
                        c.FillScreen(new Color(255, 255, 50)); // color?
                        break;
                    case 3:
                        c.FillScreen(new Color(255, 255, 100)); // color?
                        break;
                    case 4:
                        c.FillScreen(new Color(255, 255, 150)); // color?
                        break;
                }
                c.Paint();
            }
        }
    }
}