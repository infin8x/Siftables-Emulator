namespace WordApp
{
    internal class CubeData
    {
        public string CubeChar;
        public int LetterIndex;
        public string Word;
        public bool SpellChecked;

        public CubeData(string cubeChar = "", int letterIndex = 0, string word = "", bool spellChecked = false)
        {
            CubeChar = cubeChar;
            LetterIndex = letterIndex;
            Word = word;
            SpellChecked = spellChecked;
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

        public void SetSpellChecked(bool b)
        {
            SpellChecked = b;
        }

        public bool GetSpellChecked()
        {
            return SpellChecked;
        }
    }
}