using System;
using Sifteo;

namespace FractionOrderingApp
{
    public class FractionOrderingApp : BaseApp
    {
        public Random Rand = new Random();

        public override void Setup()
        {
            foreach (Cube cube in CubeSet)
            {
                int img = Rand.Next(0, 11);
                ChangeCubeImg(cube, img, Color.White);
            }
        }

        private void ChangeCubeImg(Cube cube, int img, Color color)
        {
            cube.FillScreen(color);

            switch (img)
            {
                case 0:
                    cube.Image("zero.png", 30, 0, 0, 0, 200, 200, 1, 0);
                    cube.userData = img;
                    break;
                case 1:
                    cube.Image("one.png", 30, 0, 0, 0, 200, 200, 1, 0);
                    cube.userData = img;
                    break;
                case 2:
                    cube.Image("two.png", 30, 0, 0, 0, 200, 200, 1, 0);
                    cube.userData = img;
                    break;
                case 3:
                    cube.Image("three.png", 30, 0, 0, 0, 200, 200, 1, 0);
                    cube.userData = img;
                    break;
                case 4:
                    cube.Image("four.png", 30, 0, 0, 0, 200, 200, 1, 0);
                    cube.userData = img;
                    break;
                case 5:
                    cube.Image("five.png", 30, 0, 0, 0, 200, 200, 1, 0);
                    cube.userData = img;
                    break;
                case 6:
                    cube.Image("six.png", 30, 0, 0, 0, 200, 200, 1, 0);
                    cube.userData = img;
                    break;
                case 7:
                    cube.Image("seven.png", 30, 0, 0, 0, 200, 200, 1, 0);
                    cube.userData = img;
                    break;
                case 8:
                    cube.Image("eight.png", 30, 0, 0, 0, 200, 200, 1, 0);
                    cube.userData = img;
                    break;
                case 9:
                    cube.Image("nine.png", 30, 0, 0, 0, 200, 200, 1, 0);
                    cube.userData = img;
                    break;
                case 10:
                    cube.Image("ten.png", 30, 0, 0, 0, 200, 200, 1, 0);
                    cube.userData = img;
                    break;
            }

            cube.Paint();
        }


        public override void Tick()
        {
            foreach (Cube cube in CubeSet)
            {
                CheckNeighbors(cube);
            }
        }

        private void CheckNeighbors(Cube cube)
        {
            if ((null == cube.Neighbors.Left ||
                 (int)cube.Neighbors.Left.userData <= (int)cube.userData) &&
                (null == cube.Neighbors.Right ||
                 (int)cube.Neighbors.Right.userData >= (int)cube.userData))
            {
                ChangeCubeImg(cube, (int)cube.userData, new Color(0, 255, 0)); // green
                PlaySuccessSound();
            }
            else
            {
                ChangeCubeImg(cube, (int)cube.userData, new Color(255, 0, 0)); // red
            }
        }

        private void PlaySuccessSound()
        {
            Sound s = Sounds.CreateSound("gliss");
            s.Play(1);
        }
    }
}