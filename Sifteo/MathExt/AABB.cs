using System;
using System.Windows;
using System.Windows.Shapes;

namespace Sifteo.MathExt
{
    public class AABB
    {
        public Int2 position;
        public Int2 size;

        public Int2 TopLeft
        {
            get { return position; }
            set
            {
                Left = value.x;
                Top = value.y;
            }
        }

        public Int2 TopRight
        {
            get { return new Int2(Right, Top);}
            set
            {
                Top = value.y;
                Right = value.x;
            }
        }

        public Int2 BottomLeft
        {
            get { return new Int2(Left, Bottom); }
            set
            {
                Bottom = value.y;
                Left = value.x;
            }
        }

        public Int2 BottomRight
        {
            get { return new Int2(Right, Bottom); }
            set
            {
                Bottom = value.y;
                Right = value.x;
            }
        }

        public int Left
        {
            get { return position.x; }
            set
            {
                size.x = Right - value;
                position.x = value;
            }
        }

        public int Right
        {
            get { return position.x + size.x; }
            set { size.x = value - position.x; }
        }

        public int Top
        {
            get { return position.y; }
            set
            {
                size.y = Bottom - value;
                position.y = value;
            }
        }

        public int Bottom
        {
            get { return position.y + size.y; }
            set { size.y = value - position.y; }
        }

        public AABB(int x, int y, int sx, int sy) : this(new Int2(x, y), new Int2(sx, sy))
        {
        }

        public AABB(Int2 position, Int2 size)
        {
            this.position = position;
            this.size = size;
        }

        public bool Intersection(AABB aabb, out AABB result)
        {
            if (Overlaps(aabb))
            {
                var thisRect = new Rect(Left, Top, size.x, size.y);
                var otherRect = new Rect(aabb.Left, aabb.Top, aabb.size.x, aabb.size.y);
                thisRect.Intersect(otherRect);
                result = new AABB((int) thisRect.X, (int) thisRect.Y, (int) thisRect.Width, (int) thisRect.Height);
                return true;
            } else
            {
                result = null;
                return false;
            }
        }

        public bool Overlaps(AABB aabb)
        {
            var thisRect = new Rect(Left, Top, size.x, size.y);
            var otherRect = new Rect(aabb.Left, aabb.Top, aabb.size.x, aabb.size.y);
            thisRect.Intersect(otherRect);
            return (thisRect != Rect.Empty);
        }
    }
}
