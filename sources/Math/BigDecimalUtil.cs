using System;
using System.Text;

namespace java.math
{
    /// <summary>
    /// 
    /// </summary>
    public static class BigDecimalMath2
    {
        #region Nums
        /// <summary>
        /// Max number of integer power.
        /// </summary>
        private const int MAXPOW = 1000;
        /// <summary>
        /// Scale.
        /// </summary>
        public static int SCALE = 32;
        /// <summary>
        /// Max number of iterations.
        /// </summary>
        public static long MAXITER = 1000;
        /// <summary>
        /// Rounding mode.
        /// </summary>
        public static int ROUNDING_MODE = BigDecimal.ROUND_DOWN;
        #endregion

        #region Math
        /// <summary>
        /// Returns the sqaure root of the value.
        /// </summary>
        /// <param name="x">Value</param>
        /// <returns>Result</returns>
        public static BigDecimal sqrt(BigDecimal x)
        {
            // Check that x >= 0.
            if (x.signum() < 0)
            {
                throw new ArithmeticException("x < 0");
            }

            // n = x*(10^(2*SCALE))
            BigInteger n = x.movePointRight(SCALE << 1).toBigInteger();

            // The first approximation is the upper half of n.
            int bits = (n.bitLength() + 1) >> 1;
            BigInteger ix = n.shiftRight(bits);
            BigInteger ixPrev;

            // Loop until the approximations converge
            // (two successive approximations are equal after rounding).
            do
            {
                ixPrev = ix;

                // x = (x + n/x)/2
                ix = ix.add(n.divide(ix)).shiftRight(1);

                //Thread.yield();
            } while (ix.compareTo(ixPrev) != 0);

            return new BigDecimal(ix, SCALE);
        }
        /// <summary>
        /// Returns the integer root of the value.
        /// </summary>
        /// <param name="x">Value</param>
        /// <param name="index">Index</param>
        /// <param name="scale">Scale</param>
        /// <returns>Results</returns>
        public static BigDecimal intRoot(BigDecimal x, long index, int scale)
        {
            // Check that x >= 0.
            if (x.signum() < 0)
            {
                throw new ArgumentException("x < 0");
            }

            int sp1 = scale + 1;
            BigDecimal n = x;
            BigDecimal i = BigDecimal.valueOf(index);
            BigDecimal im1 = BigDecimal.valueOf(index - 1);
            BigDecimal tolerance = BigDecimal.valueOf(5)
                                                .movePointLeft(sp1);
            BigDecimal xPrev;

            // The initial approximation is x/index.
            x = x.divide(i, scale, BigDecimal.ROUND_HALF_EVEN);

            // Loop until the approximations converge
            // (two successive approximations are equal after rounding).
            do
            {
                // x^(index-1)
                BigDecimal xToIm1 = intPower(x, index - 1, sp1);

                // x^index
                BigDecimal xToI =
                        x.multiply(xToIm1)
                            .setScale(sp1, BigDecimal.ROUND_HALF_EVEN);

                // n + (index-1)*(x^index)
                BigDecimal numerator =
                        n.add(im1.multiply(xToI))
                            .setScale(sp1, BigDecimal.ROUND_HALF_EVEN);

                // (index*(x^(index-1))
                BigDecimal denominator =
                        i.multiply(xToIm1)
                            .setScale(sp1, BigDecimal.ROUND_HALF_EVEN);

                // x = (n + (index-1)*(x^index)) / (index*(x^(index-1)))
                xPrev = x;
                x = numerator
                        .divide(denominator, sp1, BigDecimal.ROUND_DOWN);

                //Thread.yield();
            } while (x.subtract(xPrev).abs().compareTo(tolerance) > 0);

            return x;
        }
        /// <summary>
        /// Returns the natural logarithm of the value.
        /// </summary>
        /// <param name="x">Value</param>
        /// <param name="scale">Scale</param>
        /// <returns>Result</returns>
        public static BigDecimal log(BigDecimal x, int scale)
        {
            // Check that x > 0.
            if (x.signum() <= 0)
            {
                throw new ArgumentException("x <= 0");
            }

            // The number of digits to the left of the decimal point.
            int magnitude = x.toString().Length - x.scale() - 1;

            if (magnitude < 3)
            {
                return logNewton(x, scale);
            }

            // Compute magnitude*ln(x^(1/magnitude)).
            else
            {

                // x^(1/magnitude)
                BigDecimal root = intRoot(x, magnitude, scale);

                // ln(x^(1/magnitude))
                BigDecimal lnRoot = logNewton(root, scale);

                // magnitude*ln(x^(1/magnitude))
                return BigDecimal.valueOf(magnitude).multiply(lnRoot)
                            .setScale(scale, BigDecimal.ROUND_HALF_EVEN);
            }
        }
        /// <summary>
        /// Returns the Newton product.
        /// </summary>
        /// <param name="x">Value</param>
        /// <param name="scale">Scale</param>
        /// <returns>Result</returns>
        private static BigDecimal logNewton(BigDecimal x, int scale)
        {
            int sp1 = scale + 1;
            BigDecimal n = x;
            BigDecimal term;

            // Convergence tolerance = 5*(10^-(scale+1))
            BigDecimal tolerance = BigDecimal.valueOf(5)
                                                .movePointLeft(sp1);

            // Loop until the approximations converge
            // (two successive approximations are within the tolerance).
            do
            {

                // e^x
                BigDecimal eToX = exp(x, sp1);

                // (e^x - n)/e^x
                term = eToX.subtract(n)
                            .divide(eToX, sp1, BigDecimal.ROUND_DOWN);

                // x - (e^x - n)/e^x
                x = x.subtract(term);

                //Thread.yield();
            } while (term.compareTo(tolerance) > 0);

            return x.setScale(scale, BigDecimal.ROUND_HALF_EVEN);
        }
        /// <summary>
        /// Returns the cosine of the value.
        /// </summary>
        /// <param name="x">Value</param>
        /// <returns>Result</returns>
        public static BigDecimal cos(BigDecimal x)
        {

            BigDecimal currentValue = BigDecimal.ONE;
            BigDecimal lastVal = currentValue.add(BigDecimal.ONE);
            BigDecimal xSquared = x.multiply(x);
            BigDecimal numerator = BigDecimal.ONE;
            BigDecimal denominator = BigDecimal.ONE;
            int i = 0;

            while (lastVal.compareTo(currentValue) != 0)
            {
                lastVal = currentValue;

                int z = 2 * i + 2;

                denominator = denominator.multiply(BigDecimal.valueOf(z));
                denominator = denominator.multiply(BigDecimal.valueOf(z - 1));
                numerator = numerator.multiply(xSquared);

                BigDecimal term = numerator.divide(denominator, SCALE + 5, ROUNDING_MODE);

                if (i % 2 == 0)
                {
                    currentValue = currentValue.subtract(term);
                }
                else
                {
                    currentValue = currentValue.add(term);
                }
                i++;
            }

            return currentValue;
        }
        /// <summary>
        /// Returns the sine of the value.
        /// </summary>
        /// <param name="x">Value</param>
        /// <returns>Result</returns>
        public static BigDecimal sin(BigDecimal x)
        {
            BigDecimal lastVal = x.add(BigDecimal.ONE);
            BigDecimal currentValue = x;
            BigDecimal xSquared = x.multiply(x);
            BigDecimal numerator = x;
            BigDecimal denominator = BigDecimal.ONE;
            int i = 0;

            while (lastVal.compareTo(currentValue) != 0)
            {
                lastVal = currentValue;

                int z = 2 * i + 3;

                denominator = denominator.multiply(BigDecimal.valueOf(z));
                denominator = denominator.multiply(BigDecimal.valueOf(z - 1));
                numerator = numerator.multiply(xSquared);

                BigDecimal term = numerator.divide(denominator, SCALE + 5, ROUNDING_MODE);

                if (i % 2 == 0)
                {
                    currentValue = currentValue.subtract(term);
                }
                else
                {
                    currentValue = currentValue.add(term);
                }

                i++;
            }
            return currentValue;
        }
        /// <summary>
        /// Returns the tangent of the value.
        /// </summary>
        /// <param name="x">Value</param>
        /// <returns>Result</returns>
        public static BigDecimal tan(BigDecimal x)
        {
            BigDecimal _sin = sin(x);
            BigDecimal _cos = cos(x);

            return _sin.divide(_cos, SCALE, BigDecimal.ROUND_HALF_UP);
        }
        /// <summary>
        /// Returns the logarithm of the value.
        /// </summary>
        /// <param name="x">Value</param>
        /// <returns>Result</returns>
        public static BigDecimal log10(BigDecimal x)
        {
            int NUM_OF_DIGITS = SCALE + 2;
            // need to add one to get the right number of dp
            //  and then add one again to get the next number
            //  so I can round it correctly.

            MathContext mc = new MathContext(NUM_OF_DIGITS, RoundingMode.HALF_EVEN);
            //special conditions:
            // log(-x) -> exception
            // log(1) == 0 exactly;
            // log of a number lessthan one = -log(1/x)
            if (x.signum() <= 0)
            {
                throw new ArithmeticException("log of a negative number! (or zero)");
            }
            else if (x.compareTo(BigDecimal.ONE) == 0)
            {
                return BigDecimal.ZERO;
            }
            else if (x.compareTo(BigDecimal.ONE) < 0)
            {
                return (log10((BigDecimal.ONE).divide(x, mc))).negate();
            }

            StringBuilder sb = new StringBuilder();
            //number of digits on the left of the decimal point
            int leftDigits = x.precision() - x.scale();

            //so, the first digits of the log10 are:
            sb.Append(leftDigits - 1).Append(".");

            //this is the algorithm outlined in the webpage
            int n = 0;
            while (n < NUM_OF_DIGITS)
            {
                x = (x.movePointLeft(leftDigits - 1)).pow(10, mc);
                leftDigits = x.precision() - x.scale();
                sb.Append(leftDigits - 1);
                n++;
            }

            BigDecimal ans = new BigDecimal(sb.ToString());

            //Round the number to the correct number of decimal places.
            ans = ans.round(new MathContext(ans.precision() - ans.scale() + SCALE, RoundingMode.HALF_EVEN));
            return ans;
        }
        /// <summary>
        /// Returns the cube root of the value.
        /// </summary>
        /// <param name="x">Value</param>
        /// <returns>Result</returns>
        public static BigDecimal cuberoot(BigDecimal x)
        {
            // Specify a math context with 40 digits of precision.

            MathContext mc = new MathContext(40);

            BigDecimal y = new BigDecimal("1", mc);

            // Search for the cube root via the Newton-Raphson loop. Output each // successive iteration's value.

            for (int i = 0; i < MAXITER; i++)
            {
                y = y.subtract(y.pow(3, mc).subtract(x, mc).divide(new BigDecimal("3", mc).multiply(y.pow(2, mc), mc), mc), mc);
            }
            return y;
        }
        /// <summary>
        /// Returns the number raised to the power.
        /// </summary>
        /// <param name="a">Value</param>
        /// <param name="n">Power</param>
        /// <returns>Returns</returns>
        public static BigDecimal pow(BigDecimal a, BigDecimal n)
        {
            BigDecimal _exponent = integral(n);
            BigDecimal _mantissa = fraction(n);

            // A^B = { B = X + x } = A^X * A^x
            // right part of equation
            BigDecimal right = exp(log(a, SCALE).multiply(_mantissa), SCALE);

            // left part of equation
            BigDecimal tmp = BigDecimal.ONE;
            int i = 0;
            while (_exponent.abs().compareTo(BigDecimal.valueOf(MAXPOW)) == 1)
            {
                i++;
                int diff = _exponent.signum() > 0 ? MAXPOW : -MAXPOW;
                tmp = tmp.multiply(a.pow(diff));
                _exponent = _exponent.subtract(BigDecimal.valueOf(diff));
            }
            BigDecimal left = tmp.multiply(a.pow(_exponent.intValue()));

            // result
            BigDecimal result = left.multiply(right);
            return result;
        }
        /// <summary>
        /// Returns the number raised to the integer power.
        /// </summary>
        /// <param name="x">Value</param>
        /// <param name="exponent">Exponent</param>
        /// <param name="scale">Scale</param>
        /// <returns>Returns</returns>
        public static BigDecimal intPower(BigDecimal x, long exponent, int scale)
        {
            // If the exponent is negative, compute 1/(x^-exponent).
            if (exponent < 0)
            {
                return BigDecimal.valueOf(1)
                            .divide(intPower(x, -exponent, scale), scale,
                                    BigDecimal.ROUND_HALF_EVEN);
            }

            BigDecimal power = BigDecimal.valueOf(1);

            // Loop to compute value^exponent.
            while (exponent > 0)
            {

                // Is the rightmost bit a 1?
                if ((exponent & 1) == 1)
                {
                    power = power.multiply(x)
                              .setScale(scale, BigDecimal.ROUND_HALF_EVEN);
                }

                // Square x and shift exponent 1 bit to the right.
                x = x.multiply(x)
                        .setScale(scale, BigDecimal.ROUND_HALF_EVEN);
                exponent >>= 1;

                //Thread.yield();
            }

            return power;
        }
        /// <summary>
        /// Returns the exponent raised to the power.
        /// </summary>
        /// <param name="x">Value</param>
        /// <param name="scale">Scale</param>
        /// <returns>Returns</returns>
        public static BigDecimal exp(BigDecimal x, int scale)
        {
            // e^0 = 1
            if (x.signum() == 0)
            {
                return BigDecimal.valueOf(1);
            }

            // If x is negative, return 1/(e^-x).
            else if (x.signum() == -1)
            {
                return BigDecimal.valueOf(1)
                            .divide(exp(x.negate(), scale), scale,
                                    BigDecimal.ROUND_HALF_EVEN);
            }

            // Compute the whole part of x.
            BigDecimal xWhole = x.setScale(0, BigDecimal.ROUND_DOWN);

            // If there isn't a whole part, compute and return e^x.
            if (xWhole.signum() == 0)
            {
                return expTaylor(x, scale);
            }

            // Compute the fraction part of x.
            BigDecimal xFraction = x.subtract(xWhole);

            // z = 1 + fraction/whole
            BigDecimal z = BigDecimal.valueOf(1)
                                .add(xFraction.divide(
                                        xWhole, scale,
                                        BigDecimal.ROUND_HALF_EVEN));

            // t = e^z
            BigDecimal t = expTaylor(z, scale);

            BigDecimal maxLong = BigDecimal.valueOf(long.MaxValue);
            BigDecimal result = BigDecimal.valueOf(1);

            // Compute and return t^whole using intPower().
            // If whole > Long.MAX_VALUE, then first compute products
            // of e^Long.MAX_VALUE.
            while (xWhole.compareTo(maxLong) >= 0)
            {
                result = result.multiply(
                                    intPower(t, long.MaxValue, scale))
                            .setScale(scale, BigDecimal.ROUND_HALF_EVEN);
                xWhole = xWhole.subtract(maxLong);

                //Thread.yield();
            }
            return result.multiply(intPower(t, xWhole.longValue(), scale))
                            .setScale(scale, BigDecimal.ROUND_HALF_EVEN);
        }
        /// <summary>
        /// Returns the Taylor series product.
        /// </summary>
        /// <param name="x">Value</param>
        /// <param name="scale">Scale</param>
        /// <returns>Returns</returns>
        private static BigDecimal expTaylor(BigDecimal x, int scale)
        {
            BigDecimal factorial = BigDecimal.valueOf(1);
            BigDecimal xPower = x;
            BigDecimal sumPrev;

            // 1 + x
            BigDecimal sum = x.add(BigDecimal.valueOf(1));

            // Loop until the sums converge
            // (two successive sums are equal after rounding).
            int i = 2;
            do
            {
                // x^i
                xPower = xPower.multiply(x).setScale(scale, BigDecimal.ROUND_HALF_EVEN);

                // i!
                factorial = factorial.multiply(BigDecimal.valueOf(i));

                // x^i/i!
                BigDecimal term = xPower
                                    .divide(factorial, scale,
                                            BigDecimal.ROUND_HALF_EVEN);

                // sum = sum + x^i/i!
                sumPrev = sum;
                sum = sum.add(term);

                ++i;
                //Thread.yield();
            } while (sum.compareTo(sumPrev) != 0);

            return sum;
        }
        /// <summary>
        /// Returns the arcsine of the value.
        /// </summary>
        /// <param name="x">Value</param>
        /// <returns>Returns</returns>
        public static BigDecimal asin(BigDecimal x)
        {
            return BigDecimal.valueOf(Math.Asin(x.doubleValue()));
        }
        /// <summary>
        /// Returns the arccosine of the value.
        /// </summary>
        /// <param name="x">Value</param>
        /// <returns>Returns</returns>
        public static BigDecimal acos(BigDecimal x)
        {
            return BigDecimal.valueOf(Math.Acos(x.doubleValue()));
        }
        /// <summary>
        /// Returns the arctangent of the value.
        /// </summary>
        /// <param name="x">Value</param>
        /// <returns>Returns</returns>
        public static BigDecimal atan(BigDecimal x)
        {
            return BigDecimal.valueOf(Math.Atan(x.doubleValue()));
        }
        /// <summary>
        /// Returns the fraction part of the value.
        /// </summary>
        /// <param name="x">Value</param>
        /// <returns>Returns</returns>
        public static BigDecimal fraction(BigDecimal x)
        {
            BigDecimal _exponent = integral(x);
            return x.subtract(_exponent);
        }
        /// <summary>
        /// Returns the integral of the value.
        /// </summary>
        /// <param name="x">Value</param>
        /// <returns>Result</returns>
        public static BigDecimal integral(BigDecimal x)
        {
            return x.setScale(0, ROUNDING_MODE);
        }
        /// <summary>
        /// Returns the one diveded by the value.
        /// </summary>
        /// <param name="x">Value</param>
        /// <returns>Result</returns>
        public static BigDecimal reciprocal(BigDecimal x)
        {
            return BigDecimal.ONE.divide(x);
        }
        #endregion
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public static class BigDecimalMath
    //{
    //    #region Nums
    //    /// <summary>
    //    /// Max number of integer power.
    //    /// </summary>
    //    private const int MAXPOW = 1000000000;
    //    /// <summary>
    //    /// Scale.
    //    /// </summary>
    //    public static int SCALE = 64;
    //    #endregion

