using System;

// I don't know how to get the initial cube set to pass to the constructor...
public class OrderGame : BaseApp
{

    private Dictionary<Cube, Integer> _order;
    private Color _green;
    private Color _red;

    public OrderGame(List<Cube> cubes)
	{
        super(cubes);
        this._green = Color.FromArgb(255, 0, 0, 255);
        this._red = Color.FromArgb(255, 255, 0, 0);
	}

    public void Setup()
    {
        this._order = new Dictionary<Cube, Integer>();
        int i = 0;
        foreach (Cube cube in this.Cubes)
        {
            cube.FillScreen(this._red);
            this._order.add(cube, i);
            this.i++;
        }
    }

    // TODO display the number on the screen, maybe?
    public void Tick()
    {
        foreach (Cube cube in this.Cubes)
        {
            Cube left = cube.Neighbors.Left;
            Cube right = cube.Neighbors.Right;
            int order = this._order.Get(cube);
            if ((left == null && order == 0) || (right == null && order == this.Cubes.Size))
            {
                cube.FillScreen(this._green);
            }
            else if ((this._order.Get(left) < order) && (this._order.Get(right) > order))
            {
                cube.FillScreen(this._green);
            }
            else
            {
                cube.FillScreen(this._red);
            }
        }
    }
}

