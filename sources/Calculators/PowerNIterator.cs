namespace java.math.utils
{
	internal class PowerNIterator : PowerIterator
	{

		private BigDecimal x;

		private MathContext mathContext;

		private BigDecimal powerOfX;

		public PowerNIterator(BigDecimal x, MathContext mathContext)
		{
			this.x = x;
			this.mathContext = mathContext;

			powerOfX = BigDecimal.ONE;
		}

		public BigDecimal getCurrentPower()
		{
			return powerOfX;
		}

		public void calculateNextPower()
		{
			powerOfX = powerOfX.multiply(x, mathContext);
		}
	}
}
