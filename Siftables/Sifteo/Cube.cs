using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace Siftables.Sifteo
{
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

        public enum Side { TOP = 0, LEFT = 1, BOTTOM = 2, RIGHT = 3, NONE = 4 };
        #endregion

        #region Private Implementation Members

        private Color _backgroundColor;

        public Color BackgroundColor
        {
            get
            {
                return this._backgroundColor;
            }
        }

        private Collection<FrameworkElement> _screenItems;

        public Collection<FrameworkElement> ScreenItems
        {
            get
            {
                return this._screenItems;
            }
        }

        #endregion

        #region Member Change Event Handling

        public delegate void EventHandler(object sender, EventArgs args);

        public event EventHandler NotifyBackgroundColorChanged = delegate { };

        public event EventHandler NotifyScreenItemsChanged = delegate { };

        #endregion

        public Cube()
        {
            this._backgroundColor = Colors.White;
            this._screenItems = new ObservableCollection<FrameworkElement>();
        }

        public void FillScreen(Color color)
        {
            ScreenItems.Clear();
            NotifyScreenItemsChanged(this, new EventArgs());
            this._backgroundColor = color;
            NotifyBackgroundColorChanged(this, new EventArgs());
        }

        public void FillRect(Color c, int x, int y, int w, int h)
        {
            if ((x > SCREEN_WIDTH) || (y > SCREEN_HEIGHT)) return;

            Rectangle r = new Rectangle();
            r.Fill = new SolidColorBrush(c);

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

            ScreenItems.Add(r);
            NotifyScreenItemsChanged(this, new EventArgs());
        }
    }
}
