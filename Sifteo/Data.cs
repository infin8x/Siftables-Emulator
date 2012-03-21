namespace SiftDomain
{
    public class Data
    {
        #region Properties

        public bool IsValid { get; private set; }

        #endregion

        #region Public Member Functions

        #endregion

        #region Public Member Functions

        public virtual void Setup()
        {
            // Should be overridden by application
        }

        public virtual void Tick()
        {
            // Should be overridden by application
        }

        #endregion
    }
}