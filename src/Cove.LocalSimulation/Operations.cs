using CoveX.Base;
using System;
using System.Collections.Generic;


namespace CoveX.LocalSimulation
{
    /// <summary>
    /// Static instances of all the operations so they can be used easily.
    /// </summary>
    /// <remarks>
    /// This is partially due to a constraint in C#- static classes cannot implement
    /// interfaces or derive from classes other than object. This allows for operations
    /// to be applied as in 
    /// Qubit.ApplyOperation(Operations.Hadamard);
    /// Instead of the less concise:
    /// Qubit.ApplyOperation(new OperationHadamard());
    /// </remarks>
    public static class Operations
    {
        /// <summary>
        /// Perform the Hadamard operation on the qubit. This operation is
        /// also known as Hadamard-Walsh and the square root of not.
        /// </summary>
        /// <remarks>
        /// The set (OperationHadamard, OperationTGate) is universal for 1-qubit gates
        /// [1]	P. Kayne, R. Laflamme, and M. Mosca, An Introduction to Quantum Computing. 
        /// New York City, New York: Oxford University Press, 2007.
        /// </remarks>
        public static GeneralSimulatedOperation Hadamard
        {
            get
            {
                return new OperationHadamard();
            }
        }


        /// <summary>
        /// Perform the identity operation on the qubit. This does not change the
        /// state of the qubit.
        /// </summary>
        public static GeneralSimulatedOperation Identity
        {
            get
            {
                return new OperationIdentity();
            }
        }


        /// <summary>
        /// The Not operation. Also known as the Pauli X gate.
        /// </summary>
        public static GeneralSimulatedOperation Not
        {
            get
            {
                return new OperationNot();
            }
        }


        /// <summary>
        /// The S Gate operation
        /// </summary>
        public static GeneralSimulatedOperation SGate
        {
            get
            {
                return new OperationSGate();
            }
        }


        /// <summary>
        /// The T Gate operation
        /// </summary>
        public static GeneralSimulatedOperation TGate
        {
            get
            {
                return new OperationTGate();
            }
        }


        /// <summary>
        /// The Pauli Y Gate
        /// </summary>
        public static GeneralSimulatedOperation YGate
        {
            get
            {
                return new OperationYGate();
            }
        }


        /// <summary>
        /// The Pauli Z gate
        /// </summary>
        public static GeneralSimulatedOperation ZGate
        {
            get
            {
                return new OperationZGate();
            }
        }


        /// <summary>
        /// The controlled not (CNot) opeartion.
        /// </summary>
        public static GeneralSimulatedOperation CNot
        {
            get
            {
                return new OperationCNot();
            }
        }


        /// <summary>
        /// The controlled U operation.
        /// </summary>
        public static GeneralSimulatedOperation ControlledU
        {
            get
            {
                return new OperationControlledU();
            }
        }


        /// <summary>
        /// The Fredkin operation (controlled swap).
        /// </summary>
        public static GeneralSimulatedOperation Fredkin
        {
            get
            {
                return new OperationFredkin();
            }
        }


        /// <summary>
        /// The Toffoli opeartion (double controlled not).
        /// </summary>
        public static GeneralSimulatedOperation Toffoli
        {
            get
            {
                return new OperationToffoli();
            }
        }


        //get arrays of operations to perform more complex tasks


