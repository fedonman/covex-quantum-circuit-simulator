namespace CoveX.ClassicalUtilities
{
    /// <summary>
    /// A static class containing various math functions that are not
    /// contained in the .NET Core 5
    /// </summary>
    public static class ClassicalMath
    {
        /// <summary>
        /// Tests to see if TestValue is a (integer) power of PowerOf.
        /// </summary>
        /// <example>If PowerOf = 2, then this function returns true if TestValue is
        /// 1, 2, 4, 8, 16,...</example>
        /// <param name="PowerOf">Number to raise to integer powers.</param>
        /// <param name="TestValue">Value to test to see if it is an integer power</param>
        /// <returns>True if TestValue is a power of PowerOf.</returns>
        public static bool IsPowerOf(int PowerOf, int TestValue)
        {
            int iCurPower = 0;
            long iTotal = (long)System.Math.Pow(PowerOf, iCurPower);

            //Make sure the number of rows is a power of two
            while (iTotal <= TestValue)
            {
                //found an exact match?
                if (iTotal == TestValue)
                    return true;

                //prep for next iteration
                iCurPower++;
                iTotal = (long)System.Math.Pow(PowerOf, iCurPower);
            }

            //made it to here then there wasn't an exact match, so it isn't a power of
            return false;
        }

    }
}
