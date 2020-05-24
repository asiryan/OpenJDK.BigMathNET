namespace java.math.utils
{
	internal class PowerTwoNMinusOneIterator : PowerIterator
	{
		private MathContext mathContext;

		private BigDecimal xPowerTwo;

		private BigDecimal powerOfX;

		public PowerTwoNMinusOneIterator(BigDecimal x, MathContext mathContext)
		{
			this.mathContext = mathContext;

			xPowerTwo = x.multiply(x, mathContext);
			powerOfX = BigDecimalMath.reciprocal(x, mathContext);
		}

		public BigDecimal getCurrentPower()
		{
			return powerOfX;
		}

		public void calculateNextPower()
		{
			powerOfX = powerOfX.multiply(xPowerTwo, mathContext);
		}
	}
}