    //    #region Booleans
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="value"></param>
    //    /// <returns></returns>
    //    public static bool isInt(BigDecimal value)
    //    {
    //        // TODO impl isIntValue() without exceptions
    //        try
    //        {
    //            value.intValueExact();
    //            return true;
    //        }
    //        catch ////(ArithmeticException ex)
    //        {
    //            // ignored
    //        }
    //        return false;
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="value"></param>
    //    /// <returns></returns>
    //    public static bool isLong(BigDecimal value)
    //    {
    //        // TODO impl isLongValue() without exceptions
    //        try
    //        {
    //            value.longValueExact();
    //            return true;
    //        }
    //        catch ////(ArithmeticException ex)
    //        {
    //            // ignored
    //        }
    //        return false;
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="value"></param>
    //    /// <returns></returns>
    //    public static bool isDouble(BigDecimal value)
    //    {
    //        if (value.compareTo(double.MaxValue) > 0)
    //        {
    //            return false;
    //        }
    //        if (value.compareTo(double.MinValue) < 0)
    //        {
    //            return false;
    //        }

    //        return true;
    //    }
    //    #endregion

    //    #region Properties
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="value"></param>
    //    /// <returns></returns>
    //    public static BigDecimal mantissa(BigDecimal value)
    //    {
    //        int _exponent = exponent(value);
    //        if (_exponent == 0)
    //        {
    //            return value;
    //        }

