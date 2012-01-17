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
using System.Windows.Interactivity;

namespace Siftables
{
    public class DragAndDropBehavior : Behavior<UIElement>
    {
        // This code comes largely from: http://shirl9141.wordpress.com/2010/01/16/silverlight-drag-n-drop-behaviors-and-custom-controls/
        private Point mouseClickPosition;
        private bool _isDragging;

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseLeftButtonDown += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.MouseLeftButtonUp += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonUp);
            this.AssociatedObject.MouseMove += new MouseEventHandler(AssociatedObject_MouseMove);
        }
        protected override void OnDetaching()
        {
            base.OnAttached();
            this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
        }

        void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                this.AssociatedObject.ReleaseMouseCapture();
                this._isDragging = false;
            }
        }

        void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this._isDragging = true;
            this.mouseClickPosition = e.GetPosition(this.AssociatedObject);
            this.AssociatedObject.CaptureMouse();
        }


        void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                Point point = e.GetPosition(null);
                AssociatedObject.SetValue(Canvas.TopProperty, point.Y - this.mouseClickPosition.Y);
                AssociatedObject.SetValue(Canvas.LeftProperty, point.X - this.mouseClickPosition.X);
            }
        }
    }
}
