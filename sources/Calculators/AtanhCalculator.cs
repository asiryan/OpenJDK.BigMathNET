namespace java.math.utils
{
	internal class AtanhCalculator : SeriesCalculator
	{
		public static AtanhCalculator INSTANCE = new AtanhCalculator();
		private int n = 0;

		private AtanhCalculator()
		{
			//super(true);
		}

		protected override BigRational getCurrentFactor()
		{
			return BigRational.valueOf(1, 2 * n + 1);
		}

		protected override void calculateNextFactor()
		{
			n++;
		}

		protected override PowerIterator createPowerIterator(BigDecimal x, MathContext mathContext)
		{
			return new PowerTwoNPlusOneIterator(x, mathContext);
		}
	}
}