    //        return value.movePointLeft(_exponent);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="value"></param>
    //    /// <returns></returns>
    //    public static int exponent(BigDecimal value)
    //    {
    //        return value.precision() - value.scale() - 1;
    //    }
    //    /// <summary>
    //    /// Returns the fraction part of the value.
    //    /// </summary>
    //    /// <param name="x">Value</param>
    //    /// <returns>Returns</returns>
    //    public static BigDecimal fraction(BigDecimal x)
    //    {
    //        BigDecimal _exponent = integral(x);
    //        return x.subtract(_exponent);
    //    }
    //    /// <summary>
    //    /// Returns the integral of the value.
    //    /// </summary>
    //    /// <param name="x">Value</param>
    //    /// <returns>Result</returns>
    //    public static BigDecimal integral(BigDecimal x)
    //    {
    //        return x.setScale(0, BigDecimal.ROUND_DOWN);
    //    }
    //    /// <summary>
    //    /// Returns the one diveded by the value.
    //    /// </summary>
    //    /// <param name="x">Value</param>
    //    /// <returns>Result</returns>
    //    public static BigDecimal reciprocal(BigDecimal x)
    //    {
    //        return BigDecimal.ONE.divide(x);
    //    }
    //    #endregion

    //    #region Sqrt
    //    /// <summary>
    //    /// Returns the sqaure root of the value.
    //    /// </summary>
    //    /// <param name="x">Value</param>
    //    /// <returns>Result</returns>
    //    public static BigDecimal sqrt(BigDecimal x)
    //    {
    //        // Check that x >= 0.
    //        if (x.signum() < 0)
    //        {
    //            throw new ArithmeticException("x < 0");
    //        }

