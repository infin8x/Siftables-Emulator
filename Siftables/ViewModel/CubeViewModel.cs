using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using Siftables.Sifteo;
using System.Windows.Shapes;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;

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
                return _backgroundColor;
            }

            set
            {
                if (value != _backgroundColor)
                {
                    _backgroundColor = value;
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

        public RelayCommand RotateCcwCommand { get; private set; }
        public RelayCommand RotateCwCommand { get; private set; }
        public RelayCommand CubeFlipCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

        public Cube CubeModel { get; private set; }

        private readonly Collection<FrameworkElement> _pendingScreenItems;
        private SolidColorBrush _pendingBackgroundColor;

        private ImageManager _imageManager;

        public CubeViewModel()
        {
            #region RelayCommandDefinitions
            #region CubeFlipCommand
            CubeFlipCommand = new RelayCommand(() => CubeModel.OnFlip());
            #endregion
            #region RotateCWCommand
            RotateCwCommand = new RelayCommand(() =>
            {
                
            });
            #endregion
            #region RotateCCWCommand
            RotateCcwCommand = new RelayCommand(() =>
            {
                
            });
            #endregion
            #endregion

            CubeModel = new Cube();
            ScreenItems = new ObservableCollection<FrameworkElement>();
            _pendingScreenItems = new Collection<FrameworkElement>();

            CubeModel.NotifyBackgroundColorChanged += (sender, args) => UpdatePendingBackgroundColor((BackgroundEventArgs) args);
            CubeModel.NotifyNewRectangle += (sender, args) => AddPendingRectangle((RectangleEventArgs) args);
            CubeModel.NotifyScreenItemsEmptied += (sender, args) => EmptyScreenItems();
            CubeModel.NotifyNewImage += (sender, args) => AddPendingImage((ImageEventArgs) args);
            CubeModel.NotifyPaint += (sender, args) => PaintPendingGraphics();
        }

        public void AssociateImageManager(ImageManager imageManager)
        {
            _imageManager = imageManager;
        }

        public void PaintPendingGraphics()
        {
            if (_pendingBackgroundColor != null)
            {
                ScreenItems.Clear();
                BackgroundColor = _pendingBackgroundColor;
                _pendingBackgroundColor = null;
            }

            if (_pendingScreenItems.Count > 0)
            {
                foreach (var screenItem in _pendingScreenItems)
                {
                    ScreenItems.Add(screenItem);
                }
                _pendingScreenItems.Clear();
            }
        }

        public void UpdatePendingBackgroundColor(BackgroundEventArgs backgroundEventArgs)
        {
            _pendingScreenItems.Clear();
            _pendingBackgroundColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, backgroundEventArgs.BackgroundColor.R, backgroundEventArgs.BackgroundColor.G, backgroundEventArgs.BackgroundColor.B));
        }

        public void AddPendingRectangle(RectangleEventArgs rectangleEventArgs)
        {
            var r = new Rectangle();
            r.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, rectangleEventArgs.Color.R, rectangleEventArgs.Color.G, rectangleEventArgs.Color.B));
            r.Width = rectangleEventArgs.Width;
            r.Height = rectangleEventArgs.Height;
            Canvas.SetTop(r, rectangleEventArgs.Y);
            Canvas.SetLeft(r, rectangleEventArgs.X);
            _pendingScreenItems.Add(r);
        }

        public void EmptyScreenItems()
        {
            ScreenItems.Clear();
        }

        public void AddPendingImage(ImageEventArgs imageEventArgs)
        {
            var image = new Image
                            {
                                Source = _imageManager.GetBitmapImage(imageEventArgs.ImageName),
                                Width = imageEventArgs.Width,
                                Height = imageEventArgs.Height
                            };
            Canvas.SetLeft(image, imageEventArgs.X);
            Canvas.SetTop(image, imageEventArgs.Y);
            var clip = new RectangleGeometry {Rect = new Rect(imageEventArgs.SourceX, imageEventArgs.SourceY, imageEventArgs.Width, imageEventArgs.Height)};
            image.Clip = clip;

            _pendingScreenItems.Add(image);
        }
    }

}