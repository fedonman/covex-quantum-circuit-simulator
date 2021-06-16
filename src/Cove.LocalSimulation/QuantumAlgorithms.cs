using CoveX.Base;
using System;
using System.Collections.Generic;


namespace CoveX.LocalSimulation
{
    /// <summary>
    /// This class provides functions to perform common quantum algorithms. This allows
    /// users to perform some typical quantum algorithms without being concerned about the
    /// implementation details.
    /// </summary>
    public class QuantumAlgorithms : IQuantumAlgorithms
    {
        /// <summary>
        /// Factor a number using Shor's algorithm.
        /// </summary>
        /// <param name="NumberToFactor">The number to factor</param>
        /// <param name="Factor1">The first factor of the number</param>
        /// <param name="Factor2">The second factor of the number</param>
        /// <returns>False if the number could not be factored. In this 
        /// case the two output variables will be 0.</returns>
        public bool Factor(int NumberToFactor, out int Factor1, out int Factor2)
        {
            ShorsAlgorithm cFactor = new ShorsAlgorithm();

            return cFactor.Factor(NumberToFactor, out Factor1, out Factor2);
        }

        #region "Algorithms needed to perform factoring"

        /// <summary>
        /// Return the operations that perform Sum.
        /// </summary>
        /// <param name="CarryIndex">The index of the carry qubit. This remains unchanged
        /// after the operations are applied.</param>
        /// <param name="XIndex">The index of the X qubit. This remains unchanged
        /// after the operations are applied.</param>
        /// <param name="YIndex">The index of the Y qubit. CarryIndex + XIndex + YIndex 
        /// (mod 2 addition)</param>
        /// <returns>The quantum operations that perform sum over the specified qubits.</returns>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes
        /// passed in are the same. All indexes must be unique.</exception>
        public List<IQuantumOperation> Sum(int CarryIndex, int XIndex, int YIndex)
        {
            List<IQuantumOperation> listRetVal = new List<IQuantumOperation>();

            //make sure duplicates are not specified
            if ((CarryIndex == XIndex) || (CarryIndex == YIndex) || (XIndex == YIndex))
                throw new DuplicateIndexesException("Unique indexes must be specified.");

            //build up the operations to apply.
            listRetVal.Add(new OperationCNot(CarryIndex, YIndex));
            listRetVal.Add(new OperationCNot(XIndex, YIndex));

            return listRetVal;
        }


        /// <summary>
        /// Return the operations to perform the carry gate.
        /// </summary>
        /// <param name="CarryIndex">The index of the carry qubit. Remains unchanged after
        /// the operations are applied.</param>
        /// <param name="XIndex">The index of the X qubit. Remains unchanged after
        /// the operations are applied.</param>
        /// <param name="YIndex">The index of the Y qubit. On output this will be a + b (mod 2
        /// addition)</param>
        /// <param name="AncilliaIndex">The index of the ancillia (scratch) qubit. On output
        /// this will be (CarryIndex)(XIndex) + (XIndex)(CarryIndex) + (YIndex)(CarryIndex)
        /// (mod 2 addition)</param>
        /// <returns>The operations to perform carry.</returns>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes
        /// are duplicates.</exception>
        public List<IQuantumOperation> Carry(int CarryIndex, int XIndex, int YIndex, int AncilliaIndex)
        {
            List<IQuantumOperation> listRetVal = new List<IQuantumOperation>();

            //make sure all the indexes are unique
            if ((CarryIndex == XIndex) || (CarryIndex == YIndex) || (CarryIndex == AncilliaIndex)
            || (XIndex == YIndex) || (XIndex == AncilliaIndex) || (YIndex == AncilliaIndex))
                throw new DuplicateIndexesException("All indexes passed must be unique.");

            //build the operations to return
            listRetVal.Add(new OperationToffoli(XIndex, YIndex, AncilliaIndex));
            listRetVal.Add(new OperationCNot(XIndex, YIndex));
            listRetVal.Add(new OperationToffoli(CarryIndex, YIndex, AncilliaIndex));

            return listRetVal;
        }


