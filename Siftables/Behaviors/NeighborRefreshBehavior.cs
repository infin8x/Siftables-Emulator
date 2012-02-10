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

using Siftables;
using Siftables.ViewModel;

namespace Siftables.Behaviors
{
    // Much of this is duplicated logic from DragAndDropBehavior.cs
    // The main reasons why I decided to make this its own behavior instead of modifying DnD is that
    // because with my implementation, this behavior must be parameterized to CubeView instead of a
    // more abstract UIElement.
    public class NeighborRefreshBehavior : Behavior<CubeView>
    {
        private bool _isDragging;
        protected override void OnAttached()
        {
            base.OnAttached();
            this._isDragging = false;
            this.AssociatedObject.MouseMove += new MouseEventHandler(AssociatedObject_MouseMove);
            this.AssociatedObject.MouseLeftButtonDown += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.MouseLeftButtonUp += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonUp);
        }
        protected override void OnDetaching()
        {
            base.OnAttached();
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
        }

        void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (this._isDragging)
              ((CubeViewModel)this.AssociatedObject.LayoutRoot.DataContext).CubeModel.OnMove();
        }


        void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this._isDragging = true;
        }

        void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this._isDragging)
            {
                this._isDragging = false;
            }
        }
    }
}
