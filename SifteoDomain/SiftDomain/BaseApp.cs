namespace Siftables.Sifteo
{
    public class BaseApp
    {

        private CubeSet _cubes;

        public CubeSet Cubes
        {
            get { return this._cubes; }
        }

        private int _frameRate;

        public int FrameRate
        {
            get
            {
                return this._frameRate;
            }
        }

        public BaseApp()
        {
            this._frameRate = 20;
        }

        public void AssociateCubes(CubeSet cubes)
        {
            this._cubes = cubes;
            System.Diagnostics.Debug.WriteLine("Here");
        }

        virtual public void Setup()
        {
            // Should be overridden by application
        }

        virtual public void Tick()
        {
            // Should be overridden by application
        }
    }
}