        /// <summary>
        /// Return the operations to perform the inverse carry.
        /// </summary>
        /// <param name="CarryIndex">The index of the carry qubit. Remains unchanged once
        /// the operations are applied.</param>
        /// <param name="XIndex">The index of the X qubit. Remains unchanged once the 
        /// operations are applied.</param>
        /// <param name="YIndex">The index of the Y qubit. After the operations are
        /// applied this will be X + Y (mod 2 addition)</param>
        /// <param name="CarryPrimeIndex">The index of the carry prime qubit. Will be 
        /// x(x + y) + yc + c' on output.</param>
        /// <returns>The operations to apply the carry inverse.</returns>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes
        /// specified are duplicates.</exception>
        public List<IQuantumOperation> CarryInverse(int CarryIndex, int XIndex, int YIndex,
        int CarryPrimeIndex)
        {
            List<IQuantumOperation> listRetVal = Carry(CarryIndex, XIndex, YIndex, CarryPrimeIndex);

            //The carry inverse is just the carry in reverse.
            listRetVal.Reverse();

            return listRetVal;
        }


        /// <summary>
        /// Return the operations needed to perform Add over two registers of equal size.
        /// </summary>
        /// <param name="XIndexes">The indexes of the X register. These remain unchanged
        /// once the operations are applied.</param>
        /// <param name="YIndexes">The indexes of the Y register. These contain the
        /// result after the operations are applied, along with the last ancillia qubit</param>
        /// <param name="AncilliaIndexes">The indexes of the ancillia qubits, which should
        /// be initialized to |0>. There should be one more ancillia qubit than there are
        /// X or Y qubits. The result will be in the YIndexes and the last ancillia index.</param>
        /// <returns>The operations to apply add n.</returns>
        /// <exception cref="ArgumentNullException">Thrown if any of the indexes passed
        /// in are null.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes specified
        /// are duplicates. All indexes must be unique.</exception>
        /// <exception cref="SizeMismatchException">Thrown if XIndexes and YIndexes are not of
        /// equal length, or if AncilliaIndexes is not 1 larger than XIndexes and 
        /// YIndexes.</exception>
        public List<IQuantumOperation> AddN(int[] XIndexes, int[] YIndexes, int[] AncilliaIndexes)
        {
            int[] iaResultIndexes = null;

            //just wrap the one that returns the result indexes and throw those away.
            return this.AddN(XIndexes, YIndexes, AncilliaIndexes, out iaResultIndexes);
        }


