using Sifteo;

namespace SimonSaysApp
{
    internal class CubeData
    {
        private int _color;

        public CubeData(int color)
        {
            _color = color;
        }

        public int GetCubeColor()
        {
            return _color;
        }

        public void SetCubeColor(int color)
        {
            _color = color;
        }
    }
}