    //        // n = x*(10^(2*SCALE))
    //        BigInteger n = x.movePointRight(SCALE << 1).toBigInteger();

    //        // The first approximation is the upper half of n.
    //        int bits = (n.bitLength() + 1) >> 1;
    //        BigInteger ix = n.shiftRight(bits);
    //        BigInteger ixPrev;

    //        // Loop until the approximations converge
    //        // (two successive approximations are equal after rounding).
    //        do
    //        {
    //            ixPrev = ix;

    //            // x = (x + n/x)/2
    //            ix = ix.add(n.divide(ix)).shiftRight(1);

    //            //Thread.yield();
    //        } while (ix.compareTo(ixPrev) != 0);

    //        return new BigDecimal(ix, SCALE);
    //    }
    //    #endregion

    //    #region Log
    //    /// <summary>
    //    /// Returns the natural logarithm of the value.
    //    /// </summary>
    //    /// <param name="x">Value</param>
    //    /// <returns>Result</returns>
    //    public static BigDecimal log(BigDecimal x)
    //    {
    //        // Check that x > 0.
    //        if (x.signum() <= 0)
    //        {
    //            throw new ArgumentException("x <= 0");
    //        }

    //        // The number of digits to the left of the decimal point.
    //        int magnitude = x.toString().Length - x.scale() - 1;

