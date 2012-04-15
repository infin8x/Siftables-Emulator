namespace Sifteo
{
    public struct Color
    {
        private byte _r;
        public byte R {
            get
            {
                return this._r;
            }
        }

        private byte _g;
        public byte G
        {
            get
            {
                return this._g;
            }
        }

        private byte _b;
        public byte B
        {
            get
            {
                return this._b;
            }
        }

        public Color(byte r, byte g, byte b)
        {
            this._r = r;
            this._g = g;
            this._b = b;
        }

        public static Color Black
        {
            get
            {
                return new Color(0, 0, 0);
            }
        }

        public static Color White {
            get
            {
                return new Color(255, 255, 255);
            }
        }
    }
}
