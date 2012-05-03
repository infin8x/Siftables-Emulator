using System.Windows;
using System.Windows.Input;

namespace Siftables
{
    public partial class MainWindowView
    {
        private double _workspaceScale = 1.0;
        private const int MaxScale = 2;
        private const int MinScale = 1;

        public MainWindowView()
        {
            InitializeComponent();
        }

        private void WorkspaceMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point mousePoint = e.GetPosition(LayoutRoot);
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

            aniScaleX.To = _workspaceScale;
            aniScaleY.To = _workspaceScale;
            aniCenterX.To = centerX;
            aniCenterY.To = centerY;
            sbScale.Begin();
        }
    }
}
