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
    public partial class CubeView : UserControl
    {

        public CubeView()
        {
            InitializeComponent();

            Interaction.GetBehaviors(this).Add(new DragAndDropBehavior());
            Interaction.GetBehaviors(this).Add(new NeighborRefreshBehavior());
        }
        /*
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
        */
        
    }
}
