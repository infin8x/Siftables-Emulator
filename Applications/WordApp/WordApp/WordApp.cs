using System.Collections.Generic;
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
                    ((CubeData) cube.userData).SetCubeChar(c: wordToUse.Substring(0, 1));
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
            _dictionary.Add("as");
            _dictionary.Add("a");
        }

        public override void Tick()
        {
            foreach (var cube in CubeSet)
            {
                CheckWord(cube);
            }
        }

        private void CheckWord(Cube cube)
        {
            var myData = (CubeData) cube.userData;
            var myWord = myData.GetCubeChar();

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
            var s = Sounds.CreateSound("Success.mp3");
            s.Play(1);
        }
    }
}