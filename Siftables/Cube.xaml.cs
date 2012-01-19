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
using System.Windows.Interactivity;
using Siftables;

namespace Siftables
{
    public partial class CubeSL : UserControl
    {
        public CubeSL()
        {
            InitializeComponent();

            Interaction.GetBehaviors(this).Add(new DragAndDropBehavior());
        }

        public void FillScreen(Color c)
        {
            this.screen.Children.Clear();
            this.screen.Background = new SolidColorBrush(c);
        }

        public void FillRect(Color c, int x, int y, int w, int h)
        {
            Rectangle r = new Rectangle();

            r.Width = w;
            r.Height = h;
            r.Fill = new SolidColorBrush(c);

            Canvas.SetTop(r, x);
            Canvas.SetLeft(r, y);

            this.screen.Children.Add(r);
        }
    }
}
