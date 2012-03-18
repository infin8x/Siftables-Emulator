using System;

namespace Siftables.Sifteo
{
    public class BackgroundEventArgs : EventArgs
    {
        public Color BackgroundColor { get; private set; }

        public BackgroundEventArgs(Color backgroundColor)
        {
            BackgroundColor = backgroundColor;
        }
    }

    public class RectangleEventArgs : EventArgs
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Color Color { get; private set; }

        public RectangleEventArgs(Color color, int x, int y, int width, int height)
        {
            Color = color;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }

    public class ImageEventArgs : EventArgs
    {
        public string ImageName { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int SourceX { get; private set; }
        public int SourceY { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Scale { get; private set; }
        public int Rotation { get; private set; }

        public ImageEventArgs(string imageName, int x, int y, int sourceX, int sourceY, int width, int height, int scale, int rotation)
        {
            ImageName = imageName;
            X = x;
            Y = y;
            SourceX = sourceX;
            SourceY = sourceY;
            Width = width;
            Height = height;
            Scale = scale;
            Rotation = rotation;
        }
    }

    public class Cube
    {

        public static int dimension = 128;

        public void OnMove()
        {
            NotifyCubeMoved(this, new EventArgs());
        }

        public void OnFlip()
        {
            NotifyCubeFlip(this, true);
        }

        #region Public Types
        public enum Side { TOP = 0, LEFT = 1, BOTTOM = 2, RIGHT = 3, NONE = 4 }
        #endregion

        #region Public Member Functions
        public void FillScreen(Color color)
        {
            NotifyScreenItemsEmptied(this, new EventArgs());
            NotifyBackgroundColorChanged(this, new BackgroundEventArgs(color));
        }

        public void FillRect(Color color, int x, int y, int width, int height)
        {
            if ((x > SCREEN_WIDTH) || (y > SCREEN_HEIGHT)) return;

            if (x < SCREEN_MIN_X)
            {
                width += x;
                x = 0;
            }

            if (y < SCREEN_MIN_Y)
            {
                height += y;
                y = 0;
            }

            if (width > SCREEN_WIDTH)
            {
                width = SCREEN_WIDTH;
            }

            if (y > SCREEN_HEIGHT)
            {
                height = SCREEN_HEIGHT;
            }

            NotifyNewRectangle(this, new RectangleEventArgs(color, x, y, width, height));
        }

        // scale and rotation not taken into account yet
        public void Image(String name, int x = 0, int y = 0, int sourceX = 0, int sourceY = 0, int w = SCREEN_WIDTH, int h = SCREEN_HEIGHT, int scale = 1, int rotation = 0)
        {
            if ((x > SCREEN_WIDTH) || (y > SCREEN_HEIGHT)) return;

            if (x < SCREEN_MIN_X)
            {
                w += x;
                x = 0;
            }

            if (y < SCREEN_MIN_Y)
            {
                h += y;
                y = 0;
            }

            if (w > SCREEN_WIDTH)
            {
                w = SCREEN_WIDTH;
            }

            if (y > SCREEN_HEIGHT)
            {
                h = SCREEN_HEIGHT;
            }

            NotifyNewImage(this, new ImageEventArgs(name, x, y, sourceX, sourceY, w, h, scale, rotation));
        }

        public void Paint()
        {
            NotifyPaint(this, new EventArgs());
        }
        #endregion

        #region Public Attributes

        public object userData;

        public const int SCREEN_WIDTH = 128;

        public const int SCREEN_HEIGHT = 128;

        public const int SCREEN_MAX_X = SCREEN_WIDTH - 1;

        public const int SCREEN_MAX_Y = SCREEN_HEIGHT - 1;

        public const int SCREEN_MIN_X = 0;

        public const int SCREEN_MIN_Y = 0;

        #endregion

        #region Properties

        public Neighbors Neighbors
        {
            get; set;
        }

        public string UniqueId { get; set; }

        public Side Orientation { get; set; }

        public bool ButtonIsPressed { get; set; }

        public bool IsShaking { get; set; }

        #endregion

        #region Events
        public delegate void FlipEventHandler(Cube c, bool newOrientationIsUp);
        public event FlipEventHandler NotifyCubeFlip = delegate { };

        #endregion

        #region Member Change Event Handling

        public delegate void EventHandler(object sender, EventArgs args);
        public event EventHandler NotifyBackgroundColorChanged = delegate { };
        public event EventHandler NotifyNewRectangle = delegate { };
        public event EventHandler NotifyScreenItemsEmptied = delegate { };
        public event EventHandler NotifyNewImage = delegate { };
        public event EventHandler NotifyPaint = delegate { }; 

        public event EventHandler NotifyCubeMoved = delegate { };

        #endregion
    }
}