        /// <summary>
        /// Return the series of operations that performs the Sum operation.
        /// </summary>
        /// <param name="CarryIndex">The index of the carry qubit.</param>
        /// <param name="XIndex">The index of the first (x) qubit to sum.</param>
        /// <param name="YIndex">The index of the second (y) qubit to sum.</param>
        /// <returns>An array of operations that performs sum over the specified
        /// indexes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if any of
        /// the indexes are less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any of
        /// the index parameters are the same. All indexes must be unique.</exception>
        public static GeneralSimulatedOperation[] GetSum(int CarryIndex, int XIndex, int YIndex)
        {
            List<GeneralSimulatedOperation> listRetVal = new List<GeneralSimulatedOperation>();

            //verify input
            if ((CarryIndex < 0) || (XIndex < 0) || (YIndex < 0))
                throw new ArgumentException("All indexes must be at least 0. Negative indexes are invalid");
            if ((CarryIndex == XIndex) || (CarryIndex == YIndex) || (XIndex == YIndex))
                throw new DuplicateIndexesException("All indexes must be unique, duplicates are not allowed");

            //the array of operations to return
            listRetVal.Add(new OperationCNot(CarryIndex, YIndex));
            listRetVal.Add(new OperationCNot(XIndex, YIndex));

            return listRetVal.ToArray();
        }


        /// <summary>Return a series of simple operations that perform
        /// carry over the specified indexes.</summary>
        /// <param name="CarryIndex">The index of the carry qubit.</param>
        /// <param name="XIndex">Index of the first qubit to add.</param>
        /// <param name="YIndex">Index of the second qubit to add.</param>
        /// <param name="OutputCarryIndex">The index of the output carry index,
        /// which can be feed into CarryIndex of a subsequent carry. This qubit
        /// should be set to 0 on input, and is not reset to this if not.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if any of
        /// the indexes are less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any of
        /// the index parameters are the same. All indexes must be unique.</exception>
        public static GeneralSimulatedOperation[] GetCarry(int CarryIndex, int XIndex, int YIndex,
        int OutputCarryIndex)
        {
            List<GeneralSimulatedOperation> listRetVal = new List<GeneralSimulatedOperation>();

            //verify the inputs
            if ((CarryIndex < 0) || (XIndex < 0) || (YIndex < 0) || (OutputCarryIndex < 0))
                throw new ArgumentOutOfRangeException("All indexes specified must be at least 0. Negative indexes are invalid.");
            if ((CarryIndex == XIndex) || (CarryIndex == YIndex) || (CarryIndex == OutputCarryIndex)
            || (XIndex == YIndex) || (XIndex == OutputCarryIndex) || (YIndex == OutputCarryIndex))
                throw new DuplicateIndexesException("All indexes must be unique, duplicates are not allowed.");

            //add the operations
            listRetVal.Add(new OperationToffoli(XIndex, YIndex, OutputCarryIndex));
            listRetVal.Add(new OperationCNot(XIndex, YIndex));
            listRetVal.Add(new OperationToffoli(CarryIndex, YIndex, OutputCarryIndex));

            return listRetVal.ToArray();
        }


        /// <summary>Return a series of simple operations that perform
        /// carry over the specified indexes.</summary>
        /// <param name="CarryIndex">The index of the carry qubit.</param>
        /// <param name="XIndex">Index of the first qubit to add.</param>
        /// <param name="YIndex">Index of the second qubit to add.</param>
        /// <param name="OutputCarryIndex">The index of the output carry index,
        /// which can be feed into CarryIndex of a subsequent carry. This qubit
        /// should be set to 0 on input, and is not reset to this if not.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if any of
        /// the indexes are less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any of
        /// the index parameters are the same. All indexes must be unique.</exception>
        public static GeneralSimulatedOperation[] GetCarryInverse(int CarryIndex, int XIndex, int YIndex,
        int OutputCarryIndex)
        {
            List<GeneralSimulatedOperation> listRetVal = new List<GeneralSimulatedOperation>
                (GetCarry(CarryIndex, XIndex, YIndex, OutputCarryIndex));

            //the inverse is just applying them in reverse
            listRetVal.Reverse();

            return listRetVal.ToArray();
        }