    //        if (magnitude < 3)
    //        {
    //            return logNewton(x);
    //        }

    //        // Compute magnitude*ln(x^(1/magnitude)).
    //        else
    //        {

    //            // x^(1/magnitude)
    //            BigDecimal root = intRoot(x, magnitude);

    //            // ln(x^(1/magnitude))
    //            BigDecimal lnRoot = logNewton(root);

    //            // magnitude*ln(x^(1/magnitude))
    //            return BigDecimal.valueOf(magnitude).multiply(lnRoot)
    //                        .setScale(SCALE, BigDecimal.ROUND_HALF_EVEN);
    //        }
    //    }
    //    /// <summary>
    //    /// Returns the integer root of the value.
    //    /// </summary>
    //    /// <param name="x">Value</param>
    //    /// <param name="index">Index</param>
    //    /// <returns>Results</returns>
    //    private static BigDecimal intRoot(BigDecimal x, long index)
    //    {
    //        // Check that x >= 0.
    //        if (x.signum() < 0)
    //        {
    //            throw new ArgumentException("x < 0");
    //        }

    //        int sp1 = SCALE + 1;
    //        BigDecimal n = x;
    //        BigDecimal i = BigDecimal.valueOf(index);
    //        BigDecimal im1 = BigDecimal.valueOf(index - 1);
    //        BigDecimal tolerance = BigDecimal.valueOf(5)
    //                                            .movePointLeft(sp1);
    //        BigDecimal xPrev;