        /// <summary>
        /// Return the operations needed to perform Add over two registers of equal size.
        /// </summary>
        /// <param name="XIndexes">The indexes of the X register. These remain unchanged
        /// once the operations are applied.</param>
        /// <param name="YIndexes">The indexes of the Y register. These contain the
        /// result after the operations are applied, along with the last ancillia qubit</param>
        /// <param name="AncilliaIndexes">The indexes of the ancillia qubits, which should
        /// be initialized to |0>. There should be one more ancillia qubit than there are
        /// X or Y qubits. The result will be in the YIndexes and the last ancillia index.</param>
        /// <param name="ResultIndexes">The YIndexes and last ancillia index contain the result,
        /// but this parameter will be populated with them explicitly for ease of use.</param>
        /// <returns>The operations to apply add n.</returns>
        /// <exception cref="ArgumentNullException">Thrown if any of the indexes passed
        /// in are null.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes specified
        /// are duplicates. All indexes must be unique.</exception>
        /// <exception cref="SizeMismatchException">Thrown if XIndexes and YIndexes are not of
        /// equal length, or if AncilliaIndexes is not 1 larger than XIndexes and 
        /// YIndexes.</exception>
        public List<IQuantumOperation> AddN(int[] XIndexes, int[] YIndexes, int[] AncilliaIndexes,
        out int[] ResultIndexes)
        {
            List<IQuantumOperation> listRetVal = new List<IQuantumOperation>();
            Dictionary<int, bool> dictUsedIndexes = new Dictionary<int, bool>();
            List<int> listResultIndexes = new List<int>();

            //first verify nulls and sizes
            if ((XIndexes == null) || (YIndexes == null) || (AncilliaIndexes == null))
                throw new ArgumentNullException("Cannot pass null indexes");
            if (XIndexes.Length != YIndexes.Length)
                throw new SizeMismatchException("XIndexes and YIndexes must contain an equal number of elements.");
            if ((XIndexes.Length + 1) != AncilliaIndexes.Length)
                throw new SizeMismatchException("The AncilliaIndexes must have one more index than the XIndexes and YIndexes.");

            //next make sure there are no duplicate indexes by going through them 
            //and keeping track of what has been used in a dictionary. (Also populate the
            //result indexes
            foreach (int iCurIndex in XIndexes)
            {
                if (dictUsedIndexes.ContainsKey(iCurIndex) == true)
                    throw new DuplicateIndexesException(string.Format("The index {0} is specified more than once. All indexes must be unique.", iCurIndex));
                else
                    dictUsedIndexes[iCurIndex] = true;
            }
            foreach (int iCurIndex in YIndexes)
            {
                if (dictUsedIndexes.ContainsKey(iCurIndex) == true)
                    throw new DuplicateIndexesException(string.Format("The index {0} is specified more than once. All indexes must be unique.", iCurIndex));
                else
                    dictUsedIndexes[iCurIndex] = true;
                listResultIndexes.Add(iCurIndex);          //keep track of Y indexes, as they are part of the result.
            }
            foreach (int iCurIndex in AncilliaIndexes)
            {
                if (dictUsedIndexes.ContainsKey(iCurIndex) == true)
                    throw new DuplicateIndexesException(string.Format("The index {0} is specified more than once. All indexes must be unique.", iCurIndex));
                else
                    dictUsedIndexes[iCurIndex] = true;
            }            //end of check for duplicate indexes

            //populate the out indexes
            listResultIndexes.Add(AncilliaIndexes[AncilliaIndexes.Length - 1]);
            ResultIndexes = listResultIndexes.ToArray();

            //now go through and construct the necessary gates to the half way point.
            for (int iCurIndex = 0; iCurIndex < XIndexes.Length; iCurIndex++)
            {
                listRetVal.AddRange(this.Carry(AncilliaIndexes[iCurIndex], XIndexes[iCurIndex],
                    YIndexes[iCurIndex], AncilliaIndexes[iCurIndex + 1]));
            }                //end for iCurIndex, to the half way point

            //CNot and Sum after the half way point
            listRetVal.Add(new OperationCNot(XIndexes[XIndexes.Length - 1],
                YIndexes[YIndexes.Length - 1]));
            listRetVal.AddRange(this.Sum(AncilliaIndexes[AncilliaIndexes.Length - 2],
                XIndexes[XIndexes.Length - 1], YIndexes[YIndexes.Length - 1]));

            //now carry inverses and sums to complete the gate
            //NOTE: the look starts at XIndexes.Length - 2, as there is one less carry inverse 
            //      gate than there is carry. If looking at the circuit diagram the bottom 
            //      (highest indexes) are Carry CNot Sum, then Carry inverse starts at the 
            //      next highest (lowest indexes)
            for (int iCurIndex = (XIndexes.Length - 2); iCurIndex >= 0; iCurIndex--)
            {
                listRetVal.AddRange(this.CarryInverse(AncilliaIndexes[iCurIndex], XIndexes[iCurIndex],
                    YIndexes[iCurIndex], AncilliaIndexes[iCurIndex + 1]));
                listRetVal.AddRange(this.Sum(AncilliaIndexes[iCurIndex], XIndexes[iCurIndex],
                    YIndexes[iCurIndex]));
            }

            return listRetVal;
        }


