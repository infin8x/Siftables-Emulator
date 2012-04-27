namespace FractionOrderingApp
{
    internal class Fraction
    {
        private int _Denominator;
        private int _Numerator;

        public Fraction(int numerator)
        {
            _Numerator = numerator;
            _Denominator = 1;
        }

        public Fraction(int numerator, int denominator)
        {
            _Numerator = numerator;

            if (0 == denominator)
                _Denominator = 1;
            else
                _Denominator = denominator;
        }

        public int GetNumerator()
        {
            return _Numerator;
        }

        public int GetDenominator()
        {
            return _Denominator;
        }

        public bool GreaterThen(Fraction fraction)
        {
            return (1.0 * _Numerator/_Denominator) >= (1.0 * fraction.GetNumerator()/fraction.GetDenominator());
        }
    }
}