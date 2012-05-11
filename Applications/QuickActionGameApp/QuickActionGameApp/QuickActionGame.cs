using System;
using Sifteo;
using Sifteo.Util;


namespace QuickActionGameApp
{
    public class QuickActionGame : BaseApp
    {
        private int _cubeX;
        private int _tickCount;
        private int _reqTicks;
        private Random _rand;
        private int _score;

        private readonly String[] _imgArray = {
                                                  "tiltRight.png", "tiltLeft.png", "tiltUp.png", "tiltDown.png",
                                                  "flip.png", "rotate_cw.png",
                                                  "rotate_ccw.png", "pushButton.png", "shake.png"
                                              };

        public override void Setup()
        {
            _cubeX = 35;
            _tickCount = 0;
            _reqTicks = 10;
            _rand = new Random();
            _score = 0;


            foreach (var cube in CubeSet)
            {
                cube.NotifyButtonPressed += OnPress;
                cube.NotifyCubeTilt += OnTilt;
                cube.NotifyCubeFlip += OnFlip;
                cube.NotifyShakeStarted += OnShake;
                cube.NotifyRotateCW += OnRotateCW;
                cube.NotifyRotateCCW += OnRotateCCW;
                
                cube.userData = new CubeData(null);
                cube.FillScreen(Color.White);
                cube.Paint();
            }
        }

        private void OnPress(Cube cube)
        {
            if (String.Equals(((CubeData) cube.userData).ImgSource, _imgArray[7]))
            {
                ClearCubeData(cube);
                _score += 10;
            }
        }

        private void OnTilt(Cube cube, Cube.Side direction)
        {
            //TODO
        }

        private void OnFlip(Cube cube, bool neworientationisup)
        {
            if (String.Equals(((CubeData)cube.userData).ImgSource, _imgArray[4]))
            {
                ClearCubeData(cube);
                _score += 10;
            }
        }

        private void OnShake(Cube cube)
        {
            if (String.Equals(((CubeData)cube.userData).ImgSource, _imgArray[8]))
            {
                ClearCubeData(cube);
                _score += 10;
            }
        }

        private void OnRotateCW(Cube cube, Cube.Side orientation)
        {
            if (String.Equals(((CubeData)cube.userData).ImgSource, _imgArray[5]))
            {
                ClearCubeData(cube);
                _score += 10;
            }
        }

        private void OnRotateCCW(Cube cube, Cube.Side orientation)
        {
            if (String.Equals(((CubeData)cube.userData).ImgSource, _imgArray[6]))
            {
                ClearCubeData(cube);
                _score += 10;
            }
        }

        public override void Tick()
        {
            foreach (var cube in CubeSet)
            {
                string imgSource = ((CubeData) cube.userData).ImgSource;
                if (imgSource != null)
                {
                    ((CubeData) cube.userData).Tick();
                    if (((CubeData) cube.userData).TickLimitReached())
                    {
                        //TODO Loss conditions

                        ClearCubes();
                        SetCubeBackgrounds(new Color(255, 0, 0));
                    }

//                    switch (imgSource)
//                    {
//                        case "tiltRight":
//                            clearImgSource(cube);
//                            break;
//                        case "tiltLeft":
//                            clearImgSource(cube);
//                            break;
//                        case "tiltUp":
//                            clearImgSource(cube);
//                            break;
//                        case "tiltDown":
//                            clearImgSource(cube);
//                            break;
//                        case "flip":
//                            clearImgSource(cube);
//                            break;
//                        case "rotateRight":
//                            clearImgSource(cube);
//                            break;
//                        case "rotateLeft":
//                            clearImgSource(cube);
//                            break;
//                        case "pressButton":
//                            clearImgSource(cube);
//                            break;
//                        case "shake":
//                            clearImgSource(cube);
//                            break;
//                    }
                }
            }
            _tickCount++;

            if (_tickCount >= _reqTicks)
            {
                _reqTicks = (int) (_reqTicks*0.95);
                _tickCount = 0;

                var cubeNumber = _rand.Next(CubeSet.Count);
                var imgNumber = _rand.Next(_imgArray.Length);


                if (((CubeData) CubeSet[cubeNumber].userData).ImgSource == null)
                {
//                    CubeSet[cubeNumber].FillScreen(new Color(0, 0, 255));
//                    CubeSet[cubeNumber].Paint();
                    PlaceImage(CubeSet[cubeNumber], _imgArray[imgNumber]);
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
            cube.Image(imgSource, _cubeX, 35, 0, 0, 128, 128, 2);
            cube.Paint();
        }
    }
}