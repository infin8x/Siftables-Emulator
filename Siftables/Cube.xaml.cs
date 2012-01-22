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
    public partial class Cube : UserControl
    {
        public enum Side { Top, Right, Bottom, Left, None };

        public static int dimension = 128;

        public Cube()
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
            if ((x > this.screen.Width) || (y > this.screen.Height)) return;

            Rectangle r = new Rectangle();
            r.Fill = new SolidColorBrush(c);

            if (x < 0)
            {
                w += x;
                x = 0;
            }

            if (y < 0)
            {
                h += y;
                y = 0;
            }

            r.Width = w;
            r.Height = h;
            Canvas.SetTop(r, x);
            Canvas.SetLeft(r, y);

            this.screen.Children.Add(r);
        }
    }
}
