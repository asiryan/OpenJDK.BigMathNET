namespace java.math.utils
{
	internal class ExpCalculator : SeriesCalculator
	{

		public static ExpCalculator INSTANCE = new ExpCalculator();
		private int n = 0;
		private BigRational oneOverFactorialOfN = BigRational.ONE;

		private ExpCalculator()
		{
			// prevent instances
		}

		protected override BigRational getCurrentFactor()
		{
			return oneOverFactorialOfN;
		}

		protected override void calculateNextFactor()
		{
			n++;
			oneOverFactorialOfN = oneOverFactorialOfN.divide(n);
		}

		protected override PowerIterator createPowerIterator(BigDecimal x, MathContext mathContext)
		{
			return new PowerNIterator(x, mathContext);
		}
	}
}