        /// <summary>
        /// Return the operations needed to perform Add Inverse over two registers of equal size.
        /// Add inverse is subtraction.
        /// </summary>
        /// <param name="XIndexes">The indexes of the X register. These remain unchanged
        /// once the operations are applied.</param>
        /// <param name="YIndexes">The indexes of the Y register. These contain the
        /// result after the operations are applied, along with the last ancillia qubit</param>
        /// <param name="AncilliaIndexes">The indexes of the ancillia qubits, which should
        /// be initialized to |0>. There should be one more ancillia qubit than there are
        /// X or Y qubits. The result will be in the YIndexes and the last ancillia index.</param>
        /// <returns>The operations to apply add n.</returns>
        /// <exception cref="ArgumentNullException">Thrown if any of the indexes passed
        /// in are null.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes specified
        /// are duplicates. All indexes must be unique.</exception>
        /// <exception cref="SizeMismatchException">Thrown if XIndexes and YIndexes are not of
        /// equal length, or if AncilliaIndexes is not 1 larger than XIndexes and 
        /// YIndexes.</exception>
        public List<IQuantumOperation> AddNInverse(int[] XIndexes, int[] YIndexes, int[] AncilliaIndexes)
        {
            List<IQuantumOperation> listRetVal = null;

            //the inverse gate is just the operations in reverse, let any exceptions perculate up.
            listRetVal = this.AddN(XIndexes, YIndexes, AncilliaIndexes);
            listRetVal.Reverse();
            return listRetVal;
        }


        /// <summary>
        /// Return the operations needed to perform Add Inverse over two registers of equal size.
        /// Add inverse is subtraction.
        /// </summary>
        /// <param name="XIndexes">The indexes of the X register. These remain unchanged
        /// once the operations are applied.</param>
        /// <param name="YIndexes">The indexes of the Y register. These contain the
        /// result after the operations are applied, along with the last ancillia qubit</param>
        /// <param name="AncilliaIndexes">The indexes of the ancillia qubits, which should
        /// be initialized to |0>. There should be one more ancillia qubit than there are
        /// X or Y qubits. The result will be in the YIndexes and the last ancillia index.</param>
        /// <param name="ResultIndexes">The YIndexes index contain the result,
        /// but this parameter will be populated with them explicitly for ease of use.</param>
        /// <param name="CarryIndex">The carry index, the element at that location will be 1 if (Y - X) 
        /// less than 0, else the value at that location will be 0.</param>
        /// <returns>The operations to apply add n.</returns>
        /// <exception cref="ArgumentNullException">Thrown if any of the indexes passed
        /// in are null.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes specified
        /// are duplicates. All indexes must be unique.</exception>
        /// <exception cref="SizeMismatchException">Thrown if XIndexes and YIndexes are not of
        /// equal length, or if AncilliaIndexes is not 1 larger than XIndexes and 
        /// YIndexes.</exception>
        public List<IQuantumOperation> AddNInverse(int[] XIndexes, int[] YIndexes, int[] AncilliaIndexes,
        out int[] ResultIndexes, out int CarryIndex)
        {
            List<IQuantumOperation> listRetVal = null;

            //the inverse gate is just the operations in reverse, let any exceptions perculate up.
            listRetVal = this.AddN(XIndexes, YIndexes, AncilliaIndexes, out ResultIndexes);
            listRetVal.Reverse();

            //however, the results are actually all but 1 in the results and the final one is the carry
            //result. populate the return objects for add inverse accordingly.
            CarryIndex = ResultIndexes[ResultIndexes.Length - 1];

            return listRetVal;
        }


