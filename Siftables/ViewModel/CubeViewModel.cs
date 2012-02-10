using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Siftables.Sifteo;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace Siftables.ViewModel
{
    public class CubeViewModel : INotifyPropertyChanged
    {

        #region BindingDefinitions

        private Brush _backgroundColor;

        public Brush BackgroundColor
        {
            get
            {
                return this._backgroundColor;
            }

            set
            {
                if (value != this._backgroundColor)
                {
                    this._backgroundColor = value;
                    NotifyPropertyChanged("BackgroundColor");
                }
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

        private Cube _cube;

        public Cube CubeModel
        {
            get
            {
                return this._cube;
            }
        }

        public CubeViewModel()
        {
            this._cube = new Cube();
            ScreenItems = new ObservableCollection<FrameworkElement>();
            this._cube.NotifyBackgroundColorChanged += (sender, args) => { UpdateBackgroundColor((BackgroundEventArgs) args); };
            this._cube.NotifyScreenItemsChanged += (sender, args) => { UpdateScreenItems((RectangleEventArgs) args); };
            this._cube.NotifyScreenItemsEmptied += (sender, args) => { EmptyScreenItems(); };
        }

        public void UpdateBackgroundColor(BackgroundEventArgs bArgs)
        {
            BackgroundColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, bArgs.BackgroundColor.R, bArgs.BackgroundColor.G, bArgs.BackgroundColor.B));
        }

        public void UpdateScreenItems(RectangleEventArgs sArgs)
        {
            Rectangle r = new Rectangle();
            r.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, sArgs.C.R, sArgs.C.G, sArgs.C.B));
            r.Width = sArgs.W;
            r.Height = sArgs.H;
            Canvas.SetTop(r, sArgs.Y);
            Canvas.SetLeft(r, sArgs.X);
            ScreenItems.Add(r);
        }

        public void EmptyScreenItems()
        {
            ScreenItems.Clear();
        }
    }

}