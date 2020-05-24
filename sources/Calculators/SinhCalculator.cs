namespace java.math.utils
{
	internal class SinhCalculator : SeriesCalculator
	{
		public static SinhCalculator INSTANCE = new SinhCalculator();

		private int n = 0;

		private BigRational factorial2nPlus1 = BigRational.ONE;

		private SinhCalculator()
		{
			//super(true);
		}

		protected override BigRational getCurrentFactor()
		{
			return factorial2nPlus1.reciprocal();
		}

		protected override void calculateNextFactor()
		{
			n++;
			factorial2nPlus1 = factorial2nPlus1.multiply(2 * n);
			factorial2nPlus1 = factorial2nPlus1.multiply(2 * n + 1);
		}

		protected override PowerIterator createPowerIterator(BigDecimal x, MathContext mathContext)
		{
			return new PowerTwoNPlusOneIterator(x, mathContext);
		}
	}
}
