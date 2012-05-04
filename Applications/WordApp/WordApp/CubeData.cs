namespace WordApp
{
    internal class CubeData
    {
        public string CubeChar;
        public int LetterIndex;
        public string Word;

        public CubeData(string cubeChar = "", int letterIndex = 0, string word = "")
        {
            CubeChar = cubeChar;
            LetterIndex = letterIndex;
            Word = word;
        }

        public void SetLetterIndex(int i)
        {
            LetterIndex = i;
        }

        public int GetLetterIndex()
        {
            return LetterIndex;
        }

        public void SetCubeChar(string c)
        {
            CubeChar = c;
        }

        public string GetCubeChar()
        {
            return CubeChar;
        }

        public void SetWord(string w)
        {
            Word = w;
        }

        public string GetWord()
        {
            return Word;
        }
    }
}