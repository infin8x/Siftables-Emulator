using System;
using System.Collections.Generic;
using Sifteo;

namespace SimonSaysApp
{
    internal class SimonSaysApp : BaseApp
    {
        private static List<Color> _colors;
        private static List<int> _cubeToPressList;
        private static int _cubeHighlightWidth;
        private static int _cubeToHighlightIndex;
        private static int _currentFrame;
        private static int _timeDelay;
        private static int _turn;
        private static bool _lose;
        private static bool _started;

        public int FrameRate
        {
            get { return 20; } // half second for every frame rate of 20
        }

        private void InitCubeFields()
        {
            _colors = new List<Color>
                          {
                              new Color(34, 139, 34),
                              // Forest Green
                              new Color(220, 20, 60),
                              // Crimson Red
                              new Color(255, 215, 0),
                              // Golden Yellow
                              new Color(70, 130, 180)
                              // Steel Blue
                          };

            _cubeToPressList = new List<int>();

            _cubeHighlightWidth = 10;
            _cubeToHighlightIndex = -1;
            _currentFrame = 0;
            _timeDelay = 0;
            _turn = 0;
            _lose = false;
            _started = false;
        }

        public override void Setup()
        {
            if (4 != CubeSet.Count)
                return;

            InitCubeFields();
            int i = 0;

            foreach (Cube cube in CubeSet)
            {
                cube.userData = new CubeData(i);
                ChangeCubeImage(cube, _colors[i]);

                cube.NotifyButtonPressed += OnPress;

                ++i;
            }
        }

        private void OnPress(Cube cube)
        {
            int cubeColor = ((CubeData) cube.userData).GetCubeColor();
            
            // if we've just started or this is correct
            if (!_started || cubeColor == _cubeToPressList[_turn])
            {
                HighlightSuccessCube(cube);
                PlaySuccessCubeSound(cubeColor);
                ++_turn;
                DelayCubeRepaint();

                if (!_started)
                {
                    _cubeToPressList.Add(cubeColor);
                    _started = true;
                }
                    

                if (_turn == _cubeToPressList.Count)
                {
                    AddRandomColorToList();
                    RunHighlightRoutine();
                }
            }
                // this is the wrong cube to press
            else
            {
                HighlightFailCube(cube);
                _lose = true;
            }
        }

        private void RunHighlightRoutine()
        {
            ++_cubeToHighlightIndex;
        }

        private void AddRandomColorToList()
        {
            var Rand = new Random();
            _cubeToPressList.Add(Rand.Next(0, 5));
        }

        private void DelayCubeRepaint()
        {
            _timeDelay = 1; // 1 second
        }

        private void HighlightCube(Cube cube)
        {
            Color highlight = Color.White;

            cube.FillRect(highlight, 0, 0, Cube.SCREEN_WIDTH, _cubeHighlightWidth); // top red
            cube.FillRect(highlight, 0, Cube.SCREEN_HEIGHT - _cubeHighlightWidth, Cube.SCREEN_WIDTH,
                          _cubeHighlightWidth); // bottom red
            cube.FillRect(highlight, 0, 0, _cubeHighlightWidth, Cube.SCREEN_HEIGHT); // left red
            cube.FillRect(highlight, Cube.SCREEN_WIDTH - _cubeHighlightWidth, 0, _cubeHighlightWidth,
                          Cube.SCREEN_HEIGHT); // right red
            cube.Paint();
        }

        private void HighlightSuccessCube(Cube cube)
        {
            cube.FillRect(new Color(153, 255, 0), 0, 0, Cube.SCREEN_WIDTH, _cubeHighlightWidth); // top green
            cube.FillRect(new Color(153, 255, 0), 0, Cube.SCREEN_HEIGHT - _cubeHighlightWidth, Cube.SCREEN_WIDTH,
                          _cubeHighlightWidth); // bottom green
            cube.FillRect(new Color(153, 255, 0), 0, 0, _cubeHighlightWidth, Cube.SCREEN_HEIGHT); // left green
            cube.FillRect(new Color(153, 255, 0), Cube.SCREEN_WIDTH - _cubeHighlightWidth, 0, _cubeHighlightWidth,
                          Cube.SCREEN_HEIGHT); // right green
            cube.Paint();
        }

        private void HighlightFailCube(Cube cube)
        {
            cube.FillRect(new Color(204, 0, 0), 0, 0, Cube.SCREEN_WIDTH, _cubeHighlightWidth); // top red
            cube.FillRect(new Color(204, 0, 0), 0, Cube.SCREEN_HEIGHT - _cubeHighlightWidth, Cube.SCREEN_WIDTH,
                          _cubeHighlightWidth); // bottom red
            cube.FillRect(new Color(204, 0, 0), 0, 0, _cubeHighlightWidth, Cube.SCREEN_HEIGHT); // left red
            cube.FillRect(new Color(204, 0, 0), Cube.SCREEN_WIDTH - _cubeHighlightWidth, 0, _cubeHighlightWidth,
                          Cube.SCREEN_HEIGHT); // right red
            cube.Paint();
        }

        private void PlaySuccessCubeSound(int cubeColor)
        {
            Sound s;
            switch (cubeColor)
            {
                case 0:
                    s = Sounds.CreateSound("Green.mp3");
                    s.Play(1);
                    break;
                case 1:
                    s = Sounds.CreateSound("Red.mp3");
                    s.Play(1);
                    break;
                case 2:
                    s = Sounds.CreateSound("Yellow.mp3");
                    s.Play(1);
                    break;
                case 3:
                    s = Sounds.CreateSound("Blue.mp3");
                    s.Play(1);
                    break;
                default:
                    s = Sounds.CreateSound("Bad.mp3");
                    s.Play(1);
                    break;
            }
        }

        private static void ChangeCubeImage(Cube cube, Color color)
        {
            cube.FillScreen(color);
            cube.Paint();
        }

        public override void Tick()
        {
            CheckDelay();
        }

        private void CheckDelay()
        {




            // are we expecting to delay?
            if (_timeDelay > 0)
            {
                // we've waiting long enough
                if (_currentFrame == _timeDelay)
                {
                    int i = 0;

                    // repaint the cubes
                    foreach (Cube cube in CubeSet)
                    {
                        ChangeCubeImage(cube, _colors[i]);
                        ++i;
                    }

                    // did we lose?
                    if (_lose)
                    {
                        InitCubeFields();
                        return;
                    }

                    // are we highlighting cubes in the routine?
                    if (_cubeToHighlightIndex > -1 && 
                             _cubeToHighlightIndex < _cubeToPressList.Count)
                    {
                        foreach (Cube cube in CubeSet)
                        {
                            int cubeColor = ((CubeData) cube.userData).GetCubeColor();
                            if (_cubeToPressList[_cubeToHighlightIndex] == cubeColor)
                            {
                                HighlightCube(cube);
                                DelayCubeRepaint();
                                ++_cubeToHighlightIndex;

                                break;
                            }
                        }
                    }

                    // did we just highlight the last cube in the routine?
                    if (_cubeToHighlightIndex >= _cubeToPressList.Count)
                    {
                        _cubeToHighlightIndex = -1;
                        _turn = 0;
                    }

                    // reset the time delay and frame
                    _timeDelay = 0;
                    _currentFrame = 0;
                }

                else
                {
                    ++_currentFrame;
                }
            }
        }
    }
}