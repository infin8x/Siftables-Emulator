using Sifteo.MathExt;

namespace SiftDomain.Util
{
    public class SpriteData
    {
        public string imageName { get; set; }
        public Int2 source { get; set; }
        public Int2 size { get; set; }
        public Int2 pivot { get; set; }

        public SpriteData(string imageName, int sourceX, int sourceY, int width, int height, int pivotX=0, int pivotY=0)
        {
            this.imageName = imageName;
            source = new Int2(sourceX, sourceY);
            size = new Int2(width, height);
            pivot = new Int2(pivotX, pivotY);
        }
    }
}
