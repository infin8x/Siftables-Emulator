using SifteoDomain;
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

        foreach (Cube cube in this.Cubes)
        {
            cube.FillScreen(Color.Black);
            //cube.Paint();
        }
    }

    override public void Tick()
    {
        Random rand = new Random();
        int r = rand.Next(0, 256);
        int g = rand.Next(0, 256);
        int b = rand.Next(0, 256);
        Color color = new Color((byte) r, (byte) g, (byte) b);

        Cube nextCube = this.Cubes[this.currentCubeIndex];
        nextCube.FillScreen(color);
        //nextCube.Paint();

        this.currentCubeIndex = (this.currentCubeIndex + 1) % this.Cubes.Count;
    }
  }
}

