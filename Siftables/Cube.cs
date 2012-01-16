using System;
using System.Collections;
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
        private Canvas _canvas;

        private List<Shape> _shapes = new List<Shape>();

        // Click and drag behavior
        private bool isMouseCaptured;
        private double mouseVerticalPosition;
        private double mouseHorizontalPosition;
        private double canvasTopPosition;
        private double canvasLeftPosition;


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

        public void addShape(Shape s)
        {
            this._shapes.Add(s);
        }

        public void addRectangle(int x1, int y1, int x2, int y2, Color c)
        {
            Rectangle r = new Rectangle();
            r.Width = Math.Abs(x1 - x2);
            r.Height = Math.Abs(y1 - y2);
            r.Fill = new SolidColorBrush(c);
            r.Stroke = new SolidColorBrush(Colors.Black);
            this.addShape(r);
        }

        /**
         * Set the background fill of this cube.  Works by creating a big rectangle.
         */
        public void setFill(Color c)
        {
            // todo
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
            Rectangle item = sender as Rectangle;
            mouseVerticalPosition = args.GetPosition(null).Y;
            mouseHorizontalPosition = args.GetPosition(null).X;
            canvasLeftPosition = _canvas.TransformToVisual(null).Transform(new Point()).X;
            canvasTopPosition = _canvas.TransformToVisual(null).Transform(new Point()).Y;
            isMouseCaptured = true;
            item.CaptureMouse();
        }
    }
}
