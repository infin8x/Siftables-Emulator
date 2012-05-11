using System;
using System.Globalization;
using System.Linq;
using Sifteo;
using Sifteo.Util;

namespace FractionOrderingApp
{
    public class FractionOrderingApp : BaseApp
    {
        public Random Rand = new Random();
        private bool _completelyOrdered;
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

            foreach (var cube in CubeSet)
            {
                var fraction = new Fraction(Rand.Next(1, 11), Rand.Next(1, 11));

                ChangeCubeImg(cube, fraction, Color.White);
                cube.userData = new CubeData(fraction);
            }

            foreach (var cube in CubeSet)
            {
                CheckNeighborsAreOrdered(cube);
            }

            CheckEverythingOrdered();
        }

        private void ChangeCubeImg(Cube cube, Fraction fraction, Color color)
        {
            cube.FillScreen(color);

//            _imageSourceNum = fraction.GetNumerator() + ".png";
//            _imageSourceDen = fraction.GetDenominator() + ".png";
            switch (fraction.GetNumerator())
            {
                case 1:
                    _imageSourceNum = "1.png";
                    break;
                case 2:
                    _imageSourceNum = "2.png";
                    break;
                case 3:
                    _imageSourceNum = "3.png";
                    break;
                case 4:
                    _imageSourceNum = "4.png";
                    break;
                case 5:
                    _imageSourceNum = "5.png";
                    break;
                case 6:
                    _imageSourceNum = "6.png";
                    break;
                case 7:
                    _imageSourceNum = "7.png";
                    break;
                case 8:
                    _imageSourceNum = "8.png";
                    break;
                case 9:
                    _imageSourceNum = "9.png";
                    break;
                case 10:
                    _imageSourceNum = "10.png";
                    break;
            }

            switch (fraction.GetDenominator())
            {
                case 1:
                    _imageSourceDen = "1.png";
                    break;
                case 2:
                    _imageSourceDen = "2.png";
                    break;
                case 3:
                    _imageSourceDen = "3.png";
                    break;
                case 4:
                    _imageSourceDen = "4.png";
                    break;
                case 5:
                    _imageSourceDen = "5.png";
                    break;
                case 6:
                    _imageSourceDen = "6.png";
                    break;
                case 7:
                    _imageSourceDen = "7.png";
                    break;
                case 8:
                    _imageSourceDen = "8.png";
                    break;
                case 9:
                    _imageSourceDen = "9.png";
                    break;
                case 10:
                    _imageSourceDen = "10.png";
                    break;
            }

            cube.Image(_imageSourceNum, _cubeX, _cubeYNum, _sourceX, _sourceY, _width, _height, _scale,
                       _rotation);
            cube.Image(_imageSourceDen, _cubeX, _cubeYDen, _sourceX, _sourceY, _width, _height, _scale,
                       _rotation);
            cube.FillRect(Color.Black, _cubeX, _cubeYDen - 10, 65, 10);
            cube.Paint();
        }

        private void CheckEverythingOrdered()
        {
            var totalCubes = CubeSet.Count;
            var row = CubeHelper.FindRow(CubeSet);

            if (row.Length == totalCubes)
            {
                if (row.Any(cube => !((CubeData) cube.userData).IsOrdered()))
                {
                    _completelyOrdered = false;
                    return;
                }

                if (!_completelyOrdered)
                {
                    _completelyOrdered = true;
                    PlaySuccessSound();
                }
            }
            else
            {
                _completelyOrdered = false;
            }
        }

        public override void Tick()
        {
            foreach (var cube in CubeSet)
            {
                CheckNeighborsAreOrdered(cube);
            }
            CheckEverythingOrdered();
        }

        private void CheckNeighborsAreOrdered(Cube cube)
        {
            if ((null == cube.Neighbors.Left ||
                 ((CubeData) cube.userData).GreaterThan(((CubeData) cube.Neighbors.Left.userData))) &&
                (null == cube.Neighbors.Right ||
                 ((CubeData) cube.Neighbors.Right.userData).GreaterThan((CubeData) cube.userData)))
            {
                ChangeCubeImg(cube, ((CubeData) cube.userData).GetFraction(), new Color(0, 255, 0)); // green
                if (!((CubeData) cube.userData).IsOrdered())
                {
                    PlayGoodSound();
                }
                ((CubeData) cube.userData).SetOrder(true);
            }
            else
            {
                ChangeCubeImg(cube, ((CubeData) cube.userData).GetFraction(), new Color(255, 0, 0)); // red
                if (((CubeData) cube.userData).IsOrdered())
                {
                    PlayBadSound();
                }
                ((CubeData) cube.userData).SetOrder(false);
            }
        }

        private void PlayGoodSound()
        {
            var s = Sounds.CreateSound("Good.mp3");
            s.Play(1);
        }

        private void PlayBadSound()
        {
            var s = Sounds.CreateSound("Bad.mp3");
            s.Play(1);
        }

        private void PlaySuccessSound()
        {
            var s = Sounds.CreateSound("Success.mp3");
            s.Play(1);
        }
    }
}