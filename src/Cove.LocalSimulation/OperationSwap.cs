using CoveX.Base;
using CoveX.ClassicalUtilities;
using System;


namespace CoveX.LocalSimulation
{
    /// <summary>
    /// This is the class for the swap operation. This swaps the two qubits it
    /// operates on.
    /// </summary>
    public class OperationSwap : GeneralSimulatedOperation, IOperationSwap
    {
        /// <summary>
        /// The index in listTargetQubits that is the first qubit to swap.
        /// </summary>
        protected const int SWAP_FIRST_INDEX = 0;

        /// <summary>
        /// The index in listTargetQubits that is the second qubit to swap.
        /// </summary>
        protected const int SWAP_SECOND_INDEX = 1;


        /// <summary>
        /// The default constructor.
        /// </summary>
        public OperationSwap()
        {
            this.OperationMatrix = new ComplexMatrix(new Complex[,] {
                {1, 0, 0, 0},
                {0, 0, 1, 0},
                {0, 1, 0, 0},
                {0, 0, 0, 1}
            });


            //standard ordering
            this.listTargetQubits = new System.Collections.Generic.List<int>(new int[] { 0, 1 });
        }


        /// <summary>Overloaded constructor to also set the swap indexes 
        /// at the time of construction</summary>
        /// <param name="FirstSwapIndex">The index of the first qubit to swap.</param>
        /// <param name="SecondSwapIndex">The index of the second qubit to swap.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if either parameter
        /// is less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the two 
        /// index parameters are the same. The indexes must be unique.</exception>
        public OperationSwap(int FirstSwapIndex, int SecondSwapIndex) : this()
        {
            this.SetSwapIndexes(FirstSwapIndex, SecondSwapIndex);
        }


        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IOperationSwap Clone()
        {
            OperationSwap cRetVal = new OperationSwap();

            cRetVal.OperationMatrix = this.OperationMatrix.Clone();
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(this.listTargetQubits);

            return (IOperationSwap)cRetVal;
        }


        /// <summary>
        /// Get the index of the first qubit in the register to swap when this operation is applied.
        /// </summary>
        /// <returns>The index of the first qubit in the register to swap when this operation is
        /// applied.</returns>
        public int GetFirstSwapIndex()
        {
            return this.listTargetQubits[SWAP_FIRST_INDEX];
        }


        /// <summary>
        /// Get the index of the second qubit in the register to swap when this operation is applied.
        /// </summary>
        /// <returns>The index of the first qubit in the register to swap when this operation is
        /// applied.</returns>
        public int GetSecondSwapIndex()
        {
            return this.listTargetQubits[SWAP_SECOND_INDEX];
        }



        /// <summary>
        /// Set the target qubit of the first qubit to swap.
        /// </summary>
        /// <param name="FirstSwapIndex">The new target index of the first
        /// qubit to swap.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if FirstSwapIndex
        /// is less than 0. All indexes must be greater than or equal to zero.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if FirstSwapIndex
        /// is the same as the second swap index.</exception>
        public void SetFirstSwapIndex(int FirstSwapIndex)
        {
            //verify input
            if (FirstSwapIndex < 0)
                throw new ArgumentException("The FirstSwapIndex must be at least 0, negative indexes are not allowed");
            if (FirstSwapIndex == this.listTargetQubits[SWAP_SECOND_INDEX])
                throw new DuplicateIndexesException("Indexes must be unique. FirstSwapIndex was the same as the second swap index.");

            //set the new target
            this.listTargetQubits[SWAP_FIRST_INDEX] = FirstSwapIndex;
        }


        /// <summary>
        /// Set the target qubit of the second qubit to swap.
        /// </summary>
        /// <param name="SecondSwapIndex">The new target index of the second
        /// qubit to swap.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if SecondSwapIndex
        /// is less than 0. All indexes must be greater than or equal to zero.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if SecondSwapIndex
        /// is the same as the first swap index.</exception>
        public void SetSecondSwapIndex(int SecondSwapIndex)
        {
            //verify input
            if (SecondSwapIndex < 0)
                throw new ArgumentException("The SecondSwapIndex must be at least 0, negative indexes are not allowed");
            if (SecondSwapIndex == this.listTargetQubits[SWAP_FIRST_INDEX])
                throw new DuplicateIndexesException("Indexes must be unique. SecondSwapIndex was the same as the first swap index.");

            //set the new target
            this.listTargetQubits[SWAP_SECOND_INDEX] = SecondSwapIndex;
        }


        /// <summary>
        /// Set both target indexes to swap.
        /// </summary>
        /// <param name="FirstSwapIndex">The index of the first qubit to swap.</param>
        /// <param name="SecondSwapIndex">The index of the second qubit to swap.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if either parameter
        /// is less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the two 
        /// index parameters are the same. The indexes must be unique.</exception>
        public void SetSwapIndexes(int FirstSwapIndex, int SecondSwapIndex)
        {
            //verify input
            if (FirstSwapIndex < 0)
                throw new ArgumentException("The FirstSwapIndex must be at least 0, negative indexes are not allowed");
            if (SecondSwapIndex < 0)
                throw new ArgumentException("The SecondSwapIndex must be at least 0, negative indexes are not allowed");
            if (SecondSwapIndex == FirstSwapIndex)
                throw new DuplicateIndexesException("Indexes must be unique. SecondSwapIndex was the same as the FirstSwapIndex.");

            //set the new target
            this.listTargetQubits[SWAP_FIRST_INDEX] = FirstSwapIndex;
            this.listTargetQubits[SWAP_SECOND_INDEX] = SecondSwapIndex;
        }


        /// <summary>
        /// Get a string that shows the operations name and target. An example might
        /// be something like "CNOT: Control = 0, Target = 2".
        /// </summary>
        /// <returns>A string representing the operation and what it targets</returns>
        public override string GetOperationAndTargets()
        {
            return string.Format("{0}: Swap1 = {1}, Swap2 = {2}", System.Reflection.MethodBase.GetCurrentMethod().Name,
                this.listTargetQubits[SWAP_FIRST_INDEX], this.listTargetQubits[SWAP_SECOND_INDEX]);
        }

    }
}
