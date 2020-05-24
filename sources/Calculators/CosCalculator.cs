using System;

namespace java.math.utils
{
	internal class CosCalculator : SeriesCalculator
	{	
		public static CosCalculator INSTANCE = new CosCalculator();
		private int n = 0;
		private Boolean negative = false;
		private BigRational factorial2n = BigRational.ONE;

		private CosCalculator()
		{
			//super(true);
		}

		protected override BigRational getCurrentFactor()
		{
			BigRational factor = factorial2n.reciprocal();
			if (negative)
			{
				factor = factor.negate();
			}
			return factor;
		}

		protected override void calculateNextFactor()
		{
			n++;
			factorial2n = factorial2n.multiply(2 * n - 1).multiply(2 * n);
			negative = !negative;
		}

		protected override PowerIterator createPowerIterator(BigDecimal x, MathContext mathContext)
		{
			return new PowerTwoNIterator(x, mathContext);
		}
	}
}
