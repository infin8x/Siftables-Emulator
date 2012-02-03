using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;

namespace Siftables.Sifteo
{
    public class CubeSift : INotifyPropertyChanged
    {

        #region Sifteo
        public static int dimension = 128;

        public const int SCREEN_WIDTH = 128;

        public const int SCREEN_HEIGHT = 128;

        public const int SCREEN_MAX_X = SCREEN_WIDTH - 1;

        public const int SCREEN_MAX_Y = SCREEN_HEIGHT - 1;

        public const int SCREEN_MIN_X = 0;

        public const int SCREEN_MIN_Y = 0;

        public enum Side { TOP = 0, LEFT = 1, BOTTOM = 2, RIGHT = 3, NONE = 4 };
        #endregion

        private Brush _backgroundColor;

        public Brush BackgroundColor
        {
            get
            {
                return this._backgroundColor;
            }

            set {
                if (value != this._backgroundColor)
                {
                    this._backgroundColor = value;
                    NotifyPropertyChanged("BackgroundColor");
                }
            }
        }

        public int Width
        {
            get
            {
                return SCREEN_WIDTH;
            }
        }

        public int Height
        {
            get
            {
                return SCREEN_HEIGHT;
            }
        }

        private ObservableCollection<FrameworkElement> _screenItems;

        public ObservableCollection<FrameworkElement> ScreenItems
        {
            get { return _screenItems; }

            set
            {
                if (_screenItems == value) { return; }
                _screenItems = value;
            }

        }

        public CubeSift()
        {
            BackgroundColor = new SolidColorBrush(Colors.White);
            ScreenItems = new ObservableCollection<FrameworkElement>();
        }

        public void FillScreen(Color color)
        {
            ScreenItems.Clear();
            BackgroundColor = new SolidColorBrush(color);
        }

        public void FillRect(Color c, int x, int y, int w, int h)
        {
            if ((x > Width) || (y > Height)) return;

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
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
