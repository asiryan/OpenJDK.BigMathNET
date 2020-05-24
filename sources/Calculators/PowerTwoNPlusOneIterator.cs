namespace java.math.utils
{
	internal class PowerTwoNPlusOneIterator : PowerIterator
	{
		private MathContext mathContext;

		private BigDecimal xPowerTwo;

		private BigDecimal powerOfX;

		public PowerTwoNPlusOneIterator(BigDecimal x, MathContext mathContext)
		{
			this.mathContext = mathContext;

			xPowerTwo = x.multiply(x, mathContext);
			powerOfX = x;
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
