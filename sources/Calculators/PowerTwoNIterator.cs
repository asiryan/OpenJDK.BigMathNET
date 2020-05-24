namespace java.math.utils
{
	internal class PowerTwoNIterator : PowerIterator
	{
		private MathContext mathContext;
		private BigDecimal xPowerTwo;
		private BigDecimal powerOfX;

		public PowerTwoNIterator(BigDecimal x, MathContext mathContext)
		{
			this.mathContext = mathContext;

			xPowerTwo = x.multiply(x, mathContext);
			powerOfX = BigDecimal.ONE;
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
