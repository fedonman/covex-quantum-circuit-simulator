using System;

namespace CoveX.Base
{
    /// <summary>
    /// Contains various utility functions for Cove that are not specific
    /// to a particular implementation.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Return the greatest common divisor (GCD) of X and Y via
        /// the Euclidean algorithm.
        /// </summary>
        /// <param name="X">First number</param>
        /// <param name="Y">Second number</param>
        /// <returns>The greatest common divisor of X and Y.</returns>
        public static int EuclideanGCD(int X, int Y)
        {
            if (Y == 0)
            {
                return X;
            }
            else
            {
                return EuclideanGCD(Y, (X % Y));
            }
        }


        /// <summary>
        /// Gets the number of bits needed to express the number as an unsigned long.
        /// </summary>
        /// <param name="Number">The number to see how many bits are needed to
        /// express it.</param>
        /// <returns>Number of bits needed to express the number as an 
        /// unsigned long.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if Number
        /// is less than zero.</exception>
        public static int BitsToExpress(long Number)
        {
            int iRetVal = 0;

            if (Number < 0)
                throw new ArgumentOutOfRangeException("Only positive numbers can be passed");

            //I know there is a better way to do this, but this will work for now.
            //just go increase powers of 2 until it is bigger than the number. once it
            //is bigger then we've found the value.
            while (Math.Pow(2, iRetVal) <= Number)
            {
                iRetVal++;
            }

            return iRetVal;
        }

    }
}
