using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Shapes;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using Sifteo;

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

        private int _positionX;
        public int PositionX
        {
            get { return _positionX; }
            set
            { 
                _positionX = value;
                NotifyPropertyChanged("PositionX");
            }
        }

        private int _positionY;
        public int PositionY
        {
            get { return _positionY; }
            set 
            { 
                _positionY = value;
                NotifyPropertyChanged("PositionY");
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
        public RelayCommand TiltLeftCommand { get; private set; }
        public RelayCommand TiltRightCommand { get; private set; }
        public RelayCommand TiltUpCommand { get; private set; }
        public RelayCommand TiltDownCommand { get; private set; }
        public RelayCommand ButtonPressCommand { get; private set; }
        public RelayCommand ButtonReleaseCommand { get; private set; }

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

        private ImageSources _imageSources;
        public ImageSources ImageSources
        {
            get { return _imageSources; }
            set
            {
                _imageSources = value;
                CubeModel.NotifyNewImage += (sender, args) => AddPendingImage((ImageEventArgs)args);
            }
        }

        public CubeViewModel()
        {
            #region RelayCommandDefinitions
            #region CubeFlipCommand
            CubeFlipCommand = new RelayCommand(() => CubeModel.OnFlip());
            #endregion
            #region CubeTiltCommands
            TiltLeftCommand = new RelayCommand(() => CubeModel.OnTilt(Cube.Side.LEFT));
            TiltRightCommand = new RelayCommand(() => CubeModel.OnTilt(Cube.Side.RIGHT));
            TiltUpCommand = new RelayCommand(() => CubeModel.OnTilt(Cube.Side.TOP));
            TiltDownCommand = new RelayCommand(() => CubeModel.OnTilt(Cube.Side.BOTTOM));
            #endregion
            #region RotateCWCommand
            RotateCwCommand = new RelayCommand(() => CubeModel.OnRotateCW());
            #endregion
            #region RotateCCWCommand
            RotateCcwCommand = new RelayCommand(() => CubeModel.OnRotateCCW());
            #endregion
            #region ButtonCommands
            ButtonPressCommand = new RelayCommand(() => CubeModel.OnButtonPress());
            ButtonReleaseCommand = new RelayCommand(() => CubeModel.OnButtonRelease());
            #endregion
            #endregion

            CubeModel = new Cube();
            ScreenItems = new ObservableCollection<FrameworkElement>();
            _pendingScreenItems = new Collection<FrameworkElement>();

            CubeModel.NotifyBackgroundColorChanged += (sender, args) => UpdatePendingBackgroundColor((BackgroundEventArgs) args);
            CubeModel.NotifyNewRectangle += (sender, args) => AddPendingRectangle((RectangleEventArgs) args);
            CubeModel.NotifyScreenItemsEmptied += (sender, args) => EmptyScreenItems();
            CubeModel.NotifyPaint += (sender, args) => PaintPendingGraphics();
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
            if (ImageSources.Contains(imageEventArgs.ImageName))
            {
                var imageSource = ImageSources.GetBitmapImage(imageEventArgs.ImageName);
                var width = imageEventArgs.Width;
                var height = imageEventArgs.Height;
                if (imageEventArgs.Rotation % 2 != 0)
                {
                    width = imageEventArgs.Height;
                    height = imageEventArgs.Width;
                }
                var image = new Image
                                {
                                    Source = imageSource,
                                    Width = width,
                                    Height = height
                                };
                Canvas.SetLeft(image, imageEventArgs.X);
                Canvas.SetTop(image, imageEventArgs.Y);

                image.Clip = new RectangleGeometry
                                 {
                                     Rect =
                                         new Rect(imageEventArgs.SourceX, imageEventArgs.SourceY, imageSource.PixelWidth - imageEventArgs.SourceX,
                                                  imageSource.PixelHeight - imageEventArgs.SourceY)
                                 };


                var transformGroup = new TransformGroup();

                var scaleTransform = new ScaleTransform
                {
                    ScaleX = imageEventArgs.Scale,
                    ScaleY = imageEventArgs.Scale,
                    CenterX = 0,
                    CenterY = 0
                };
                transformGroup.Children.Add(scaleTransform);

                var translateTransform = new TranslateTransform
                {
                    X = -1 * imageEventArgs.SourceX,
                    Y = -1 * imageEventArgs.SourceY
                };
                transformGroup.Children.Add(translateTransform);

                var fitTransform = new ScaleTransform
                                         {
                                             ScaleX = ((double) width) / (imageSource.PixelWidth * imageEventArgs.Scale - imageEventArgs.SourceX),
                                             ScaleY = ((double) height) / (imageSource.PixelHeight * imageEventArgs.Scale - imageEventArgs.SourceY),
                                             CenterX = 0,
                                             CenterY = 0
                                         };
                transformGroup.Children.Add(fitTransform);

                var rotateTransform = new RotateTransform
                {
                    Angle = imageEventArgs.Rotation * 90,
                    CenterX = width / 2,
                    CenterY = height / 2
                };
                transformGroup.Children.Add(rotateTransform);

                image.RenderTransform = transformGroup;

                _pendingScreenItems.Add(image);
            }
        }
    }

}