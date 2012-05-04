using System;
using System.Collections.Generic;
using System.IO;
using Sifteo;

namespace WordApp
{
    public class WordApp : BaseApp
    {
        private static List<string> _dictionary;
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

            var wordToUse = PickWord();

            foreach (var cube in CubeSet)
            {
                cube.userData = new CubeData();
                if ("" != wordToUse)
                {
                    ((CubeData)cube.userData).SetCubeChar(c: wordToUse.Substring(0, 1));
                    wordToUse = wordToUse.Remove(0, 1);
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
            _cubeCount = CubeSet.Count;
            _dictionary = new List<string>();

            AddWordsToDictionary();
        }

        private static string PickWord()
        {
            var wordToUse = "";

            // this should be randomized
            foreach (var word in _dictionary)
            {
                if (word.Length > _cubeCount) continue;
                wordToUse = word;
                break;
            }

            if ("" == wordToUse)
                wordToUse = "tracks";

            return wordToUse;
        }

        private static void ChangeCubeImage(Cube cube, Color color)
        {
            cube.FillScreen(color);
            var cubeChar = ((CubeData) cube.userData).CubeChar;
            string imageSource;

            if ("" != cubeChar)
                imageSource = cubeChar + ".png";
            else
                imageSource = "wat.png";

//           using (var sw = new StreamWriter(path: "C:/Users/thairp/workspace/Siftables-Emulator/Applications/WordApp/WordApp/bin/Debug/assets/images/debug_info.txt"))
//           {
//               sw.WriteLine("Shitty debugger info:");
//               sw.WriteLine(imageSource);
//           }
            
            cube.Image(imageSource, x: _cubeX, y: _cubeY, sourceX: _sourceX, sourceY: _sourceY, w: _width, h: _height,
                       scale: _scale, rotation: _rotation);
            cube.Paint();
        }

        private static void AddWordsToDictionary()
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
        }

        public override void Tick()
        {
//            CheckWord();
        }

        private static void CheckWord()
        {
            // set the positioning of each cube so that we know what char to look for
            // (no left neighbors means you're first)

            // once you know your position, add your letter to your internal array
            // of letters

            // check to see if your internal array matches with a word in the
            // dictionary.  yes?  turn green + play sound + update points
        }
    }
}