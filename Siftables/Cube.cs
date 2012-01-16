using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;

namespace Siftables
{
    class Cube
    {
        private Rectangle _rect = new Rectangle();
        private Canvas _parent;
        private Canvas _screen;

        // Click and drag behavior
        private bool isMouseCaptured;
        private double mouseVerticalPosition;
        private double mouseHorizontalPosition;
        private double canvasTopPosition;
        private double canvasLeftPosition;

        UIElementCollection Children;

        public Cube(Canvas parent, double left, double top)
        {
            _parent = parent;
            _screen = new Canvas();
            Children = _screen.Children;
            _screen.Background = new SolidColorBrush(Colors.Green);
            _screen.Width = 128;
            _screen.Height = 128;
            _parent.Children.Add(_screen);
            _screen.MouseLeftButtonDown += Handle_MouseDownRect;
            _screen.MouseMove += Handle_MouseMoveRect;
            _screen.MouseLeftButtonUp += Handle_MouseUpRect;



            // Initialize rectangle properties
            _rect.Width = 128;
            _rect.Height = 128;
            _rect.Fill = new SolidColorBrush(Colors.White);
            _rect.Stroke = new SolidColorBrush(Colors.Black);
            _rect.MouseLeftButtonDown += Handle_MouseDownRect;
            _rect.MouseMove += Handle_MouseMoveRect;
            _rect.MouseLeftButtonUp += Handle_MouseUpRect;
            _parent.Children.Add(_rect);
            Canvas.SetLeft(_rect, left);
            Canvas.SetTop(_rect, top);
        }

        private void Handle_MouseUpRect(object sender, MouseEventArgs args)
        {
            UIElement item = sender as UIElement;
            isMouseCaptured = false;
            item.ReleaseMouseCapture();
            mouseVerticalPosition = -1;
            mouseHorizontalPosition = -1;
        }

        private void Handle_MouseMoveRect(object sender, MouseEventArgs args)
        {
            UIElement item = sender as UIElement;

            if (isMouseCaptured)
            {
                // Calculate the current position of the object.
                double deltaV;
                if (args.GetPosition(null).Y < canvasTopPosition) { deltaV = 0; }
                else { deltaV = args.GetPosition(null).Y - mouseVerticalPosition; }

                double deltaH;
                if (args.GetPosition(null).X < canvasLeftPosition) { deltaH = 0; }
                else { deltaH = args.GetPosition(null).X - mouseHorizontalPosition; }
                double newTop = deltaV + (double)item.GetValue(Canvas.TopProperty);
                double newLeft = deltaH + (double)item.GetValue(Canvas.LeftProperty);

                // Set new position of object.
                if (newTop < 0) { item.SetValue(Canvas.TopProperty, 0.0); }
                else { item.SetValue(Canvas.TopProperty, newTop); }

                if (newLeft < 0) { item.SetValue(Canvas.LeftProperty, 0.0); }
                else { item.SetValue(Canvas.LeftProperty, newLeft); }

                // Update position global variables.
                mouseVerticalPosition = args.GetPosition(null).Y;
                mouseHorizontalPosition = args.GetPosition(null).X;
            }
        }

        private void Handle_MouseDownRect(object sender, MouseEventArgs args)
        {
            UIElement item = sender as UIElement;
            mouseVerticalPosition = args.GetPosition(null).Y;
            mouseHorizontalPosition = args.GetPosition(null).X;
            canvasLeftPosition = _parent.TransformToVisual(null).Transform(new Point()).X;
            canvasTopPosition = _parent.TransformToVisual(null).Transform(new Point()).Y;
            isMouseCaptured = true;
            item.CaptureMouse();
        }
    }
}
