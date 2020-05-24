﻿namespace java.math
{
	public static class BigComplexMath
	{
		private static BigComplex I = BigComplex.I;
		private static BigDecimal TWO = BigDecimal.valueOf(2);

		/**
		 * Calculates the reciprocal of the given complex number using the specified {@link MathContext}.
		 *
		 * @param x the complex number to calculate the reciprocal
		 * @param mathContext the {@link MathContext} used to calculate the result
		 * @return the calculated {@link BigComplex} result
		 * @see BigComplex#reciprocal(MathContext)
		 */
		public static BigComplex reciprocal(this BigComplex x, MathContext mathContext)
		{
			return x.reciprocal(mathContext);
		}

		/**
		 * Calculates the conjugate of the given complex number using the specified {@link MathContext}.
		 *
		 * @param x the complex number to calculate the conjugate
		 * @return the calculated {@link BigComplex} result
		 * @see BigComplex#conjugate()
		 */
		public static BigComplex conjugate(this BigComplex x)
		{
			return x.conjugate();
		}

		/**
		 * Calculates the absolute value (also known as magnitude, length or radius) of the given complex number using the specified {@link MathContext}.
		 *
		 * @param x the complex number to calculate the absolute value
		 * @param mathContext the {@link MathContext} used to calculate the result
		 * @return the calculated {@link BigComplex} result
		 * @see BigComplex#abs(MathContext)
		 */
		public static BigDecimal abs(this BigComplex x, MathContext mathContext)
		{
			return x.abs(mathContext);
		}

		/**
		 * Calculates the square of the absolute value (also known as magnitude, length or radius) of the given complex number using the specified {@link MathContext}.
		 *
		 * @param x the complex number to calculate the square of the absolute value
		 * @param mathContext the {@link MathContext} used to calculate the result
		 * @return the calculated {@link BigComplex} result
		 * @see BigComplex#absSquare(MathContext)
		 */
		public static BigDecimal absSquare(this BigComplex x, MathContext mathContext)
		{
			return x.absSquare(mathContext);
		}

		/**
		 * Calculates the angle in radians of the given complex number using the specified {@link MathContext}.
		 *
		 * @param x the complex number to calculate the angle
		 * @param mathContext the {@link MathContext} used to calculate the result
		 * @return the calculated {@link BigComplex} angle in radians
		 * @see BigComplex#angle(MathContext)
		 */
		public static BigDecimal angle(this BigComplex x, MathContext mathContext)
		{
			return x.angle(mathContext);
		}


		/**
		 * Calculates the natural exponent of {@link BigComplex} x (e<sup>x</sup>) in the complex domain.
		 *
		 * <p>See: <a href="https://en.wikipedia.org/wiki/Exponential_function#Complex_plane">Wikipedia: Exponent (Complex plane)</a></p>
		 *
		 * @param x the {@link BigComplex} to calculate the exponent for
		 * @param mathContext the {@link MathContext} used for the result
		 * @return the calculated exponent {@link BigComplex} with the precision specified in the <code>mathContext</code>
		 */
		public static BigComplex exp(this BigComplex x, MathContext mathContext)
		{
			MathContext mc = new MathContext(mathContext.getPrecision() + 4, mathContext.getRoundingMode());

			BigDecimal expRe = BigDecimalMath.exp(x.re, mc);
			return BigComplex.valueOf(
					expRe.multiply(BigDecimalMath.cos(x.im, mc), mc).round(mathContext),
					expRe.multiply(BigDecimalMath.sin(x.im, mc), mc)).round(mathContext);
		}

		/**
		 * Calculates the sine (sinus) of {@link BigComplex} x in the complex domain.
		 *
		 * <p>See: <a href="https://en.wikipedia.org/wiki/Sine#Sine_with_a_complex_argument">Wikipedia: Sine (Sine with a complex argument)</a></p>
		 *
		 * @param x the {@link BigComplex} to calculate the sine for
		 * @param mathContext the {@link MathContext} used for the result
		 * @return the calculated sine {@link BigComplex} with the precision specified in the <code>mathContext</code>
		 */
		public static BigComplex sin(this BigComplex x, MathContext mathContext)
		{
			MathContext mc = new MathContext(mathContext.getPrecision() + 4, mathContext.getRoundingMode());

			return BigComplex.valueOf(
					BigDecimalMath.sin(x.re, mc).multiply(BigDecimalMath.cosh(x.im, mc), mc).round(mathContext),
					BigDecimalMath.cos(x.re, mc).multiply(BigDecimalMath.sinh(x.im, mc), mc).round(mathContext));
		}

		/**
		 * Calculates the cosine (cosinus) of {@link BigComplex} x in the complex domain.
		 *
		 * @param x the {@link BigComplex} to calculate the cosine for
		 * @param mathContext the {@link MathContext} used for the result
		 * @return the calculated cosine {@link BigComplex} with the precision specified in the <code>mathContext</code>
		 */
		public static BigComplex cos(this BigComplex x, MathContext mathContext)
		{
			MathContext mc = new MathContext(mathContext.getPrecision() + 4, mathContext.getRoundingMode());

			return BigComplex.valueOf(
					BigDecimalMath.cos(x.re, mc).multiply(BigDecimalMath.cosh(x.im, mc), mc).round(mathContext),
					BigDecimalMath.sin(x.re, mc).multiply(BigDecimalMath.sinh(x.im, mc), mc).negate().round(mathContext));
		}

		// 
		// http://scipp.ucsc.edu/~haber/archives/physics116A10/arc_10.pdf

		/**
		 * Calculates the tangens of {@link BigComplex} x in the complex domain.
		 *
		 * @param x the {@link BigComplex} to calculate the tangens for
		 * @param mathContext the {@link MathContext} used for the result
		 * @return the calculated tangens {@link BigComplex} with the precision specified in the <code>mathContext</code>
		 */
		public static BigComplex tan(this BigComplex x, MathContext mathContext)
		{
			MathContext mc = new MathContext(mathContext.getPrecision() + 4, mathContext.getRoundingMode());

			return sin(x, mc).divide(cos(x, mc), mc).round(mathContext);
		}

		/**
		 * Calculates the arc tangens (inverted tangens) of {@link BigComplex} x in the complex domain.
		 *
		 * <p>See: <a href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Extension_to_complex_plane">Wikipedia: Inverse trigonometric functions (Extension to complex plane)</a></p>
		 *
		 * @param x the {@link BigComplex} to calculate the arc tangens for
		 * @param mathContext the {@link MathContext} used for the result
		 * @return the calculated arc tangens {@link BigComplex} with the precision specified in the <code>mathContext</code>
		 */
		public static BigComplex atan(this BigComplex x, MathContext mathContext)
		{
			MathContext mc = new MathContext(mathContext.getPrecision() + 4, mathContext.getRoundingMode());

			return log(I.subtract(x, mc).divide(I.add(x, mc), mc), mc).divide(I, mc).divide(TWO, mc).round(mathContext);
		}

		/**
		 * Calculates the arc cotangens (inverted cotangens) of {@link BigComplex} x in the complex domain.
		 *
		 * <p>See: <a href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Extension_to_complex_plane">Wikipedia: Inverse trigonometric functions (Extension to complex plane)</a></p>
		 *
		 * @param x the {@link BigComplex} to calculate the arc cotangens for
		 * @param mathContext the {@link MathContext} used for the result
		 * @return the calculated arc cotangens {@link BigComplex} with the precision specified in the <code>mathContext</code>
		 */
		public static BigComplex acot(this BigComplex x, MathContext mathContext)
		{
			MathContext mc = new MathContext(mathContext.getPrecision() + 4, mathContext.getRoundingMode());

			return log(x.add(I, mc).divide(x.subtract(I, mc), mc), mc).divide(I, mc).divide(TWO, mc).round(mathContext);
		}

		/**
		 * Calculates the arc sine (inverted sine) of {@link BigComplex} x in the complex domain.
		 *
		 * <p>See: <a href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Extension_to_complex_plane">Wikipedia: Inverse trigonometric functions (Extension to complex plane)</a></p>
		 *
		 * @param x the {@link BigComplex} to calculate the arc sine for
		 * @param mathContext the {@link MathContext} used for the result
		 * @return the calculated arc sine {@link BigComplex} with the precision specified in the <code>mathContext</code>
		 */
		public static BigComplex asin(this BigComplex x, MathContext mathContext)
		{
			MathContext mc = new MathContext(mathContext.getPrecision() + 4, mathContext.getRoundingMode());

			return I.negate().multiply(log(I.multiply(x, mc).add(sqrt(BigComplex.ONE.subtract(x.multiply(x, mc), mc), mc), mc), mc), mc).round(mathContext);
		}

		/**
		 * Calculates the arc cosine (inverted cosine) of {@link BigComplex} x in the complex domain.
		 *
		 * <p>See: <a href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Extension_to_complex_plane">Wikipedia: Inverse trigonometric functions (Extension to complex plane)</a></p>
		 *
		 * @param x the {@link BigComplex} to calculate the arc cosine for
		 * @param mathContext the {@link MathContext} used for the result
		 * @return the calculated arc cosine {@link BigComplex} with the precision specified in the <code>mathContext</code>
		 */
		public static BigComplex acos(this BigComplex x, MathContext mathContext)
		{
			MathContext mc = new MathContext(mathContext.getPrecision() + 4, mathContext.getRoundingMode());

			return I.negate().multiply(log(x.add(sqrt(x.multiply(x, mc).subtract(BigComplex.ONE, mc), mc), mc), mc), mc).round(mathContext);
		}

		/**
		 * Calculates the square root of {@link BigComplex} x in the complex domain (√x).
		 *
		 * <p>See <a href="https://en.wikipedia.org/wiki/Square_root#Square_root_of_an_imaginary_number">Wikipedia: Square root (Square root of an imaginary number)</a></p>
		 *
		 * @param x the {@link BigComplex} to calculate the square root for
		 * @param mathContext the {@link MathContext} used for the result
		 * @return the calculated square root {@link BigComplex} with the precision specified in the <code>mathContext</code>
		 */
		public static BigComplex sqrt(this BigComplex x, MathContext mathContext)
		{
			// https://math.stackexchange.com/questions/44406/how-do-i-get-the-square-root-of-a-complex-number
			MathContext mc = new MathContext(mathContext.getPrecision() + 4, mathContext.getRoundingMode());

			BigDecimal magnitude = x.abs(mc);

			BigComplex a = x.add(magnitude, mc);
			return a.divide(a.abs(mc), mc).multiply(BigDecimalMath.sqrt(magnitude, mc), mc).round(mathContext);
		}

		/**
		 * Calculates the natural logarithm of {@link BigComplex} x in the complex domain.
		 *
		 * <p>See: <a href="https://en.wikipedia.org/wiki/Complex_logarithm">Wikipedia: Complex logarithm</a></p>
		 *
		 * @param x the {@link BigComplex} to calculate the natural logarithm for
		 * @param mathContext the {@link MathContext} used for the result
		 * @return the calculated natural logarithm {@link BigComplex} with the precision specified in the <code>mathContext</code>
		 */
		public static BigComplex log(this BigComplex x, MathContext mathContext)
		{
			// https://en.wikipedia.org/wiki/Complex_logarithm
			MathContext mc1 = new MathContext(mathContext.getPrecision() + 20, mathContext.getRoundingMode());
			MathContext mc2 = new MathContext(mathContext.getPrecision() + 5, mathContext.getRoundingMode());

			return BigComplex.valueOf(
					BigDecimalMath.log(x.abs(mc1), mc1).round(mathContext),
					x.angle(mc2)).round(mathContext);
		}

		/**
		 * Calculates {@link BigComplex} x to the power of <code>long</code> y (x<sup>y</sup>).
		 *
		 * <p>The implementation tries to minimize the number of multiplications of {@link BigComplex x} (using squares whenever possible).</p>
		 *
		 * <p>See: <a href="https://en.wikipedia.org/wiki/Exponentiation#Efficient_computation_with_integer_exponents">Wikipedia: Exponentiation - efficient computation</a></p>
		 *
		 * @param x the {@link BigComplex} value to take to the power
		 * @param y the <code>long</code> value to serve as exponent
		 * @param mathContext the {@link MathContext} used for the result
		 * @return the calculated x to the power of y with the precision specified in the <code>mathContext</code>
		 */
		public static BigComplex pow(this BigComplex x, long y, MathContext mathContext)
		{
			MathContext mc = new MathContext(mathContext.getPrecision() + 10, mathContext.getRoundingMode());

			if (y < 0)
			{
				return BigComplex.ONE.divide(pow(x, -y, mc), mc).round(mathContext);
			}

			BigComplex result = BigComplex.ONE;
			while (y > 0)
			{
				if ((y & 1) == 1)
				{
					// odd exponent -> multiply result with x
					result = result.multiply(x, mc);
					y -= 1;
				}

				if (y > 0)
				{
					// even exponent -> square x
					x = x.multiply(x, mc);
				}

				y >>= 1;
			}

			return result.round(mathContext);
		}

		/**
		 * Calculates {@link BigComplex} x to the power of {@link BigDecimal} y (x<sup>y</sup>).
		 *
		 * @param x the {@link BigComplex} value to take to the power
		 * @param y the {@link BigDecimal} value to serve as exponent
		 * @param mathContext the {@link MathContext} used for the result
		 * @return the calculated x to the power of y with the precision specified in the <code>mathContext</code>
		 */
		public static BigComplex pow(this BigComplex x, BigDecimal y, MathContext mathContext)
		{
			MathContext mc = new MathContext(mathContext.getPrecision() + 4, mathContext.getRoundingMode());

			BigDecimal angleTimesN = x.angle(mc).multiply(y, mc);
			return BigComplex.valueOf(
					BigDecimalMath.cos(angleTimesN, mc),
					BigDecimalMath.sin(angleTimesN, mc)).multiply(BigDecimalMath.pow(x.abs(mc), y, mc), mc).round(mathContext);
		}

		/**
		 * Calculates {@link BigComplex} x to the power of {@link BigComplex} y (x<sup>y</sup>).
		 *
		 * @param x the {@link BigComplex} value to take to the power
		 * @param y the {@link BigComplex} value to serve as exponent
		 * @param mathContext the {@link MathContext} used for the result
		 * @return the calculated x to the power of y with the precision specified in the <code>mathContext</code>
		 */
		public static BigComplex pow(this BigComplex x, BigComplex y, MathContext mathContext)
		{
			MathContext mc = new MathContext(mathContext.getPrecision() + 4, mathContext.getRoundingMode());

			return exp(y.multiply(log(x, mc), mc), mc).round(mathContext);
		}

		/**
		 * Calculates the {@link BigDecimal} n'th root of {@link BigComplex} x (<sup>n</sup>√x).
		 *
		 * <p>See <a href="http://en.wikipedia.org/wiki/Square_root">Wikipedia: Square root</a></p>
		 * @param x the {@link BigComplex} value to calculate the n'th root
		 * @param n the {@link BigDecimal} defining the root
		 * @param mathContext the {@link MathContext} used for the result
		 *
		 * @return the calculated n'th root of x with the precision specified in the <code>mathContext</code>
		 */
		public static BigComplex root(this BigComplex x, BigDecimal n, MathContext mathContext)
		{
			MathContext mc = new MathContext(mathContext.getPrecision() + 4, mathContext.getRoundingMode());

			return pow(x, BigDecimal.ONE.divide(n, mc), mc).round(mathContext);
		}

		/**
		 * Calculates the {@link BigComplex} n'th root of {@link BigComplex} x (<sup>n</sup>√x).
		 *
		 * <p>See <a href="http://en.wikipedia.org/wiki/Square_root">Wikipedia: Square root</a></p>
		 * @param x the {@link BigComplex} value to calculate the n'th root
		 * @param n the {@link BigComplex} defining the root
		 * @param mathContext the {@link MathContext} used for the result
		 *
		 * @return the calculated n'th root of x with the precision specified in the <code>mathContext</code>
		 */
		public static BigComplex root(this BigComplex x, BigComplex n, MathContext mathContext)
		{
			MathContext mc = new MathContext(mathContext.getPrecision() + 4, mathContext.getRoundingMode());

			return pow(x, BigComplex.ONE.divide(n, mc), mc).round(mathContext);
		}

		// TODO add root() for the k'th root - https://math.stackexchange.com/questions/322481/principal-nth-root-of-a-complex-number 
	}
}
