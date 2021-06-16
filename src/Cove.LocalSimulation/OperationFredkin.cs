using CoveX.Base;
using CoveX.ClassicalUtilities;
using System;

namespace CoveX.LocalSimulation
{
    /// <summary>
    /// The Fredkin operation, which is also known as controlled swap.
    /// </summary>
    public class OperationFredkin : GeneralSimulatedOperation, IOperationFredkin
    {
        /// <summary>
        /// The index in listTargetQubits that is the control qubit.
        /// </summary>
        protected const int CONTROL_INDEX = 0;

        /// <summary>
        /// The index in listTargetQubits that is the first swap (x) qubit.
        /// </summary>
        protected const int SWAP_FIRST_INDEX = 1;

        /// <summary>
        /// The index in listTargetQubits that is the second swap (y) qubit
        /// </summary>
        protected const int SWAP_SECOND_INDEX = 2;


        /// <summary>
        /// The default constructor
        /// </summary>
        public OperationFredkin()
        {
            this.OperationMatrix = new ComplexMatrix(new Complex[,] {
                {1, 0, 0, 0, 0, 0, 0, 0},
                {0, 1, 0, 0, 0, 0, 0, 0},
                {0, 0, 1, 0, 0, 0, 0, 0},
                {0, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 0, 0, 1, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 1, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 1}
            });

            //standard ordering
            this.listTargetQubits = new System.Collections.Generic.List<int>(new int[] { 0, 1, 2 });
        }


        /// <summary>
        /// Overload the constructor to also set the target indexes at construction
        /// </summary>
        /// <param name="ControlIndex">The control index</param>
        /// <param name="FirstSwapIndex">The first swap index</param>
        /// <param name="SecondSwapIndex">The second swap index</param>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the indexes
        /// are duplicates.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if any of the indexes
        /// specified are less than 0.</exception>
        public OperationFredkin(int ControlIndex, int FirstSwapIndex, int SecondSwapIndex) : this()
        {
            this.SetIndexes(ControlIndex, FirstSwapIndex, SecondSwapIndex);
        }


        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IOperationFredkin Clone()
        {
            OperationFredkin cRetVal = new OperationFredkin();

            cRetVal.OperationMatrix = this.OperationMatrix.Clone();
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(this.listTargetQubits);

            return (IOperationFredkin)cRetVal;
        }


        /// <summary>
        /// Get the index of the control qubit when this operation is applied.
        /// </summary>
        /// <returns>The index of the control qubit in the register when this operation
        /// is applied.</returns>
        public int GetControlIndex()
        {
            return this.listTargetQubits[CONTROL_INDEX];
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
        /// Set the indexes of qubits in a register when this operation is applied.
        /// </summary>
        /// <param name="ControlIndex">The index of the control qubit.</param>
        /// <param name="FirstSwapIndex">The index of the first qubit to swap.</param>
        /// <param name="SecondSwapIndex">The index of the second qubit to swap.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if any
        /// of the parameters are less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the same index is specified
        /// for more than one parameter.</exception>
        /// <returns>A reference to this operation after this method has been performed.</returns>
        public IOperationFredkin SetIndexes(int ControlIndex, int FirstSwapIndex, int SecondSwapIndex)
        {
            //validate input
            if ((ControlIndex < 0) || (FirstSwapIndex < 0) || (SecondSwapIndex < 0))
                throw new ArgumentOutOfRangeException(string.Format("All parameters to OperationFredkin.SetIndexes() must be greater than or equal to 0. ControlIndex = {0}, FirstSwapIndex = {1}, SecondSwapIndex = {2}", ControlIndex, FirstSwapIndex, SecondSwapIndex));
            if ((ControlIndex == FirstSwapIndex) || (ControlIndex == SecondSwapIndex) || (FirstSwapIndex == SecondSwapIndex))
                throw new ArgumentOutOfRangeException(string.Format("All indexes passed to OperationFredkin.SetIndexes() must unique, parameters cannot have the same value. ControlIndex = {0}, FirstSwapIndex = {1}, SecondSwapIndex = {2}", ControlIndex, FirstSwapIndex, SecondSwapIndex));

            this.listTargetQubits[CONTROL_INDEX] = ControlIndex;
            this.listTargetQubits[SWAP_FIRST_INDEX] = FirstSwapIndex;
            this.listTargetQubits[SWAP_SECOND_INDEX] = SecondSwapIndex;

            return this;
        }


        /// <summary>
        /// Get a string that shows the operations name and target. An example might
        /// be something like "CNOT: Control = 0, Target = 2".
        /// </summary>
        /// <returns>A string representing the operation and what it targets</returns>
        public override string GetOperationAndTargets()
        {
            return string.Format("{0}: Control = {1}, Swap1 = {2}, Swap2 = {3}", System.Reflection.MethodBase.GetCurrentMethod().Name,
                this.listTargetQubits[CONTROL_INDEX], this.listTargetQubits[SWAP_FIRST_INDEX],
                this.listTargetQubits[SWAP_SECOND_INDEX]);
        }
    }
}
