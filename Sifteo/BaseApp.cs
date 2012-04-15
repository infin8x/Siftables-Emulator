namespace Sifteo
{
    public class BaseApp
    {
        #region Properties

        public ImageSet Images { get; set; }

        //public SoundSet Sounds { get; set; }

        public string ResourcePath { get; private set; }

        public string AppID { get; set; }

        public bool IsIdle { get; set; }

        public virtual int FrameRate { get; private set; }

        public float TargetDeltaTime { get; private set; }

        public float DeltaTime { get; private set; }

        public float Time { get; private set; }

        public CubeSet CubeSet { get; private set; }

        public Data StoredData { get; set; }

        #endregion

        public BaseApp()
        {
            FrameRate = 20;
        }

        public void AssociateCubes(CubeSet cubes)
        {
            CubeSet = cubes;
        }

        #region Public Member Functions
        virtual public void Setup()
        {
            // Should be overridden by application
        }

        virtual public void Tick()
        {
            // Should be overridden by application
        }
        #endregion
    }
}