        /// <summary>
        /// Swap the first indexes and the second indexes. Each qubit in element x of 
        /// FirstSwapIndexes will be swapped with element x of SecondSwapIndexes. Example:
        /// A register is ordered 0, 1, 2, 3. If FirstIndexes = 0, 2 and SecondIndexes = 3, 1
        /// then the resulting order will be 3, 1, 2, 0.
        /// </summary>
        /// <param name="FirstSwapIndexes">First set of indexes to swap.</param>
        /// <param name="SecondSwapIndexes">Second set of indexes to swap.</param>
        /// <returns>The operations that perform this swap.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the arrays passed in are null.</exception>
        /// <exception cref="ArgumentException">Thrown if the indexes passed in are not
        /// of equal length.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes
        /// are duplicated. All indexes in and between the two parameters must be unique.</exception>
        public List<IQuantumOperation> SwapIndexes(int[] FirstSwapIndexes, int[] SecondSwapIndexes)
        {
            List<IQuantumOperation> listRetVal = new List<IQuantumOperation>();
            Dictionary<int, bool> dictUsedIndexes = new Dictionary<int, bool>();

            //verify the input
            if (FirstSwapIndexes == null)
                throw new ArgumentNullException("FirstSwapIndexes cannot be null");
            if (SecondSwapIndexes == null)
                throw new ArgumentNullException("SecondSwapIndexes cannot be null");
            if (FirstSwapIndexes.Length != SecondSwapIndexes.Length)
                throw new ArgumentException("FirstSwapIndexes and SecondSwapIndexes must be arrays of equal length.");

            //make sure indexes aren't duplicated.
            foreach (int iCurIndex in FirstSwapIndexes)
            {
                if (dictUsedIndexes.ContainsKey(iCurIndex) == true)
                    throw new DuplicateIndexesException(string.Format("All indexes in the parameters must be unique. {0} is used more than once.", iCurIndex));
                else
                    dictUsedIndexes[iCurIndex] = true;
            }

            foreach (int iCurIndex in SecondSwapIndexes)
            {
                if (dictUsedIndexes.ContainsKey(iCurIndex) == true)
                    throw new DuplicateIndexesException(string.Format("All indexes in the parameters must be unique. {0} is used more than once.", iCurIndex));
                else
                    dictUsedIndexes[iCurIndex] = true;
            }

            //now just build up the operations to return. just drops through and returns
            //an empty list if the parameters are empty arrays.
            for (int iCurIndex = 0; iCurIndex < FirstSwapIndexes.Length; iCurIndex++)
            {
                listRetVal.Add(new OperationSwap(FirstSwapIndexes[iCurIndex], SecondSwapIndexes[iCurIndex]));
            }

            return listRetVal;
        }


        /// <summary>
        /// Returns the operations to perform the negated CNot. The normal CNot operation flips
        /// the target qubit when the control is 1, otherwise no change is made. This (the negated)
        /// flips the target when the control is 0 instead of 1.
        /// </summary>
        /// <param name="ControlIndex">Index of the control qubit.</param>
        /// <param name="TargetIndex">Index of the target qubit.</param>
        /// <returns>The operations to performt he negated CNot.</returns>
        /// <exception cref="DuplicateIndexesException">Thrown if the control
        /// and target indexes are the same.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if either parameter
        /// is less than 0.</exception>
        public List<IQuantumOperation> NegatedCNot(int ControlIndex, int TargetIndex)
        {
            List<IQuantumOperation> listRetVal = new List<IQuantumOperation>();

            //verify input
            if (ControlIndex == TargetIndex)
                throw new DuplicateIndexesException("The control and target indexes must be different.");
            if ((ControlIndex < 0) || (TargetIndex < 0))
                throw new ArgumentOutOfRangeException("The indexes specified must be greater than or equal to 0.");

            //perform the negated CNot by flipping the control bit, then applying the normal CNot,
            //then flipping the control back to the original state.
            listRetVal.Add(new OperationNot(ControlIndex));
            listRetVal.Add(new OperationCNot(ControlIndex, TargetIndex));
            listRetVal.Add(new OperationNot(ControlIndex));

            return listRetVal;
        }

