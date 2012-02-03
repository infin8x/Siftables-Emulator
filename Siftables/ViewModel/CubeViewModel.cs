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
using System.Collections.ObjectModel;
using System.ComponentModel;
using Siftables.Sifteo;

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
            this._cube.NotifyBackgroundColorChanged += (sender, args) => { UpdateBackgroundColor(); };
            this._cube.NotifyScreenItemsChanged += (sender, args) => { UpdateScreenItems(); };
            this._cube.FillScreen(Colors.White);
        }

        public void UpdateBackgroundColor()
        {
            BackgroundColor = new SolidColorBrush(this._cube.BackgroundColor);
        }

        public void UpdateScreenItems()
        {
            ScreenItems = (ObservableCollection<FrameworkElement>) this._cube.ScreenItems;
        }
    }

}
