using System;
using Siftables.Sifteo;

namespace ChangingColorsApp
{
  public class ChangingColorsApp : BaseApp
  {

      private string[] _imageNames;
      private Random _random;

      override public void Setup()
      {
          foreach (var cube in CubeSet)
          {
              cube.FillScreen(Color.White);
              cube.Paint();
          }
          _imageNames = new string[] {"flip.png", "cube.png", "tilt.png", "rotate_cw.png", "rotate_ccw.png"};
          _random = new Random();
      }

      public override void Tick()
      {
          foreach (var c in this.CubeSet)
          {
              var num = c.Neighbors.Count;
              switch (num)
              {
                  case 0:
                      c.FillScreen(Color.Black);
                      break;
                  case 1:
                      c.FillScreen(new Color(255, 0, 0)); // red
                      break;
                  case 2:
                      c.FillScreen(new Color(255, 255, 0)); // yellow
                      break;
                  case 3:
                      c.FillScreen(new Color(0, 0, 255)); // blue
                      break;
                  case 4:
                      c.FillScreen(new Color(0, 255, 0)); // green
                      break;
              }
              c.Image(_imageNames[_random.Next(_imageNames.Length)]);
              c.Paint();
          }
      }
  }
}
