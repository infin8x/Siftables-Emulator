namespace FractionOrderingApp
{
    internal class Fraction
    {
        private readonly int _denominator;
        private readonly int _numerator;

        public Fraction(int numerator)
        {
            _numerator = numerator;
            _denominator = 1;
        }

        public Fraction(int numerator, int denominator)
        {
            _numerator = numerator;
            _denominator = 0 == denominator ? 1 : denominator;
        }

        public int GetNumerator()
        {
            return _numerator;
        }

        public int GetDenominator()
        {
            return _denominator;
        }

        public bool GreaterThan(Fraction fraction)
        {
            return (1.0 * _numerator/_denominator) >= (1.0 * fraction.GetNumerator()/fraction.GetDenominator());
        }
    }
}