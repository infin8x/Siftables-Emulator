using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Siftables
{
    // Code for Pan and Zoom is based on: http://elegantcode.com/2010/01/16/odd-behavior-with-scaletransform-centerxy-in-silverlight-3/
    public partial class MainWindowView
    {
        private double _workspaceScale = 1.0;
        private const int MaxScale = 2;
        private const int MinScale = 1;
        private bool _mouseCaptured = false;
        private Point startOffset;
        private Point ScreenStartPoint;

        public MainWindowView()
        {
            InitializeComponent();
        }

        private void WorkspaceMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var mousePoint = e.GetPosition(LayoutRoot);
            ScaleWorkspace(e.Delta > 0 ? 0.1 : -0.1, mousePoint.X, mousePoint.Y);
        }

        private void ScaleWorkspace(double changeBy, double centerX, double centerY)
        {
            _workspaceScale += changeBy;
            if (_workspaceScale < MinScale)
                _workspaceScale = MinScale;
            else if (_workspaceScale > MaxScale)
                _workspaceScale = MaxScale;

            zoomTransform.CenterX = centerX;
            zoomTransform.CenterY = centerY;
            zoomTransformScaleX.To = _workspaceScale;
            zoomTransformScaleY.To = _workspaceScale;
            zoomTransformCenterX.To = centerX;
            zoomTransformCenterY.To = centerY;
            sbScale.Begin();
        }

        private void WorkspaceMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (zoomTransformScaleX.To.Equals(0.0))
                return;

            ScreenStartPoint = e.GetPosition(this);
            startOffset = new Point(panTransform.X, panTransform.Y);
            workspace.CaptureMouse();
            _mouseCaptured = true;
            Cursor = Cursors.Hand;
        }

        private void WorkspaceMouseMove(object sender, MouseEventArgs e)
        {
            if (!_mouseCaptured) return;

            // if the mouse is captured then move the content by changing the translate transform.  
            // use the Pan Animation to animate to the new location based on the delta between the 
            // starting point of the mouse and the current point.
            Point physicalPoint = e.GetPosition(this);

            // where you'd like to move the top left corner of the view to
            double toX = physicalPoint.X - ScreenStartPoint.X + startOffset.X;
            double toY = physicalPoint.Y - ScreenStartPoint.Y + startOffset.Y;
            System.Diagnostics.Debug.WriteLine("You're attempting to move to " + toX + "," + toY);

            double scaleValue = zoomTransformScaleX.To.GetValueOrDefault();
            var content = (FrameworkElement)workspace;

            // minimum values we can shift the origin to - 
            // maximum values is always 0 (we don't want the left side of the content
            // ever being beyond the left part of the view
            double minToX = content.ActualWidth - (content.ActualWidth * scaleValue);
            double minToY = content.ActualHeight - (content.ActualHeight * scaleValue);
            double maxToX = zoomTransform.CenterX ;
            double maxToY = zoomTransform.CenterY ;
            // correct any invalid amounts:
            if (toX > maxToX)
                toX = maxToX;
            else if (toX < minToX)
                toX = minToX;
            if (toY > maxToY)
                toY = maxToY;
            else if (toY < minToY)
                toY = minToY;

            System.Diagnostics.Debug.WriteLine("You're actually moving to " + toX + "," + toY);
            panTransformX.To = toX;
            panTransformY.To = toY;
            sbScale.Begin();
        }

        private void WorkspaceMouseLeave(object sender, MouseEventArgs e)
        {
            if (_mouseCaptured)
            {
                Cursor = Cursors.Arrow;
                workspace.ReleaseMouseCapture();
                _mouseCaptured = false;
            }
        }
        private void WorkspaceMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_mouseCaptured)
            {
                Cursor = Cursors.Arrow;
                workspace.ReleaseMouseCapture();
                _mouseCaptured = false;
            }
        }

    }
}
