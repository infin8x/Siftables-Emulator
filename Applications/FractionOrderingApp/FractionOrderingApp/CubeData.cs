namespace FractionOrderingApp
{
    internal class CubeData
    {
        private readonly Fraction _fraction;
        private bool _ordered;

        public CubeData(Fraction fraction = null, bool ordering = true)
        {
            _fraction = fraction;
            _ordered = ordering;
        }

        public Fraction GetFraction()
        {
            return _fraction;
        }

        public bool IsOrdered()
        {
            return _ordered;
        }

        public bool GreaterThan(CubeData cubeData)
        {
            return _fraction.GreaterThan(cubeData.GetFraction());
        }

        public void setOrder(bool ordering)
        {
            _ordered = ordering;
        }
    }
}