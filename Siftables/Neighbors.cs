
namespace Siftables
{
    public class Neighbors
    {
        private int _numNeighbors;
        private Cube[] _neighbors = new Cube[4];

        public Neighbors()
        {
            this._numNeighbors = 0;
        }

        public Cube.Side SideOf(Cube c)
        {
            return Cube.Side.None;

        }

        public bool Contains(Cube c)
        {
            return false;
        }

        public Cube CubeOnSide(Cube.Side s)
        {
            return this._neighbors[(int)s];
        }

        public Cube Top
        {
            get { return this._neighbors[(int)Cube.Side.Top]; }
            set { this.sideUtil(Cube.Side.Top, value); }
        }

        public Cube Right
        {
            get { return this._neighbors[(int)Cube.Side.Right]; }
            set { this.sideUtil(Cube.Side.Right, value); }
        }

        public Cube Bottom
        {
            get { return this._neighbors[(int)Cube.Side.Bottom]; }
            set { this.sideUtil(Cube.Side.Bottom, value); }
        }

        public Cube Left
        {
            get { return this._neighbors[(int)Cube.Side.Left]; }
            set { this.sideUtil(Cube.Side.Left, value); }
        }

        private void sideUtil(Cube.Side s, Cube value) {
            int index = (int) s;
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