    //        // The initial approximation is x/index.
    //        x = x.divide(i, SCALE, BigDecimal.ROUND_HALF_EVEN);

    //        // Loop until the approximations converge
    //        // (two successive approximations are equal after rounding).
    //        do
    //        {
    //            // x^(index-1)
    //            BigDecimal xToIm1 = intPower(x, index - 1, sp1);

    //            // x^index
    //            BigDecimal xToI =
    //                    x.multiply(xToIm1)
    //                        .setScale(sp1, BigDecimal.ROUND_HALF_EVEN);

    //            // n + (index-1)*(x^index)
    //            BigDecimal numerator =
    //                    n.add(im1.multiply(xToI))
    //                        .setScale(sp1, BigDecimal.ROUND_HALF_EVEN);

    //            // (index*(x^(index-1))
    //            BigDecimal denominator =
    //                    i.multiply(xToIm1)
    //                        .setScale(sp1, BigDecimal.ROUND_HALF_EVEN);

    //            // x = (n + (index-1)*(x^index)) / (index*(x^(index-1)))
    //            xPrev = x;
    //            x = numerator
    //                    .divide(denominator, sp1, BigDecimal.ROUND_DOWN);

    //            //Thread.yield();
    //        } while (x.subtract(xPrev).abs().compareTo(tolerance) > 0);

