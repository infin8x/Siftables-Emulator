using Sifteo;
using System;

namespace ChangingColorsApp
{
  public class ChangingColorsApp : BaseApp
  {

    private int currentCubeIndex;

    // called during intitialization, before the game has started to run
    override public void Setup()
    {
        this.currentCubeIndex = 0;

        foreach (Cube cube in this.CubeSet)
        {
            cube.FillScreen(Color.Black);
            cube.Paint();
        }
    }

    override public void Tick()
    {
        Random rand = new Random();
        int r = rand.Next(0, 256);
        int g = rand.Next(0, 256);
        int b = rand.Next(0, 256);
        Color color = new Color(r, g, b);

        Cube nextCube = this.CubeSet.toArray()[this.currentCubeIndex];
        nextCube.FillScreen(color);
        nextCube.Paint();

        this.currentCubeIndex = (this.currentCubeIndex + 1) % this.CubeSet.Count;
    }

    // development mode only
    // start ChangingColorsApp as an executable and run it, waiting for Siftrunner to connect
    static void Main(string[] args) { new ChangingColorsApp().Run(); }
  }
}

