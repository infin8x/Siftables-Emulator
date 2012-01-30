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
using System.Windows.Media.Imaging;
using Siftables.Behaviors;

namespace Siftables
{
    public partial class Cube : UserControl
    {


        public static int dimension = 128;

        public const int SCREEN_WIDTH = 128;

        public const int SCREEN_HEIGHT = 128;

        public const int SCREEN_MAX_X = SCREEN_WIDTH - 1;

        public const int SCREEN_MAX_Y = SCREEN_HEIGHT - 1;

        public const int SCREEN_MIN_X = 0;

        public const int SCREEN_MIN_Y = 0;

        public Cube()
        {
            InitializeComponent();

            Interaction.GetBehaviors(this).Add(new DragAndDropBehavior());
        }

        public void FillScreen(Color c)
        {
            //this.screen.Children.Clear();
            //this.screen.Background = new SolidColorBrush(c);
        }

        public void FillRect(Color c, int x, int y, int w, int h)
        {
            //if ((x > this.screen.Width) || (y > this.screen.Height)) return;

            Rectangle r = new Rectangle();
            r.Fill = new SolidColorBrush(c);

            if (x < SCREEN_MIN_X)
            {
                w += x;
                x = 0;
            }

            if (y < SCREEN_MIN_Y)
            {
                h += y;
                y = 0;
            }

            if (w > SCREEN_WIDTH)
            {
                w = SCREEN_WIDTH;
            }

            if (y > SCREEN_HEIGHT)
            {
                h = SCREEN_HEIGHT;
            }

            r.Width = w;
            r.Height = h;
            Canvas.SetTop(r, y);
            Canvas.SetLeft(r, x);

            //this.screen.Children.Add(r);
        }

        // scale and rotation not taken into account yet
        public void Image(String name, int x = 0, int y = 0, int sourceX = 0, int sourceY = 0, int w = SCREEN_WIDTH, int h = SCREEN_HEIGHT, int scale = 1, int rotation = 0)
        {
            Image newImg = new Image();
            newImg.Source = new BitmapImage(new Uri(@"/Siftables;component/Images/" + name, UriKind.RelativeOrAbsolute));
            newImg.Width = w;
            newImg.Height = h;
            Canvas.SetLeft(newImg, x);
            Canvas.SetTop(newImg, y);
            RectangleGeometry clip = new RectangleGeometry();
            clip.Rect = new Rect(sourceX, sourceY, w, h);
            newImg.Clip = clip;

            //this.screen.Children.Add(newImg);
        }

        
    }
}
