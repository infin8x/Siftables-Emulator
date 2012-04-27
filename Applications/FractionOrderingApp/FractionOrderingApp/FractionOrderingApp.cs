using System;
using Sifteo;

namespace FractionOrderingApp
{
    public class FractionOrderingApp : BaseApp
    {
        public Random Rand = new Random();
        private int _cubeX;
        private int _cubeYDen;
        private int _cubeYNum;
        private int _height;
        private string _imageSourceDen;
        private string _imageSourceNum;
        private int _rotation;
        private int _scale;
        private int _sourceX;
        private int _sourceY;
        private int _width;

        public override void Setup()
        {
            _imageSourceNum = "";
            _imageSourceDen = "";
            _cubeX = 35;
            _cubeYNum = 0;
            _cubeYDen = 70;
            _sourceX = 0;
            _sourceY = 0;
            _width = 140;
            _height = 140;
            _scale = 1;
            _rotation = 0;

            foreach (Cube cube in CubeSet)
            {
                Fraction fraction = new Fraction(Rand.Next(0, 11), Rand.Next(1, 11));
                
                ChangeCubeImg(cube, fraction, Color.White);
                cube.userData = fraction;
            }
        }

        private void ChangeCubeImg(Cube cube, Fraction fraction, Color color)
        {
            cube.FillScreen(color);

            switch (fraction.GetNumerator())
            {
                case 0:
                    _imageSourceNum = "zero.png";
                    break;
                case 1:
                    _imageSourceNum = "one.png";
                    break;
                case 2:
                    _imageSourceNum = "two.png";
                    break;
                case 3:
                    _imageSourceNum = "three.png";
                    break;
                case 4:
                    _imageSourceNum = "four.png";
                    break;
                case 5:
                    _imageSourceNum = "five.png";
                    break;
                case 6:
                    _imageSourceNum = "six.png";
                    break;
                case 7:
                    _imageSourceNum = "seven.png";
                    break;
                case 8:
                    _imageSourceNum = "eight.png";
                    break;
                case 9:
                    _imageSourceNum = "nine.png";
                    break;
                case 10:
                    _imageSourceNum = "ten.png";
                    break;
            }

            switch (fraction.GetDenominator())
            {
                case 1:
                    _imageSourceDen = "one.png";
                    break;
                case 2:
                    _imageSourceDen = "two.png";
                    break;
                case 3:
                    _imageSourceDen = "three.png";
                    break;
                case 4:
                    _imageSourceDen = "four.png";
                    break;
                case 5:
                    _imageSourceDen = "five.png";
                    break;
                case 6:
                    _imageSourceDen = "six.png";
                    break;
                case 7:
                    _imageSourceDen = "seven.png";
                    break;
                case 8:
                    _imageSourceDen = "eight.png";
                    break;
                case 9:
                    _imageSourceDen = "nine.png";
                    break;
                case 10:
                    _imageSourceDen = "ten.png";
                    break;
            }

            cube.Image(_imageSourceNum, _cubeX, _cubeYNum, _sourceX, _sourceY, _width, _height, _scale,
                               _rotation);
            cube.Image(_imageSourceDen, _cubeX, _cubeYDen, _sourceX, _sourceY, _width, _height, _scale,
                               _rotation);
            cube.FillRect(Color.Black, _cubeX, _cubeYDen - 10, 65, 10);
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
                 ((Fraction) cube.userData).GreaterThen((Fraction) cube.Neighbors.Left.userData)) &&
                (null == cube.Neighbors.Right ||
                 ((Fraction) cube.Neighbors.Right.userData).GreaterThen((Fraction) cube.userData)))
            {
                ChangeCubeImg(cube, (Fraction) cube.userData, new Color(0, 255, 0)); // green
//                PlaySuccessSound();
            }
            else
            {
                ChangeCubeImg(cube, (Fraction) cube.userData, new Color(255, 0, 0)); // red
            }
        }

        private void PlaySuccessSound()
        {
            Sound s = Sounds.CreateSound("gliss");
            s.Play(1);
        }
    }
}