using System;
using Sifteo;
using Sifteo.Util;


namespace QuickActionGameApp
{
    public class QuickActionGame : BaseApp
    {
        private int _cubeX;
        private int _tickCount;
        private double _reqTicks;
        private Random _rand;
        private int _score;
        private bool _winning;

        private readonly String[] _imgArray = {
                                                  "tiltRight.png", "tiltLeft.png", "tiltUp.png", "tiltDown.png",
                                                  "flip.png", "pushButton.png"
                                                  //"shake.png", "rotate_cw.png", "rotate_ccw"
                                              };


        public override void Setup()
        {
            _cubeX = 35;
            _tickCount = 0;
            _reqTicks = 10;
            _rand = new Random();
            _score = 0;
            _winning = true;


            foreach (var cube in CubeSet)
            {
                cube.NotifyButtonPressed += OnPress;
                cube.NotifyCubeTilt += OnTilt;
                cube.NotifyCubeFlip += OnFlip;
//                cube.NotifyShakeStarted += OnShake;
//                cube.NotifyRotateCW += OnRotateCW;
//                cube.NotifyRotateCCW += OnRotateCCW;

                cube.userData = new CubeData(null);
                cube.FillScreen(Color.White);
                cube.Paint();
            }
        }

        private void OnPress(Cube cube)
        {
            if (String.Equals(((CubeData) cube.userData).ImgSource, _imgArray[5]))
            {
                ClearCubeData(cube);
                _score += 10;
                cube.FillScreen(Color.White);
                cube.Paint();
            }
        }

        private void OnTilt(Cube cube, Cube.Side direction)
        {
            switch (direction)
            {
                case Cube.Side.RIGHT:
                    if (String.Equals(((CubeData) cube.userData).ImgSource, _imgArray[0]))
                    {
                        ClearCubeData(cube);
                        _score += 10;
                        cube.FillScreen(Color.White);
                        cube.Paint();
                    }
                    break;
                case Cube.Side.LEFT:
                    if (String.Equals(((CubeData) cube.userData).ImgSource, _imgArray[1]))
                    {
                        ClearCubeData(cube);
                        _score += 10;
                        cube.FillScreen(Color.White);
                        cube.Paint();
                    }
                    break;
                case Cube.Side.TOP:
                    if (String.Equals(((CubeData) cube.userData).ImgSource, _imgArray[2]))
                    {
                        ClearCubeData(cube);
                        _score += 10;
                        cube.FillScreen(Color.White);
                        cube.Paint();
                    }
                    break;
                case Cube.Side.BOTTOM:
                    if (String.Equals(((CubeData) cube.userData).ImgSource, _imgArray[3]))
                    {
                        ClearCubeData(cube);
                        _score += 10;
                        cube.FillScreen(Color.White);
                        cube.Paint();
                    }
                    break;
                case Cube.Side.NONE:
                    break;
            }
        }

        private void OnFlip(Cube cube, bool neworientationisup)
        {
            if (String.Equals(((CubeData) cube.userData).ImgSource, _imgArray[4]))
            {
                ClearCubeData(cube);
                _score += 10;
                cube.FillScreen(Color.White);
                cube.Paint();
            }
        }

//        private void OnShake(Cube cube)
//        {
//            if (String.Equals(((CubeData) cube.userData).ImgSource, _imgArray[6]))
//            {
//                ClearCubeData(cube);
//                _score += 10;
//                cube.FillScreen(Color.White);
//                cube.Paint();
//            }
//        }

//        private void OnRotateCW(Cube cube, Cube.Side orientation)
//        {
//            if (String.Equals(((CubeData) cube.userData).ImgSource, _imgArray[6]))
//            {
//                ClearCubeData(cube);
//                _score += 10;
//                cube.FillScreen(Color.White);
//                cube.Paint();
//            }
//        }
//
//        private void OnRotateCCW(Cube cube, Cube.Side orientation)
//        {
//            if (String.Equals(((CubeData) cube.userData).ImgSource, _imgArray[7]))
//            {
//                ClearCubeData(cube);
//                _score += 10;
//                cube.FillScreen(Color.White);
//                cube.Paint();
//            }
//        }

        public override void Tick()
        {
            if (_winning)
            {
                foreach (var cube in CubeSet)
                {
                    var imgSource = ((CubeData) cube.userData).ImgSource;
                    if (imgSource != null)
                    {
                        ((CubeData) cube.userData).Tick();
                        if (((CubeData) cube.userData).TickLimitReached())
                        {
                            ClearCubes();
                            SetCubeBackgrounds(new Color(255, 0, 0));
                            _winning = false;
                            return;
                        }
                    }
                }
                _tickCount++;

                if (_tickCount >= _reqTicks)
                {
                    _reqTicks *= 0.95;
                    _tickCount = 0;

                    var cubeNumber = _rand.Next(CubeSet.Count);
                    var imgNumber = _rand.Next(_imgArray.Length);


                    if (String.Equals(((CubeData) CubeSet[cubeNumber].userData).ImgSource, null))
                    {
                        PlaceImage(CubeSet[cubeNumber], _imgArray[imgNumber]);
                    }
                }
            }
        }

        private void SetCubeBackgrounds(Color c)
        {
            foreach (var cube in CubeSet)
            {
                cube.FillScreen(c);
                cube.Paint();
            }
        }

        private void ClearCubes()
        {
            foreach (var cube in CubeSet)
            {
                ClearCubeData(cube);
            }
        }

        private void ClearCubeData(Cube cube)
        {
            cube.userData = new CubeData(null);
            cube.Paint();
        }

        private void PlaceImage(Cube cube, String imgSource)
        {
            cube.userData = new CubeData(imgSource);
            cube.Image(((CubeData) cube.userData).ImgSource, _cubeX, 30, 0, 0, 128, 128, 2);
            cube.Paint();
        }
    }
}