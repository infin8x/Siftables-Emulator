
namespace Siftables.Sifteo
{
    //public class Neighbors
    //{
    //    private int _numNeighbors;
    //    private Cube[] _neighbors = new Cube[4];

    //    public Neighbors()
    //    {
    //        this._numNeighbors = 0;
    //    }

    //    public Cube.Side SideOf(Cube c)
    //    {
    //        return Cube.Side.NONE;

    //    }

    //    public bool Contains(Cube c)
    //    {
    //        return false;
    //    }

    //    public Cube CubeOnSide(Cube.Side s)
    //    {
    //        return this._neighbors[(int)s];
    //    }

    //    public Cube TOP
    //    {
    //        get { return this._neighbors[(int)Cube.Side.TOP]; }
    //        set { this.sideUtil(Cube.Side.TOP, value); }
    //    }

    //    public Cube RIGHT
    //    {
    //        get { return this._neighbors[(int)Cube.Side.RIGHT]; }
    //        set { this.sideUtil(Cube.Side.RIGHT, value); }
    //    }

    //    public Cube BOTTOM
    //    {
    //        get { return this._neighbors[(int)Cube.Side.BOTTOM]; }
    //        set { this.sideUtil(Cube.Side.BOTTOM, value); }
    //    }

    //    public Cube LEFT
    //    {
    //        get { return this._neighbors[(int)Cube.Side.LEFT]; }
    //        set { this.sideUtil(Cube.Side.LEFT, value); }
    //    }

    //    private void sideUtil(Cube.Side s, Cube value) {
    //        int index = (int) s;
    //        if (value == null && this._neighbors[index] != null) this._numNeighbors--;
    //        else if (value != null && this._neighbors[index] == null) this._numNeighbors++;
    //        this._neighbors[index] = value;   
    //    }

    //    public int Count
    //    {
    //        get { return this._numNeighbors; }
    //    }

    //    public bool IsEmpty
    //    {
    //        get { return this._numNeighbors == 0; }
    //    }


    //}
}