    //        return x;
    //    }
    //    /// <summary>
    //    /// Returns the Newton product.
    //    /// </summary>
    //    /// <param name="x">Value</param>
    //    /// <returns>Result</returns>
    //    private static BigDecimal logNewton(BigDecimal x)
    //    {
    //        int sp1 = SCALE + 1;
    //        BigDecimal n = x;
    //        BigDecimal term;

    //        // Convergence tolerance = 5*(10^-(scale+1))
    //        BigDecimal tolerance = BigDecimal.valueOf(5)
    //                                            .movePointLeft(sp1);

    //        // Loop until the approximations converge
    //        // (two successive approximations are within the tolerance).
    //        do
    //        {
    //            // e^x
    //            BigDecimal eToX = exp(x);

    //            // (e^x - n)/e^x
    //            term = eToX.subtract(n)
    //                        .divide(eToX, sp1, BigDecimal.ROUND_DOWN);

    //            // x - (e^x - n)/e^x
    //            x = x.subtract(term);

    //            //Thread.yield();
    //        } while (term.compareTo(tolerance) > 0);

    //        return x.setScale(SCALE, BigDecimal.ROUND_HALF_EVEN);
    //    }
    //    #endregion

    //    #region Exp
    //    /// <summary>
    //    /// Returns the exponent raised to the power.
    //    /// </summary>
    //    /// <param name="x">Value</param>
    //    /// <returns>Returns</returns>
    //    public static BigDecimal exp(BigDecimal x)
    //    {
    //        // e^0 = 1
    //        if (x.signum() == 0)
    //        {
    //            return BigDecimal.valueOf(1);
    //        }

    //        // If x is negative, return 1/(e^-x).
    //        else if (x.signum() == -1)
    //        {
    //            return BigDecimal.valueOf(1)
    //                        .divide(exp(x.negate()), SCALE,
    //                                BigDecimal.ROUND_HALF_EVEN);
    //        }

    //        // Compute the whole part of x.
    //        BigDecimal xWhole = x.setScale(0, BigDecimal.ROUND_DOWN);

    //        // If there isn't a whole part, compute and return e^x.
    //        if (xWhole.signum() == 0)
    //        {
    //            return expTaylor(x);
    //        }

    //        // Compute the fraction part of x.
    //        BigDecimal xFraction = x.subtract(xWhole);

    //        // z = 1 + fraction/whole
    //        BigDecimal z = BigDecimal.valueOf(1)
    //                            .add(xFraction.divide(
    //                                    xWhole, SCALE,
    //                                    BigDecimal.ROUND_HALF_EVEN));

    //        // t = e^z
    //        BigDecimal t = expTaylor(z);

    //        BigDecimal maxLong = BigDecimal.valueOf(long.MaxValue);
    //        BigDecimal result = BigDecimal.valueOf(1);

    //        // Compute and return t^whole using intPower().
    //        // If whole > Long.MAX_VALUE, then first compute products
    //        // of e^Long.MAX_VALUE.
    //        while (xWhole.compareTo(maxLong) >= 0)
    //        {
    //            result = result.multiply(
    //                                intPower(t, long.MaxValue, SCALE))
    //                        .setScale(SCALE, BigDecimal.ROUND_HALF_EVEN);
    //            xWhole = xWhole.subtract(maxLong);