        /// <summary>
        /// The controlled reset operation. If the control qubit is 1 then all the
        /// target indexes are reset to 0.
        /// </summary>
        /// <param name="ControlIndex">The index of the control qubit.</param>
        /// <param name="TargetIndexes">The target indexes to reset if the control is
        /// set.</param>
        /// <param name="ClassicalValue">The classical value to reset.</param>
        /// <returns>Operations required to perform this.</returns>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes
        /// specified are not unique.</exception>
        /// <exception cref="ArgumentNullException">Thrown if any of the parameters
        /// are null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if any of the indexes
        /// specified are less than 0, or if TargetIndexes is an empty array.</exception>
        public List<IQuantumOperation> ControlledReset(int ControlIndex, int[] TargetIndexes,
        ClassicalResult ClassicalValue)
        {
            List<IQuantumOperation> listRetVal = new List<IQuantumOperation>();
            Dictionary<int, bool> dictUsedIndexes = new Dictionary<int, bool>();
            bool[] faActiveBits = null;

            //first verify all input
            if (TargetIndexes == null)
                throw new ArgumentNullException("TargetIndexes cannot be null");
            if (ClassicalValue == null)
                throw new ArgumentNullException("ClassicalValue cannot be null");
            if (TargetIndexes.Length <= 0)
                throw new ArgumentOutOfRangeException("TargetIndexes cannot be an empty array");
            if (ControlIndex < 0)
                throw new ArgumentOutOfRangeException("Control index must be at least 0, negative values are not allowed.");

            dictUsedIndexes[ControlIndex] = true;
            foreach (int iCurTargetIndex in TargetIndexes)
            {
                if (dictUsedIndexes.ContainsKey(iCurTargetIndex) == true)
                    throw new DuplicateIndexesException(string.Format("Indexes specified must be unique. {0} is used more than once.", iCurTargetIndex));
                if (iCurTargetIndex < 0)
                    throw new ArgumentOutOfRangeException(string.Format("All indexes specified must be at least 0. {0} violates this constraint.", iCurTargetIndex));
                dictUsedIndexes[iCurTargetIndex] = true;
            }

            //now build up the operations by just doing CNOT the correct number of times
            //on the "on" bits. This will reset it to 0
            faActiveBits = ClassicalValue.ToBoolArray();
            for (int iCurIndex = 0; iCurIndex < TargetIndexes.Length; iCurIndex++)
            {
                if ((iCurIndex < faActiveBits.Length)
                && (faActiveBits[iCurIndex] == true))
                {
                    listRetVal.Add(new OperationCNot(ControlIndex, TargetIndexes[iCurIndex]));
                }
            }

            return listRetVal;
        }


