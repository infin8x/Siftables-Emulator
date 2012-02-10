using System;
using System.Windows;
using System.Windows.Media;
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
            ScreenItems = new ObservableCollection<FrameworkElement>();
            this._cube.NotifyBackgroundColorChanged += (sender, args) => { UpdateBackgroundColor((BackgroundEventArgs) args); };
            this._cube.NotifyScreenItemsChanged += (sender, args) => { UpdateScreenItems((ScreenItemsEventArgs) args); };
            this._cube.NotifyScreenItemsEmptied += (sender, args) => { EmptyScreenItems(); };
        }

        public void UpdateBackgroundColor(BackgroundEventArgs bArgs)
        {
            BackgroundColor = new SolidColorBrush(bArgs.BackgroundColor);
        }

        public void UpdateScreenItems(ScreenItemsEventArgs sArgs)
        {
            ScreenItems.Add(sArgs.ScreenItem);
        }

        public void EmptyScreenItems()
        {
            ScreenItems.Clear();
        }
    }

}