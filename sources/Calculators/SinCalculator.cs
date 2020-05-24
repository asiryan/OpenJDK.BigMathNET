using System;

namespace java.math.utils
{
	internal class SinCalculator : SeriesCalculator
	{
		public static SinCalculator INSTANCE = new SinCalculator();

		private int n = 0;
		private Boolean negative = false;
		private BigRational factorial2nPlus1 = BigRational.ONE;

		private SinCalculator()
		{
			//super(true);
		}

		protected override BigRational getCurrentFactor()
		{
			BigRational factor = factorial2nPlus1.reciprocal();
			if (negative)
			{
				factor = factor.negate();
			}
			return factor;
		}

		protected override void calculateNextFactor()
		{
			n++;
			factorial2nPlus1 = factorial2nPlus1.multiply(2 * n);
			factorial2nPlus1 = factorial2nPlus1.multiply(2 * n + 1);
			negative = !negative;
		}

		protected override PowerIterator createPowerIterator(BigDecimal x, MathContext mathContext)
		{
			return new PowerTwoNPlusOneIterator(x, mathContext);
		}
	}
}
