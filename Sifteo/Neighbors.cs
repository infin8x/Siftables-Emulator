
namespace Sifteo
{
    public class Neighbors
    {
        // The maximunm distance apart two cubes can be and still be considered neighbors
        public static int GAP_TOLERANCE = (int) (Cube.dimension / 1.5);
        // The minimum length of a shared edge required for cubes to be considered neighbors
        public static int SHARED_EDGE_MINIMUM = (int) (Cube.dimension / 1.5);

        private int _numNeighbors;
        private Cube[] _neighbors;

        public Neighbors()
        {
            this._numNeighbors = 0;
            this._neighbors = new Cube[4];
        }

        public Cube.Side SideOf(Cube c)
        {
            if (this._neighbors[(int)Cube.Side.TOP] == c) return Cube.Side.TOP;
            if (this._neighbors[(int)Cube.Side.RIGHT] == c) return Cube.Side.RIGHT;
            if (this._neighbors[(int)Cube.Side.LEFT] == c) return Cube.Side.LEFT;
            if (this._neighbors[(int)Cube.Side.BOTTOM] == c) return Cube.Side.BOTTOM;
            return Cube.Side.NONE;
        }

        public bool Contains(Cube c)
        {
            for (int i = 0; i < 4; i++)
            {
                if (this._neighbors[i] == c)
                    return true;
            }
            return false;
        }

        public Cube CubeOnSide(Cube.Side s)
        {
            return this._neighbors[(int)s];
        }

        public Cube Top
        {
            get { return this._neighbors[(int)Cube.Side.TOP]; }
            set { this.sideUtil(Cube.Side.TOP, value); }
        }

        public Cube Right
        {
            get { return this._neighbors[(int)Cube.Side.RIGHT]; }
            set { this.sideUtil(Cube.Side.RIGHT, value); }
        }

        public Cube Bottom
        {
            get { return this._neighbors[(int)Cube.Side.BOTTOM]; }
            set { this.sideUtil(Cube.Side.BOTTOM, value); }
        }

        public Cube Left
        {
            get { return this._neighbors[(int)Cube.Side.LEFT]; }
            set { this.sideUtil(Cube.Side.LEFT, value); }
        }

        private void sideUtil(Cube.Side s, Cube value)
        {
            int index = (int)s;
            if (value == null && this._neighbors[index] != null) this._numNeighbors--;
            else if (value != null && this._neighbors[index] == null) this._numNeighbors++;
            this._neighbors[index] = value;
        }

        public int Count
        {
            get { return this._numNeighbors; }
        }

        public bool IsEmpty
        {
            get { return this._numNeighbors == 0; }
        }


    }
}