    //            //Thread.yield();
    //        }
    //        return result.multiply(intPower(t, xWhole.longValue(), SCALE))
    //                        .setScale(SCALE, BigDecimal.ROUND_HALF_EVEN);
    //    }
    //    /// <summary>
    //    /// Returns the Taylor series product.
    //    /// </summary>
    //    /// <param name="x">Value</param>
    //    /// <returns>Returns</returns>
    //    private static BigDecimal expTaylor(BigDecimal x)
    //    {
    //        BigDecimal factorial = BigDecimal.valueOf(1);
    //        BigDecimal xPower = x;
    //        BigDecimal sumPrev;

    //        // 1 + x
    //        BigDecimal sum = x.add(BigDecimal.valueOf(1));

    //        // Loop until the sums converge
    //        // (two successive sums are equal after rounding).
    //        int i = 2;
    //        do
    //        {
    //            // x^i
    //            xPower = xPower.multiply(x).setScale(SCALE, BigDecimal.ROUND_HALF_EVEN);

    //            // i!
    //            factorial = factorial.multiply(BigDecimal.valueOf(i));

    //            // x^i/i!
    //            BigDecimal term = xPower
    //                                .divide(factorial, SCALE,
    //                                        BigDecimal.ROUND_HALF_EVEN);

    //            // sum = sum + x^i/i!
    //            sumPrev = sum;
    //            sum = sum.add(term);

    //            ++i;
    //            //Thread.yield();
    //        } while (sum.compareTo(sumPrev) != 0);

    //        return sum;
    //    }
    //    #endregion

    //    #region Pow
    //    /// <summary>
    //    /// Returns the number raised to the power.
    //    /// </summary>
    //    /// <param name="a">Value</param>
    //    /// <param name="n">Power</param>
    //    /// <returns>Returns</returns>
    //    public static BigDecimal pow(BigDecimal a, BigDecimal n)
    //    {
    //        BigDecimal _exponent = integral(n);
    //        BigDecimal _mantissa = fraction(n);

    //        // A^B = { B = X + x } = A^X * A^x
    //        // right part of equation
    //        BigDecimal right = exp(log(a).multiply(_mantissa));

    //        // left part of equation
    //        BigDecimal tmp = BigDecimal.ONE;
    //        while (_exponent.abs().compareTo(BigDecimal.valueOf(MAXPOW)) == 1)
    //        {
    //            int diff = _exponent.signum() > 0 ? MAXPOW : -MAXPOW;
    //            tmp = tmp.multiply(a.pow(diff));
    //            _exponent = _exponent.subtract(BigDecimal.valueOf(diff));
    //        }
    //        BigDecimal left = tmp.multiply(a.pow(_exponent.intValue()));

    //        // result
    //        BigDecimal result = left.multiply(right);
    //        return result;
    //    }
    //    /// <summary>
    //    /// Returns the number raised to the integer power.
    //    /// </summary>
    //    /// <param name="x">Value</param>
    //    /// <param name="exponent">Exponent</param>
    //    /// <param name="scale">Scale</param>
    //    /// <returns>Returns</returns>
    //    private static BigDecimal intPower(BigDecimal x, long exponent, int scale)
    //    {
    //        // If the exponent is negative, compute 1/(x^-exponent).
    //        if (exponent < 0)
    //        {
    //            return BigDecimal.valueOf(1)
    //                        .divide(intPower(x, -exponent, scale), scale,
    //                                BigDecimal.ROUND_HALF_EVEN);
    //        }

    //        BigDecimal power = BigDecimal.valueOf(1);

    //        // Loop to compute value^exponent.
    //        while (exponent > 0)
    //        {

    //            // Is the rightmost bit a 1?
    //            if ((exponent & 1) == 1)
    //            {
    //                power = power.multiply(x)
    //                          .setScale(scale, BigDecimal.ROUND_HALF_EVEN);
    //            }

    //            // Square x and shift exponent 1 bit to the right.
    //            x = x.multiply(x)
    //                    .setScale(scale, BigDecimal.ROUND_HALF_EVEN);
    //            exponent >>= 1;

    //            //Thread.yield();
    //        }

    //        return power;
    //    }
    //    #endregion
    //}
}