        /// <summary>
        /// Get the operation to perform add n over the specified indexes. After this is applied
        /// the result will be in all indexes of YIndexes and also the last ScratchIndexes qubit.
        /// </summary>
        /// <param name="XIndexes">Indexes that represent the first number to add.
        /// These indexes remain unchanged after application.</param>
        /// <param name="YIndexes">Indexes that represent the second number to add. These
        /// plus the last ScratchIndexes index will have the result after application.</param>
        /// <param name="ScratchIndexes">These are the scratch indexes and should be initialized
        /// to 0, although they will not be reset to 0. The final index in this is the last
        /// output index.</param>
        /// <param name="ResultIndexes">The indexes that will hold the result. This
        /// can be null.</param>
        /// <returns>The operations to perform add n.</returns>
        /// <exception cref="ArgumentNullException">Thrown if any of the parameters except 
        /// ResultIndexes are null.</exception>
        /// <exception cref="SizeMismatchException">Thrown if the length of XIndexes and YIndexes
        /// are not equal (must add equal length registers).</exception>
        /// <exception cref="SizeMismatchException">Thrown if ScratchIndexes is not 1 larger
        /// than XIndexes and YIndexes</exception>
        public static GeneralSimulatedOperation[] GetAddN(int[] XIndexes, int[] YIndexes,
        int[] ScratchIndexes, ref int[] ResultIndexes)
        {
            List<GeneralSimulatedOperation> listRetVal = new List<GeneralSimulatedOperation>();
            Dictionary<int, bool> dictAlreadyUsedIndexes = new Dictionary<int, bool>();

            //first verify the input
            if ((XIndexes == null) || (YIndexes == null) || (ScratchIndexes == null))
                throw new ArgumentNullException("XIndexes, YIndexes, or ScratchIndexes cannot be null");
            if (XIndexes.Length != YIndexes.Length)
                throw new SizeMismatchException("XIndexes and YIndexes must be of equal length");
            if ((XIndexes.Length + 1) != ScratchIndexes.Length)
                throw new SizeMismatchException("ScratchIndexes must be 1 more in length than XIndexes and YIndexes");

            //make sure the indexes don't overlap
            foreach (int iCurIndex in XIndexes)
            {
                if (dictAlreadyUsedIndexes.ContainsKey(iCurIndex) == true)
                    throw new DuplicateIndexesException("Duplicate indexes were passed in. All indexes must be unique");
                dictAlreadyUsedIndexes[iCurIndex] = true;
            }
            foreach (int iCurIndex in YIndexes)
            {
                if (dictAlreadyUsedIndexes.ContainsKey(iCurIndex) == true)
                    throw new DuplicateIndexesException("Duplicate indexes were passed in. All indexes must be unique");
                dictAlreadyUsedIndexes[iCurIndex] = true;
            }
            foreach (int iCurIndex in ScratchIndexes)
            {
                if (dictAlreadyUsedIndexes.ContainsKey(iCurIndex) == true)
                    throw new DuplicateIndexesException("Duplicate indexes were passed in. All indexes must be unique");
                dictAlreadyUsedIndexes[iCurIndex] = true;
            }

            //set the output indexes if the user wants them. these are just all the YIndexes
            //and the last one in ScratchIndexes.
            if (ResultIndexes != null)
            {
                ResultIndexes = new int[YIndexes.Length + 1];
                for (int iCurIndex = 0; iCurIndex < (YIndexes.Length + 1); iCurIndex++)
                    ResultIndexes[iCurIndex] = YIndexes[iCurIndex];
                ResultIndexes[YIndexes.Length] = ScratchIndexes[YIndexes.Length];
            }                //end of setting result indexes

            //do all the carry operations.
            for (int iCurIndex = 0; iCurIndex < XIndexes.Length; iCurIndex++)
            {
                listRetVal.AddRange(GetCarry(ScratchIndexes[iCurIndex], XIndexes[iCurIndex],
                    YIndexes[iCurIndex], ScratchIndexes[iCurIndex + 1]));
            }     //end for iCurIndex

            //for the last ones do the CNot and sum
            listRetVal.Add(new OperationCNot(XIndexes[XIndexes.Length - 1], YIndexes[YIndexes.Length - 1]));
            listRetVal.AddRange(GetSum(ScratchIndexes[ScratchIndexes.Length - 2],
                XIndexes[XIndexes.Length - 1], YIndexes[YIndexes.Length - 1]));

            //finally add all the inverse carries and sum ops.
            for (int iCurIndex = XIndexes.Length - 2; iCurIndex >= 0; iCurIndex--)
            {
                listRetVal.AddRange(GetCarryInverse(ScratchIndexes[iCurIndex], XIndexes[iCurIndex],
                    YIndexes[iCurIndex], ScratchIndexes[iCurIndex + 1]));
                listRetVal.AddRange(GetSum(ScratchIndexes[iCurIndex], XIndexes[iCurIndex], YIndexes[iCurIndex]));
            }                //end of iteration through iCurIndex for carry inverse and sum gates

            return listRetVal.ToArray();
        }


