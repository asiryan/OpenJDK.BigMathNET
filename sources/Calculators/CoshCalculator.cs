namespace java.math.utils
{
	internal class CoshCalculator : SeriesCalculator
	{
		public static CoshCalculator INSTANCE = new CoshCalculator();

		private int n = 0;
	
		private BigRational factorial2n = BigRational.ONE;

		private CoshCalculator()
		{
			//super(true);
		}

		protected override BigRational getCurrentFactor()
		{
			return factorial2n.reciprocal();
		}


		protected override void calculateNextFactor()
		{
			n++;
			factorial2n = factorial2n.multiply(2 * n - 1).multiply(2 * n);
		}

		protected override PowerIterator createPowerIterator(BigDecimal x, MathContext mathContext)
		{
			return new PowerTwoNIterator(x, mathContext);
		}
	}
}