        /// <summary>
        /// Performs modular addition of X and Y: (x + y) mod n. All index arrays must be of 
        /// equal length.
        /// </summary>
        /// <param name="XIndexes">The indexes for x.</param>
        /// <param name="YIndexes">The indexes for y.</param>
        /// <param name="NIndexes">The indexes for N.</param>
        /// <param name="AncilliaIndexes">The index of an ancillia qubits, must be set to 0. This 
        /// array has to be 1 qubit greater than the others.</param>
        /// <returns>The operations to perform modular addition.</returns>
        /// <exception cref="ArgumentNullException">Thrown if any of the arrays passed in are
        /// null.</exception>
        /// <exception cref="ArgumentException">Thrown if the arrays passed in are not
        /// of equal length (excep ancillia) or less than zero.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any indexes are specified
        /// more than once.</exception>t
        public List<IQuantumOperation> ModularNAdder(int[] XIndexes, int[] YIndexes, int[] NIndexes,
        int[] AncilliaIndexes)
        {
            List<IQuantumOperation> listRetVal = new List<IQuantumOperation>();
            List<int> listAncilliaForAddIndexes = new List<int>();
            Dictionary<int, bool> dictUsedIndexes = new Dictionary<int, bool>();
            int iCarryIndex = AncilliaIndexes[AncilliaIndexes.Length - 2];
            int iControlIndex = AncilliaIndexes[AncilliaIndexes.Length - 1];

            //int iCarryIndex = AncilliaIndexes[AncilliaIndexes.Length - 3];
            //int iControlIndex = AncilliaIndexes[AncilliaIndexes.Length - 2];
            //int iResetAddAncilliaIndex = AncilliaIndexes[AncilliaIndexes.Length - 2];
            //List<int> listAncilliaForAddNoCarry = new List<int>();

            //verify the inputs
            if ((XIndexes == null) || (YIndexes == null) || (NIndexes == null) || (AncilliaIndexes == null))
                throw new ArgumentNullException("The arrays passed cannot be null");
            if ((XIndexes.Length != YIndexes.Length) || (XIndexes.Length != NIndexes.Length)
            || (YIndexes.Length != NIndexes.Length))
                throw new ArgumentException("XIndexes, YIndexes, and NIndexes arrays must be of equal length.");
            if ((XIndexes.Length + 2) != AncilliaIndexes.Length)
                throw new ArgumentException("The ancillia indexes must be 2 larger than the others.");

            //check for invalid indexes and duplicates
            foreach (int iCurIndex in XIndexes)
            {
                if (dictUsedIndexes.ContainsKey(iCurIndex) == true)
                    throw new DuplicateIndexesException(string.Format("All indexes passed must be unique. {0} is specified more than once. (Caught in X)", iCurIndex));
                if (iCurIndex < 0)
                    throw new ArgumentNullException(string.Format("All indexes must be at least 0. {0} is invalid. (Caught in X)", iCurIndex));
                dictUsedIndexes[iCurIndex] = true;
            }                //end foreach iCurIndex
            foreach (int iCurIndex in YIndexes)
            {
                if (dictUsedIndexes.ContainsKey(iCurIndex) == true)
                    throw new DuplicateIndexesException(string.Format("All indexes passed must be unique. {0} is specified more than once. (Caught in Y)", iCurIndex));
                if (iCurIndex < 0)
                    throw new ArgumentNullException(string.Format("All indexes must be at least 0. {0} is invalid. (Caught in Y)", iCurIndex));
                dictUsedIndexes[iCurIndex] = true;
            }                //end foreach iCurIndex
            foreach (int iCurIndex in NIndexes)
            {
                if (dictUsedIndexes.ContainsKey(iCurIndex) == true)
                    throw new DuplicateIndexesException(string.Format("All indexes passed must be unique. {0} is specified more than once. (Caught in N)", iCurIndex));
                if (iCurIndex < 0)
                    throw new ArgumentNullException(string.Format("All indexes must be at least 0. {0} is invalid. (Caught in N)", iCurIndex));
                dictUsedIndexes[iCurIndex] = true;
            }                //end foreach iCurIndex
            for (int iCurIndex = 0; iCurIndex < AncilliaIndexes.Length; iCurIndex++)
            {
                if (dictUsedIndexes.ContainsKey(AncilliaIndexes[iCurIndex]) == true)
                    throw new DuplicateIndexesException(string.Format("All indexes passed must be unique. {0} is specified more than once. (Caught in ancillia)", AncilliaIndexes[iCurIndex]));
                if (AncilliaIndexes[iCurIndex] < 0)
                    throw new ArgumentNullException(string.Format("All indexes must be at least 0. {0} is invalid. (Caught in ancillia )", AncilliaIndexes[iCurIndex]));
                dictUsedIndexes[AncilliaIndexes[iCurIndex]] = true;

                //also place all but the last one into another list to pass to add n
                if (iCurIndex < (AncilliaIndexes.Length - 1))
                {
                    listAncilliaForAddIndexes.Add(AncilliaIndexes[iCurIndex]);
                }
            }                //end foreach iCurIndex            

            //now build up the operations to perform modular addition. Instead
            //of complicating things by doing the swaps, make it more efficient by 
            //just passing in the right set of indexes to each op. (even though efficiency
            //isn't a goal of the simulation, it makes it a bit more readable)
            listRetVal.AddRange(this.AddN(XIndexes, YIndexes, listAncilliaForAddIndexes.ToArray()));

            listRetVal.AddRange(this.AddNInverse(NIndexes, YIndexes, listAncilliaForAddIndexes.ToArray()));

            listRetVal.AddRange(this.NegatedCNot(iCarryIndex, iControlIndex));
            throw new ApplicationException("Need to fix the controlled reset here");
            //listRetVal.AddRange(this.ControlledReset(iControlIndex, NIndexes));
            listRetVal.AddRange(this.AddN(NIndexes, YIndexes, listAncilliaForAddIndexes.ToArray()));

            //listRetVal.AddRange(this.ControlledReset(iControlIndex, NIndexes));
            listRetVal.AddRange(this.AddNInverse(XIndexes, YIndexes, listAncilliaForAddIndexes.ToArray()));

            listRetVal.Add(new OperationCNot(iCarryIndex, iControlIndex));
            listRetVal.AddRange(this.AddN(XIndexes, YIndexes, listAncilliaForAddIndexes.ToArray()));

            //TODO: need to reset anciallia after every add op above? (No, don't think so since
            //all but carry should be reset anyways.

            return listRetVal;
        }

