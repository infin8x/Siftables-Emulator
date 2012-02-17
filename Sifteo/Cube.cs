using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Siftables.Sifteo
{

    public class BackgroundEventArgs : EventArgs
    {
        private Color _backgroundColor;

        public Color BackgroundColor
        {
            get
            {
                return this._backgroundColor;
            }
        }

        public BackgroundEventArgs(Color backgroundColor)
        {
            this._backgroundColor = backgroundColor;
        }
    }

    public class RectangleEventArgs : EventArgs
    {
        private int _x;
        private int _y;
        private int _w;
        private int _h;
        private Color _c;

        public int X { get { return this._x; } }
        public int Y { get { return this._y; } }
        public int W { get { return this._w; } }
        public int H { get { return this._h; } }
        public Color C { get { return this._c; } }

        public RectangleEventArgs(Color c, int x, int y, int w, int h)
        {
            this._c = c;
            this._x = x;
            this._y = y;
            this._w = w;
            this._h = h;
        }
    }

    public class Cube
    {

        #region Public Sifteo Members

        public static int dimension = 128;

        public const int SCREEN_WIDTH = 128;

        public const int SCREEN_HEIGHT = 128;

        public const int SCREEN_MAX_X = SCREEN_WIDTH - 1;

        public const int SCREEN_MAX_Y = SCREEN_HEIGHT - 1;

        public const int SCREEN_MIN_X = 0;

        public const int SCREEN_MIN_Y = 0;

        public enum Side { TOP = 0, LEFT = 1, BOTTOM = 2, RIGHT = 3, NONE = 4 }

        #endregion

        private Neighbors _neighbors;
        public Neighbors Neighbors
        {
            get
            {
                if (this._neighbors == null) this._neighbors = new Neighbors();
                return this._neighbors;
            }
            set
            {
                this._neighbors = value;
            }
        }

        #region Member Change Event Handling

        public delegate void EventHandler(object sender, EventArgs args);
        public event EventHandler NotifyBackgroundColorChanged = delegate { };
        public event EventHandler NotifyScreenItemsChanged = delegate { };
        public event EventHandler NotifyScreenItemsEmptied = delegate { };
        public event EventHandler NotifyCubeMoved = delegate { };
        public delegate void FlipEventHandler(Cube c, bool newOrientationIsUp);
        public event FlipEventHandler NotifyCubeFlip = delegate { };
        
        #endregion
        
        public void FillScreen(Color color)
        {
            NotifyScreenItemsEmptied(this, new EventArgs());
            NotifyBackgroundColorChanged(this, new BackgroundEventArgs(color));
        }

        public void FillRect(Color c, int x, int y, int w, int h)
        {
            if ((x > SCREEN_WIDTH) || (y > SCREEN_HEIGHT)) return;

            Rectangle r = new Rectangle();
            r.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, c.R, c.G, c.B));

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

            r.Width = w;
            r.Height = h;
            Canvas.SetTop(r, y);
            Canvas.SetLeft(r, x);

            NotifyScreenItemsChanged(this, new RectangleEventArgs(c, x, y, w, h));
        }

        // scale and rotation not taken into account yet
        public void Image(String name, int x = 0, int y = 0, int sourceX = 0, int sourceY = 0, int w = SCREEN_WIDTH, int h = SCREEN_HEIGHT, int scale = 1, int rotation = 0)
        {
            /* This needs to be abstracted like rectangle and background are at this level, and propagated via events to the viewmodel
            Image newImg = new Image();
            newImg.Source = new BitmapImage(new Uri(@"/Siftables;component/Images/" + name, UriKind.RelativeOrAbsolute));
            newImg.Width = w;
            newImg.Height = h;
            Canvas.SetLeft(newImg, x);
            Canvas.SetTop(newImg, y);
            RectangleGeometry clip = new RectangleGeometry();
            clip.Rect = new Rect(sourceX, sourceY, w, h);
            newImg.Clip = clip;
            */
        }

        public void OnMove()
        {
            NotifyCubeMoved(this, new EventArgs());
        }

        public void OnFlip()
        {
            NotifyCubeFlip(this, true);
        }

        

    }
}
