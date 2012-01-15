using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;

namespace Siftables
{
    class Cube
    {
        private Rectangle _rect = new Rectangle();
        private Canvas _canvas;

        // Click and drag behavior
        private bool isMouseCaptured;
        private double mouseVerticalPosition;
        private double mouseHorizontalPosition;

        List<Shape> Children = new List<Shape>();
        
        public Cube(Canvas canvas, double left, double top)
        {
            _canvas = canvas;
            // Initialize rectangle properties
            _rect.Width = 128;
            _rect.Height = 128;
            _rect.Fill = new SolidColorBrush(Colors.White);
            _rect.Stroke = new SolidColorBrush(Colors.Black);
            _rect.MouseLeftButtonDown += Handle_MouseDownRect;
            _rect.MouseMove += Handle_MouseMoveRect;
            _rect.MouseLeftButtonUp += Handle_MouseUpRect;
            _canvas.Children.Add(_rect);
            Canvas.SetLeft(_rect, left);
            Canvas.SetTop(_rect, top);
        }

        private void Handle_MouseUpRect(object sender, MouseEventArgs args)
        {
            Rectangle item = sender as Rectangle;
            isMouseCaptured = false;
            item.ReleaseMouseCapture();
            mouseVerticalPosition = -1;
            mouseHorizontalPosition = -1;
        }

        private void Handle_MouseMoveRect(object sender, MouseEventArgs args)
        {
            Rectangle item = sender as Rectangle;
            if (isMouseCaptured)
            {

                // Calculate the current position of the object.
                double deltaV = args.GetPosition(null).Y - mouseVerticalPosition;
                double deltaH = args.GetPosition(null).X - mouseHorizontalPosition;
                double newTop = deltaV + (double)item.GetValue(Canvas.TopProperty);
                double newLeft = deltaH + (double)item.GetValue(Canvas.LeftProperty);

                // Set new position of object.
                item.SetValue(Canvas.TopProperty, newTop);
                item.SetValue(Canvas.LeftProperty, newLeft);

                // Update position global variables.
                mouseVerticalPosition = args.GetPosition(null).Y;
                mouseHorizontalPosition = args.GetPosition(null).X;
            }
        }

        private void Handle_MouseDownRect(object sender, MouseEventArgs args)
        {
            Rectangle item = sender as Rectangle;
            mouseVerticalPosition = args.GetPosition(null).Y;
            mouseHorizontalPosition = args.GetPosition(null).X;
            isMouseCaptured = true;
            item.CaptureMouse();
        }
    }
}