        /// <summary>
        /// Get the operations necessary to perform the quantum fourier transform
        /// over the indexes specified.
        /// </summary>
        /// <param name="Indexes">Indexes to perform the quantum fourier transform
        /// over.</param>
        /// <returns>The operations that perform the quantum fourier transform.</returns>
        /// <exception cref="ArgumentNullException">Thrown if Indexes is null</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes
        /// in Indexes are duplicate.</exception>
        public static GeneralSimulatedOperation[] GetQuantumFourierTransform(int[] Indexes)
        {
            List<GeneralSimulatedOperation> listRetVal = new List<GeneralSimulatedOperation>();
            Dictionary<int, bool> dictAlreadyUsedIndexes = new Dictionary<int, bool>();

            //verify the inputs
            if (Indexes == null)
                throw new ArgumentNullException("The Indexes parameter cannot be null");
            foreach (int iCurIndex in Indexes)
            {
                if (dictAlreadyUsedIndexes.ContainsKey(iCurIndex) == true)
                    throw new DuplicateIndexesException("Duplicate indexes were passed in. All indexes must be unique");
                dictAlreadyUsedIndexes[iCurIndex] = true;
            }

            //now go through and perform the necessary ops on the qubits
            for (int iCurIndex = 0; iCurIndex < Indexes.Length; iCurIndex++)
            {
                listRetVal.Add(new OperationHadamard(iCurIndex));     //always start on this qubit with Hadamard

                for (int iRValue = 2; iRValue <= (Indexes.Length - iCurIndex); iRValue++)
                {
                    listRetVal.Add(new OperationControlledU(new OperationRotateK(iRValue),
                        (iCurIndex + (iRValue - 1)), iCurIndex));
                }        //end for iRValue
            }            //end for iCurIndex

            //now reverse the register. note that the < is integer division, so if the number
            //of qubits is odd it does not do the middle one.
            for (int iSwapIndex = 0; iSwapIndex < (Indexes.Length / 2); iSwapIndex++)
            {
                listRetVal.Add(new OperationSwap(iSwapIndex, (Indexes.Length - iSwapIndex)));
            }                //end for iSwapIndex

            return listRetVal.ToArray();
        }


        /// <summary>
        /// Get the operations necessary to perform the inverse quantum fourier transform
        /// over the indexes specified.
        /// </summary>
        /// <param name="Indexes">Indexes to perform the inverse quantum fourier transform
        /// over.</param>
        /// <returns>The operations that perform the inverse quantum fourier transform.</returns>
        /// <exception cref="ArgumentNullException">Thrown if Indexes is null</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes
        /// in Indexes are duplicate.</exception>
        public static GeneralSimulatedOperation[] GetQuantumFourierTransformInverse(int[] Indexes)
        {
            List<GeneralSimulatedOperation> listRetVal = new List<GeneralSimulatedOperation>
                (GetQuantumFourierTransform(Indexes));

            //just apply the operations in reverse order.
            listRetVal.Reverse();

            return listRetVal.ToArray();
        }

    }
}
