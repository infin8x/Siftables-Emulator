using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Sifteo;

namespace WordApp
{
    public class WordApp : BaseApp
    {
        private static Random Rand;
        private static List<string> _dictionary;
        private static string _dictionaryFile;
        private static string _wordToUse;
        private static int _cubeCount;
        private static int _cubeX;
        private static int _cubeY;
        private static int _height;
        private static int _rotation;
        private static int _scale;
        private static int _sourceX;
        private static int _sourceY;
        private static int _width;

        public override void Setup()
        {
            InitCubeFields();

            GenerateDictionary();

            foreach (Cube cube in CubeSet)
            {
                cube.userData = new CubeData();
                if ("" != _wordToUse)
                {
                    ((CubeData) cube.userData).SetCubeChar(c: _wordToUse.Substring(0, 1));
                    _wordToUse = _wordToUse.Remove(0, 1);
                }

                ChangeCubeImage(cube, Color.White);
            }
        }

        private void InitCubeFields()
        {
            _cubeX = 10;
            _cubeY = 10;
            _sourceX = 0;
            _sourceY = 0;
            _width = 130;
            _height = 130;
            _scale = 1;
            _rotation = 0;
            _wordToUse = "";
            _cubeCount = CubeSet.Count;
            // check public assets folder of WordApp
            _dictionaryFile =
                "C:/Users/thairp/workspace/Siftables-Emulator/Applications/WordApp/WordApp/assets/Sift-tionary.dic";
            _dictionary = new List<string>();
            Rand = new Random();

            AddWordsToDictionary(true);
        }

        private static string PickWord()
        {
            int randomWordIndex = Rand.Next(0, _dictionary.Count);

            while (_dictionary[randomWordIndex].Length != _cubeCount)
            {
                randomWordIndex = Rand.Next(0, _dictionary.Count);
            }

            return ScrambleWord(_dictionary[randomWordIndex]);
        }

        private static string ScrambleWord(string s)
        {
            string unscrambled = s;
            string scrambled = "";

            while (unscrambled != "")
            {
                int randomLetterIndex = Rand.Next(0, unscrambled.Length);
                scrambled += unscrambled.Substring(randomLetterIndex, 1);
                unscrambled = unscrambled.Remove(randomLetterIndex, 1);
            }

            return scrambled;
        }

        private static void ChangeCubeImage(Cube cube, Color color)
        {
            cube.FillScreen(color);
            string cubeChar = ((CubeData) cube.userData).CubeChar;
            string imageSource;

            if ("" != cubeChar)
                imageSource = cubeChar + ".png";
            else
                imageSource = "wat.png";

            cube.Image(imageSource, x: _cubeX, y: _cubeY, sourceX: _sourceX, sourceY: _sourceY, w: _width, h: _height,
                       scale: _scale, rotation: _rotation);
            cube.Paint();
        }

        private void GenerateDictionary()
        {
            _wordToUse = PickWord();

            using (var sr = new StreamReader(_dictionaryFile))
            {
                string line;

                while (null != (line = sr.ReadLine()))
                {
                    line = line.Trim();
                    bool wordIsContained = true;

                    foreach (char c in line)
                    {
                        // TODO
                        // this doesn't check for proper character frequency in the words
                        // meaning there are words which will never be checked since there
                        // aren't enough letters for them
                        if (!_wordToUse.Contains(c.ToString(CultureInfo.InvariantCulture)))
                            wordIsContained = false;
                    }

                    if (wordIsContained)
                        _dictionary.Add(line);
                }
            }
        }

        private static void AddWordsToDictionary(bool dictionaryAvailable)
        {
            if (dictionaryAvailable)
            {
                using (var sr = new StreamReader(_dictionaryFile))
                {
                    string line;

                    while (null != (line = sr.ReadLine()))
                    {
                        line = line.Trim();
                        if (line.Length <= _cubeCount)
                            _dictionary.Add(line);
                    }
                }
            }
            else
            {
                _dictionary.Add("tracks");
                _dictionary.Add("track");
                _dictionary.Add("stack");
                _dictionary.Add("racks");
                _dictionary.Add("scat");
                _dictionary.Add("cats");
                _dictionary.Add("rack");
                _dictionary.Add("car");
                _dictionary.Add("arc");
                _dictionary.Add("at");
                _dictionary.Add("as");
                _dictionary.Add("a");
            }
        }

        public override void Tick()
        {
            foreach (Cube cube in CubeSet)
            {
                CheckWord(cube);
            }
        }

        private void CheckWord(Cube cube)
        {
            var myData = (CubeData) cube.userData;
            string myWord = myData.GetCubeChar();

            if (null != cube.Neighbors.Right)
            {
                var rightData = ((CubeData) cube.Neighbors.Right.userData);
                myWord += rightData.GetWord();
            }

            myData.SetWord(myWord);

            if (null != cube.Neighbors.Left)
            {
                var leftData = ((CubeData) cube.Neighbors.Left.userData);
                myData.SetSpellChecked(leftData.IsSpellChecked());
            }
            else
            {
                myData.SetSpellChecked(_dictionary.Contains(myWord));
            }

            if (myData.IsSpellChecked())
            {
                myData.SetSpellChecked(true);
                ChangeCubeImage(cube, new Color(0, 200, 0)); // green for good
                PlaySuccessSound();
            }
            else
            {
                myData.SetSpellChecked(false);
                ChangeCubeImage(cube, Color.White);
            }
        }

        private void PlaySuccessSound()
        {
            Sound s = Sounds.CreateSound("Success.mp3");
            s.Play(1);
        }
    }
}