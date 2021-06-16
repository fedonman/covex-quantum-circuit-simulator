using CoveX.Base;
using System;
using System.Collections.Generic;


namespace CoveX.LocalSimulation
{
    /// <summary>
    /// This class contains everything to factor a number using Shor's algorithm.
    /// </summary>
    public class ShorsAlgorithm
    {

        /// <summary>
        /// The random number source needed for this algorithm
        /// </summary>
        /// <remarks>
        /// This is a static member so that one value is shared across all instances. This prevents
        /// the problem of creating a bunch instances that end up having the same random numbers-
        /// the result being far from even pseudo-random.
        /// </remarks>
        private static Random cRandomSource = new Random();


        /// <summary>
        /// The default constructor.
        /// </summary>
        public ShorsAlgorithm()
        {
        }


        /// <summary>
        /// Call this to factor numbers utilizing quantum resources.
        /// </summary>
        /// <param name="NumberToFactor">The number to factor. Typically 
        /// referred to as N.</param>
        /// <param name="Factor1">The first factor. Typically referred to
        /// as p.</param>
        /// <param name="Factor2">The second factor. Typically referred
        /// to as q.</param>
        /// <returns>False if the number could not be factored. In this 
        /// case the two output variables will be 0.</returns>
        public bool Factor(int NumberToFactor, out int Factor1, out int Factor2)
        {
            List<int> listNotTriedM = new List<int>(NumberToFactor - 2);
            int iCurM = 0;
            int iCurMIndex = 0;
            int iCurPeriod = 0;
            int iGCD = 0;               //greatest common divisor
            double dActualDivision = 0.0;

            //init the factors to 0 in case it doesn't work.
            Factor1 = 0;
            Factor2 = 0;

            //don't want to select duplicate values for m, so create a list
            //of untried m, and select from there (then remove it). construct all possible
            //values of m here: 1 < m < NumberToFactor
            for (int iCurPossibleM = 2; iCurPossibleM < NumberToFactor; iCurPossibleM++)
                listNotTriedM.Add(iCurPossibleM);

            //while no factor is found and there are still values of m to try, try a value of m
            while ((Factor1 == 0) && (Factor2 == 0) && (listNotTriedM.Count >= 1))
            {
                //select a random value for m (step 1)
                iCurMIndex = cRandomSource.Next(listNotTriedM.Count);
                iCurM = listNotTriedM[iCurMIndex];
                listNotTriedM.RemoveAt(iCurMIndex);       //remove it so it isn't tried again.

                //lucky and randomly picked a factor?
                iGCD = Utilities.EuclideanGCD(iCurM, NumberToFactor);
                if (iGCD != 1)
                {
                    Factor1 = iGCD;
                    Factor2 = NumberToFactor / Factor1;
                    return true;
                }

                //find the period (step 2)
                iCurPeriod = FindPeriod(NumberToFactor, iCurM);

                //need to obtain an even period, so start over if not even (step 3)
                if ((iCurPeriod % 2) != 0)
                    continue;

                //need to see if we can continue since m^(p/2) + 1 is not divible by N (step 4).
                //integer division of iCurPeriod is ok since it is even. if the division is a whole
                //number then it is divisible.
                dActualDivision = (Math.Pow(iCurM, (iCurPeriod / 2)) + 1) / ((double)NumberToFactor);
                if (dActualDivision == Math.Truncate(dActualDivision))
                    continue;

                //one or both of the factors are found
                Factor1 = Utilities.EuclideanGCD(((int)Math.Pow(iCurM, (iCurPeriod / 2))) - 1, NumberToFactor);
                if (Factor1 == 1)       //-1 didn't work, try +1
                    Factor1 = Utilities.EuclideanGCD(((int)Math.Pow(iCurM, (iCurPeriod / 2))) + 1, NumberToFactor);

                Factor2 = NumberToFactor / Factor1;

                if ((Factor1 == 1) || (Factor2 == 1))        //make sure it worked right
                    throw new ImplementationException(string.Format("Factoring made it through, but the numbers were not factors. m = {0}, N = {1}, P = {2}, p = {3}, q = {4}", iCurM, NumberToFactor, iCurPeriod, Factor1, Factor2));
                return true;
            }          //end of iteration through attempts

            //made it to here then tried all values of m and no factor found.
            return false;
        }


        /// <summary>
        /// Find the period of iCurM (m) and the NumberToFactor (N)
        /// </summary>
        /// <param name="NumberToFactor">The number being factored</param>
        /// <param name="iCurM">The current value of m</param>
        /// <returns>The period found</returns>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public int FindPeriod(int NumberToFactor, int iCurM)
        {
            if (NumberToFactor < 16)
                return FindPeriodFor15(NumberToFactor, iCurM);
            else
                throw new NotImplementedException("Factoring numbers other than 15 not yet implemented");
        }


        /// <summary>
        /// Specific case of finding the period for 15.
        /// </summary>
        /// <param name="NumberToFactor">The number to factor, 15</param>
        /// <param name="iCurM">The value of m to find the period over</param>
        /// <returns>The period.</returns>
        /// <remarks>Some of the implementation in this is a little more general
        /// so that it can more easily be converted to the general case</remarks>
        protected int FindPeriodFor15(int NumberToFactor, int iCurM)
        {
            int iNumBitsToExpressN = 4;
            IQuantumRegister cEntireRegister = new QuantumRegister(3 * iNumBitsToExpressN);
            IQuantumRegister cReg1 = cEntireRegister.SliceTo(2 * iNumBitsToExpressN);
            IQuantumRegister cReg2 = cEntireRegister.SliceFrom(2 * iNumBitsToExpressN);

            if (NumberToFactor != 15)
                throw new ArgumentException("FindPeriodFor15() can only work on NumberToFactor == 15");

            //put Reg1 in superposition to express 0 to (N^2 - 1)
            cReg1.OperationHadamardAll();

            //Construct Uf, and apply it.

            //Measure Reg2 to collapse it and part of Reg1
            cReg2.Measure();

            //Next perform the quantum fourier transform on Reg1
            cReg1.OperationInverseQuantumFourierTransform();

            //the period is now the result of measuring Reg1, so return it.
            return ((int)(cReg1.Measure()).ToUnsignedInt32());
        }




    }
}