        /// <summary>
        /// Perform Uf for factoring
        /// </summary>
        /// <param name="Register1Indexes">First register</param>
        /// <param name="Register2Indexes">Second register</param>
        /// <param name="N">Number being factored.</param>
        /// <returns>The operations to apply Uf</returns>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the 
        /// indexes specified are duplicates.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the parameters
        /// are null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the indexes
        /// specified are invalid.</exception>
        public List<IQuantumOperation> FactoringUf(int[] Register1Indexes, int[] Register2Indexes,
        int N)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Get the operations to apply the Quantum Fourier
        /// Transform.
        /// </summary>
        /// <param name="Indexes">Indexes to target.</param>
        /// <returns>Operations to perform the QFT.</returns>
        /// <exception cref="DuplicateIndexesException">Thrown if 
        /// any of the indexes specified are duplicaes.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if
        /// any of the indexes are less than 0.</exception>
        public List<IQuantumOperation> QuantumFourierTransform(int[] Indexes)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Get the operations to apply the inverse Quantum Fourier
        /// Transform.
        /// </summary>
        /// <param name="Indexes">Indexes to target.</param>
        /// <returns>Operations to perform the QFT.</returns>
        /// <exception cref="DuplicateIndexesException">Thrown if 
        /// any of the indexes specified are duplicaes.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if
        /// any of the indexes are less than 0.</exception>
        public List<IQuantumOperation> QuantumFourierTransformInverse(int[] Indexes)
        {
            List<IQuantumOperation> listRetVal = this.QuantumFourierTransform(Indexes);

            listRetVal.Reverse();

            return listRetVal;
        }


        #endregion "Algorithms needed to perform factoring"

        #region "Get common quantum registers"

        /// <summary>
        /// Get the EPR Pair (Phi plus, or Einstein Podolsky Rosen), which is defined 
        /// as the 2 qubit register:
        /// (1 / sqrt(2))(|00> + |11>)
        /// This is also a bell state.
        /// </summary>
        /// <returns>A quantum register in the state of Phi plus or the EPR pair.</returns>
        /// <seealso cref="GetRegisterPhiPlus"/>
        /// <remarks>This is the same register as returned by GetRegisterPhiPlus, and
        /// is included for completeness for those who may not be as familiar with the
        /// names of the other bell states.</remarks>
        public IQuantumRegister GetRegisterEPRPair()
        {
            return this.GetRegisterPhiPlus();
        }


        /// <summary>
        /// Get the EPR Pair (Phi plus, or Einstein Podolsky Rosen), which is defined 
        /// as the 2 qubit register:
        /// (1 / sqrt(2))(|00> + |11>)
        /// This is also a bell state.
        /// </summary>
        /// <returns>A quantum register in the state of Phi plus or the EPR pair.</returns>
        /// <seealso cref="GetRegisterEPRPair"/>
        /// <remarks>This is the same register as returned by GetRegisterEPRPair().</remarks>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister GetRegisterPhiPlus()
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Get the Phi minus register, which is defined as the two qubits:
        /// (1 / sqrt(2))(|00> - |11>)j
        /// This is also a bell state.
        /// </summary>
        /// <returns>A quantum register in the state of Phi minus.</returns>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister GetRegisterPhiMinus()
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Get the Psi plus register, which is defined as the two qubits:
        /// (1 / sqrt(2))(|01> + |10>)
        /// This is also a bell state.
        /// </summary>
        /// <returns>A quantum register in the state of Psi plus.</returns>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister GetRegisterPsiPlus()
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Get the Psi minus register, which is defined as the two qubits:
        /// (1 / sqrt(2))(|01> - |10>)
        /// This is also a bell state.
        /// </summary>
        /// <returns>A quantum register in the state of Psi plus.</returns>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister GetRegisterPsiMinus()
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Get the GHZ (Greenberger-Horne-Zeilinger) register, which is defined
        /// as the three qubits in the state:
        /// (1 / sqrt(2))(|000> + |111>)
        /// </summary>
        /// <returns>A quantum register in the GHZ state.</returns>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister GetRegisterGHZ()
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Get the W register, which is defined as the three qubits in the state:
        /// (1 / sqrt(3))(|100> + |010> + |001>)
        /// </summary>
        /// <returns>A quantum register in the W state.</returns>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public IQuantumRegister GetRegisterW()
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        #endregion

    }
}
