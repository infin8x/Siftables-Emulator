namespace Siftables.Sifteo
{
    public class Data
    {
        #region Properties

        public bool IsValid { get; private set; }

        private string _data;

        #endregion

        #region Public Member Functions

        #endregion

        #region Public Member Functions

        public string Load()
        {
            return _data;
        }

        public void Store(string data)
        {
            _data = data;
            IsValid = true;
        }

        #endregion
    }
}