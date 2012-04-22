using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using Siftables.ViewModel;

namespace Siftables.Behaviors
{
    // Drag-and-drop code comes largely from: http://shirl9141.wordpress.com/2010/01/16/silverlight-drag-n-drop-behaviors-and-custom-controls/
    // Shake detection code comes largely from: Visual C# Kicks - http://www.vcskicks.com/  | License - http://www.vcskicks.com/license.html 
    public class DragAndDropBehavior : Behavior<UIElement>
    {
        private static readonly List<Point> ShakePoints = new List<Point>();
        private bool _isDragging;
        private Point _mouseClickPosition;
        private DateTime _captureEnd;
        private DateTime _captureStart;
        private CubeViewModel vm;
        private bool _isShaking;

        public DragAndDropBehavior(object p)
        {
            vm = p as CubeViewModel;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += AssociatedObjectMouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp += AssociatedObjectMouseLeftButtonUp;
            AssociatedObject.MouseMove += AssociatedObjectMouseMove;
        }

        protected override void OnDetaching()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown -= AssociatedObjectMouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp -= AssociatedObjectMouseLeftButtonUp;
            AssociatedObject.MouseMove -= AssociatedObjectMouseMove;
        }

        private void AssociatedObjectMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            _mouseClickPosition = e.GetPosition(AssociatedObject);
            AssociatedObject.CaptureMouse();
            _captureStart = DateTime.Now;
        }

        private void AssociatedObjectMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                Point point = e.GetPosition(null);

                ShakePoints.Add(point);

                AssociatedObject.SetValue(Canvas.TopProperty, point.Y - _mouseClickPosition.Y);
                AssociatedObject.SetValue(Canvas.LeftProperty, point.X - _mouseClickPosition.X);
                if (!_isShaking && CheckShake(e))
                {
                    vm.ShakeStartCommand.Execute(null);
                    _isShaking = true;
                    Debug.WriteLine("shaking");
                }
            }
        }

        private void AssociatedObjectMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                AssociatedObject.ReleaseMouseCapture();
                _isDragging = false;

                ShakePoints.Clear();
                _captureEnd = DateTime.Now;
            }
            if (_isShaking)
            {
                vm.ShakeStopCommand.Execute(_captureEnd.Subtract(_captureStart).Milliseconds);
                _isShaking = false;
            }
        }

        private bool CheckShake(MouseEventArgs e)
        {
            Point point = e.GetPosition(null);
            if (ShakePoints != null)
            {
                Point avg = GetAveragePoint(ShakePoints);

                //Calculate difference of average point to current position
                var deltaPoint = new Point { X = point.X - avg.X, Y = point.Y - avg.Y };

                //Calculate the number of milliseconds that spanned while the window moved
                //Note: Only uses seconds and milliseconds
                TimeSpan movementTime = _captureEnd.Subtract(_captureStart);
                int msSpan = (movementTime.Seconds * 1000 + movementTime.Milliseconds);

                //If values fall within a certain range, then the window was shaken
                return msSpan <= 1000 &&  //speed of the shake in milliseconds
                       ShakePoints.Count >= 80 && //amount of movements in the shake
                       Math.Abs(deltaPoint.X) <= 80 && Math.Abs(deltaPoint.Y) <= 10; //average "size" of shake
            }

            return false;
        }

        private Point GetAveragePoint(List<Point> points)
        {
            var avg = new Point();
            foreach (Point p in points)
            {
                avg.X += p.X;
                avg.Y += p.Y;
            }

            avg.X /= points.Count;
            avg.Y /= points.Count;

            return avg;
        }
    }
}