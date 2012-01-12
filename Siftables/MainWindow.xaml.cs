using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;

namespace Siftables
{
    public partial class MainWindow : UserControl
    {
        bool isMouseCaptured;
        double mouseVerticalPosition;
        double mouseHorizontalPosition;

        public MainWindow()
        {
            InitializeComponent();

            // When the window is resized, set new proportions with ContentResized
            App.Current.Host.Content.Resized += new EventHandler(ContentResized);

            Rectangle cube = new Rectangle();
            cube.Width = 50;
            cube.Height = 50;
            cube.Fill = new SolidColorBrush(Colors.Red);
            cube.Visibility = Visibility.Visible;
            cube.MouseLeftButtonDown += Handle_MouseDownRect;
            cube.MouseMove += Handle_MouseMoveRect;
            cube.MouseLeftButtonUp += Handle_MouseUpRect;
            workspace.Children.Add(cube);
            Canvas.SetLeft(cube, 50);
            Canvas.SetTop(cube, 80);
            OpenFileDialog d = new OpenFileDialog();
            
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

        void ContentResized(object sender, EventArgs e) {
            //workspaceBorder.Width = App.Current.Host.Content.ActualWidth * .95;
            //workspaceBorder.Height = App.Current.Host.Content.ActualHeight * .9;
        }
    }
}
