namespace java.math.utils
{
	internal interface PowerIterator
	{

		/**
		 * Returns the current power.
		 * 
		 * @return the current power.
		 */
		BigDecimal getCurrentPower();

		/**
		 * Calculates the next power.
		 */
		void calculateNextPower();
	}
}
