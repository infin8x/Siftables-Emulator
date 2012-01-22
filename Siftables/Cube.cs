using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;

using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;

namespace Siftables
{
    public class Cube
    {
        public enum Side { Top, Right, Bottom, Left, None };

        public static int dimension = 128;

        private List<Shape> _shapes = new List<Shape>();

        public Cube(Canvas parent, double left, double top)
        {

        }

        public void addShape(Shape s)
        {
            this._shapes.Add(s);
        }

        public void addRectangle(int x1, int y1, int x2, int y2, Color c)
        {
            // @TODO positioning once paint gets worked out
            Rectangle r = new Rectangle();
            r.Width = Math.Abs(x1 - x2);
            r.Height = Math.Abs(y1 - y2);
            r.Fill = new SolidColorBrush(c);
            r.Stroke = new SolidColorBrush(Colors.Black);
            this.addShape(r);
        }

        /**
         * Set the background fill of this cube.  Works by creating a big rectangle.
         */
        public void setFill(Color c)
        {
            // Honestly I have no idea how to do this in the context of silverlight
        }
    }
}
