using System;
using Sifteo;

namespace ChangingColorsApp
{
  public class ChangingColorsApp : BaseApp
  {
      private Sound _fourSound;

      override public void Setup()
      {
          var b = 0;
          var a = 3/b;
          foreach (var cube in CubeSet)
          {
              cube.FillScreen(Color.White);
              cube.Paint();
          }
          PauseEvent += Setup;
          UnpauseEvent += () =>
                              {
                                  foreach (var cube in CubeSet)
                                  {
                                      cube.FillScreen(Color.Black);
                                      cube.Paint();
                                  }
                              };
      }

      public override void Tick()
      {
          foreach (var c in CubeSet)
          {
              var num = c.Neighbors.Count;
              switch (num)
              {
                  case 0:
                      c.FillScreen(Color.Black);
                      if (_fourSound != null && !_fourSound.IsPaused)
                      {
                          _fourSound.Pause();
                      }
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
                      if (_fourSound == null)
                      {
                          _fourSound = Sounds.CreateSound("scoobydoo.mp3");
                          _fourSound.Play((float) 0.75);
                      } else if (_fourSound.IsPaused)
                      {
                          _fourSound.Resume();
                      }
                      break;
              }
              var rand = new Random();
              c.FillRect(new Color(rand.Next(256), rand.Next(256), rand.Next(256)), 50, 50, 50, 50);
              c.Paint();
          }
      }
  }
}

