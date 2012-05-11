using Siftables.Sifteo;
using System;
using System.Collections.Generic;
using Sifteo;

namespace ReflexSifteoApp
{
  public class ReflexSifteoApp : BaseApp
  {

      private CubeWrapper coloredCubeWrapper;
      private int currentFrame;
      private int lastFrame;
      private Random rand;
      private List<CubeWrapper> CubeWrapperSet = new List<CubeWrapper>(0);
      private int framesBetweenUpdates;
      private bool lost;

    public int FrameRate
    {
      get { return 20; }
    }

    // Used only for initializing variables
    override public void Setup()
    {
        this.lastFrame = 0;
        this.currentFrame = 0;
        this.framesBetweenUpdates = 40;
        this.rand = new Random();
        this.lost = false;

        foreach (Cube cube in this.Cubes) 
        {
            CubeWrapper wrapper = new CubeWrapper(this, cube);
            CubeWrapperSet.Add(wrapper);
        }

    }

    // Tick() is going to take care of updating the cubes
    override public void Tick()
    {
        if (!this.lost)
        {
            this.currentFrame++;
            if (this.currentFrame == this.framesBetweenUpdates)
            {
                // Reset colored cube to black and paint a random cube
                this.Repaint();

                // Make this the most recent update
                this.lastFrame = this.currentFrame;
                Console.WriteLine("What a failure!");
            }
            else if (this.currentFrame - this.lastFrame == this.framesBetweenUpdates)
            {
                this.YouLose("Out of time");
            }
        }
    }

    public void YouLose(String reason)
    {
        Color losingColor = new Color(200, 0, 0);
        foreach (CubeWrapper cubeWrapper in this.CubeWrapperSet)
        {
            cubeWrapper.cube.FillScreen(losingColor);
            //cubeWrapper.cube.Paint();
        }
        this.lost = true;
        Console.WriteLine(reason);
    }

    public void ResetCube(CubeWrapper cubeWrapper)
    {
        cubeWrapper.cube.FillScreen(new Color(0, 0, 0));
        //cubeWrapper.cube.Paint();
    }

    public void Repaint()
    {
        // Paint colored cube black
        if (this.lastFrame > 0) this.ResetCube(this.coloredCubeWrapper);

        // Paint a random cube a random color
        int r = (this.currentFrame * 5 % 206) + 50;
        int g = (this.currentFrame * 4 % 206) + 50;
        int b = (this.currentFrame * 3 % 206) + 50;
        CubeWrapper[] arrCubes = this.CubeWrapperSet.ToArray();
        this.coloredCubeWrapper = arrCubes[this.rand.Next(0, arrCubes.Length)];
        Color color = new Color((byte)r, (byte)g, (byte)b);
        this.coloredCubeWrapper.cube.FillScreen(color);
        //this.coloredCubeWrapper.cube.Paint();
    }

    public void Answer(Cube cube)
    {
        if (!this.lost && cube.Equals(this.coloredCubeWrapper.cube))
        {
            this.lastFrame = this.currentFrame;
            this.Repaint();
            this.framesBetweenUpdates--;
            if (this.framesBetweenUpdates < 5) this.framesBetweenUpdates = 3;
        }
        else
        {
            this.YouLose("Wrong cube");
        }
    }

    /* development mode only
    // start ReflexSifteoApp as an executable and run it, waiting for Siftrunner to connect
    static void Main(string[] args) { new ReflexSifteoApp().Run(); }*/
  }

  public class CubeWrapper
  {
      private ReflexSifteoApp app;
      public Cube cube;
      private bool justPressed;

      public CubeWrapper(ReflexSifteoApp app, Cube cube)
      {
          this.app = app;
          this.cube = cube;
          //this.cube.ButtonEvent += this.OnPress;
      }

      private void OnPress(Cube cube, bool press)
      {
          if (!this.justPressed)
          {
              Console.WriteLine("Button pressed");
              app.Answer(cube);
          }
          this.justPressed = !this.justPressed;
      }
  }
}

