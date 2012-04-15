using Sifteo.MathExt;

namespace Sifteo.Util
{
    public enum PivotMode
    {
        Center, TopLeft
    }
    public class Sprite
    {
        private static readonly AABB _canvasAabb = new AABB(0, 0, 128, 128);

        public PivotMode pivotMode = PivotMode.Center;
        public SpriteData spriteData;
        public bool visible = true;
        public Int2 position = Int2.Zero;
        public int scale = 1;
        public int rotation = 0;

        public bool IsVisible
        {
            get { return spriteData != null && visible; }
        }
        public AABB DirtyRect { get; private set; }
        public AABB AABB {
            get
            {
                AABB thisAabb;
                if (spriteData == null)
                {
                    thisAabb = new AABB(position, Int2.Zero);
                } else
                {
                    thisAabb = new AABB(position, spriteData.size);
                }
                AABB result;
                thisAabb.Intersection(_canvasAabb, out result);
                return result;
            }
        }
        public Cube.Side Orientation
        {
            get { return (Cube.Side)rotation; }
            set { rotation = (int)value; }
        }
        public Int2 LeftTop
        {
            get { return AABB.TopLeft; }
        }
        public Int2 size
        {
            get { return AABB.size; }
        }

        public Sprite(SpriteData data=null)
        {
            spriteData = data;
        }

        public void Paint(Cube c)
        {
            if (IsVisible)
            {
                c.Image(spriteData.imageName, position.x, position.y, spriteData.source.x, spriteData.source.y, spriteData.size.x, spriteData.size.y, scale, rotation);
                DirtyRect = AABB;
            }
        }

        public void PaintMasked(Cube c, AABB worldBounds)
        {
            AABB intersectionAabb;
            if (AABB.Intersection(worldBounds, out intersectionAabb))
            {
                c.Image(spriteData.imageName, intersectionAabb.position.x, intersectionAabb.position.y, spriteData.source.x, spriteData.source.y, intersectionAabb.size.x, intersectionAabb.size.y, scale, rotation);
                DirtyRect = intersectionAabb;
            }
        }
    }
}
