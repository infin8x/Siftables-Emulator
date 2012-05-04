using System;
using Sifteo;


namespace QuickActionGameApp
{
    public class QuickActionGame : BaseApp
    {
        private int _cubeX;
        private int _tickCount;
        private int _reqTicks;
        private Random _rand;

        private readonly String[] _imgArray = {
                                                  "tiltRight", "tiltLeft", "tiltUp", "tiltDown", "flip", "rotateRight",
                                                  "rotateLeft", "pressButton", "shake"
                                              };

        public override void Setup()
        {
            _cubeX = 35;
            _tickCount = 0;
            _reqTicks = 80;
            _rand = new Random();


            foreach (var cube in CubeSet)
            {
                cube.FillScreen(Color.White);
                cube.userData = new CubeData(null);
            }
        }

        public override void Tick()
        {
            foreach (var cube in CubeSet)
            {
                string imgSource = ((CubeData) cube.userData).ImgSource;
                if (imgSource != null)
                {
                    switch (imgSource)
                    {
                        case "tiltRight":
                            clearImgSource(cube);
                            break;
                        case "tiltLeft":
                            clearImgSource(cube);
                            break;
                        case "tiltUp":
                            clearImgSource(cube);
                            break;
                        case "tiltDown":
                            clearImgSource(cube);
                            break;
                        case "flip":
                            clearImgSource(cube);
                            break;
                        case "rotateRight":
                            clearImgSource(cube);
                            break;
                        case "rotateLeft":
                            clearImgSource(cube);
                            break;
                        case "pressButton":
                            clearImgSource(cube);
                            break;
                        case "shake":
                            clearImgSource(cube);
                            break;
                    }
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
                    PlaceImage(CubeSet[cubeNumber], _imgArray[imgNumber]);
                }
            }
        }

        private void clearImgSource(Cube cube)
        {
            ((CubeData) cube.userData).ImgSource = null;
        }

        private void PlaceImage(Cube cube, String imgSource)
        {
            cube.userData = new CubeData(imgSource);
            cube.Image(((CubeData) cube.userData).ImgSource, _cubeX, 0, 0, 0, 128, 128, 2, 0);
            cube.Paint();
        }
    }
}