namespace java.math.utils
{
	internal class AsinCalculator : SeriesCalculator
	{
		public static AsinCalculator INSTANCE = new AsinCalculator();

		private int n = 0;
		private BigRational factorial2n = BigRational.ONE;
		private BigRational factorialN = BigRational.ONE;
		private BigRational fourPowerN = BigRational.ONE;

		private AsinCalculator()
		{
		}

		protected override BigRational getCurrentFactor()
		{
			BigRational factor = factorial2n.divide(fourPowerN.multiply(factorialN).multiply(factorialN).multiply(2 * n + 1));
			return factor;
		}

		protected override void calculateNextFactor()
		{
			n++;
			factorial2n = factorial2n.multiply(2 * n - 1).multiply(2 * n);
			factorialN = factorialN.multiply(n);
			fourPowerN = fourPowerN.multiply(4);
		}

		protected override PowerIterator createPowerIterator(BigDecimal x, MathContext mathContext)
		{
			return new PowerTwoNPlusOneIterator(x, mathContext);
		}
	}
}
