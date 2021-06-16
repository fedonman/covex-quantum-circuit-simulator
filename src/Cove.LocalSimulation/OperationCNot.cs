using CoveX.Base;
using CoveX.ClassicalUtilities;
using System;


namespace CoveX.LocalSimulation
{
    /// <summary>
    /// The controlled not, or CNot, operation. If the control qubit is |1> then the Not (X gate)
    /// operation is performed on the target qubit.
    /// </summary>
    public class OperationCNot : GeneralSimulatedOperation, IOperationCNot
    {
        /// <summary>
        /// The element in listTargetQubits that is the control qubit.
        /// </summary>
        protected const int CONTROL_INDEX = 0;


        /// <summary>
        /// The element in listTargetQubits that is the TargetIndex.
        /// </summary>
        protected const int TARGET_INDEX = 1;


        /// <summary>
        /// The default constructor. By default the control will be qubit index 0
        /// and the target will be qubit index 1.
        /// </summary>
        public OperationCNot()
        {
            this.OperationMatrix = new ComplexMatrix(new Complex[,] {
                {1, 0, 0, 0},
                {0, 0, 0, 1},
                {0, 0, 1, 0},
                {0, 1, 0, 0}});

            //standard ordering
            this.listTargetQubits = new System.Collections.Generic.List<int>(new int[] { 0, 1 });
        }


        /// <summary>
        /// Constructor overloaded to also specify the control and target
        /// qubits at construction.
        /// </summary>
        /// <param name="ControlIndex">The index in the register that will
        /// be the control qubit.</param>
        /// <param name="TargetIndex">The index in the register that will
        /// be the target qubit</param>
        public OperationCNot(int ControlIndex, int TargetIndex) : this()
        {
            this.SetIndexes(ControlIndex, TargetIndex);
        }


        /// <summary>
        /// Get the index of the control qubit when this operation is applied: either 0 or 1.
        /// </summary>
        /// <returns>The control index: 0 or 1</returns>
        public int GetControlIndex()
        {
            return this.listTargetQubits[CONTROL_INDEX];
        }


        /// <summary>
        /// Get the index of the target qubit when this operation is applied: either 0 or 1.
        /// </summary>
        /// <returns>The target index: 0 or 1.</returns>
        public int GetTargetIndex()
        {
            return this.listTargetQubits[TARGET_INDEX];
        }


        /// <summary>
        /// Set the control and target indexes when this operation is applied. 
        /// </summary>
        /// <param name="ControlIndex">Index of the control qubit, must be >= 0. </param>
        /// <param name="TargetIndex">Index of the target qubit, must be >= 0.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if either
        /// of the parameters is less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the same index is specified
        /// for both parameters.</exception>
        /// <returns>A reference to this operation after this method has been performed.</returns>
        public IOperationCNot SetIndexes(int ControlIndex, int TargetIndex)
        {
            //validate input
            if ((ControlIndex < 0) || (TargetIndex < 0))
                throw new ArgumentOutOfRangeException(string.Format("All parameters to OperationCNot.SetIndexes() must be greater than or equal to 0. ControlIndex = {0}, TargetIndex = {1}", ControlIndex, TargetIndex));
            if (ControlIndex == TargetIndex)
                throw new DuplicateIndexesException(string.Format("The indexes passed to OperationCNot.SetIndexes() must unique, they cannot be the same value. ControlIndex = {0}, TargetIndex = {1}.", ControlIndex, TargetIndex));

            this.listTargetQubits = new System.Collections.Generic.List<int>(new int[] { ControlIndex, TargetIndex });

            return this;
        }


        /// <summary>
        /// Whatever the target and control indexes are, they will be flipped. Example: if index 0 is control 
        /// and index 1 is target, after this call index 0 will be target and index 1 will be control.
        /// </summary>
        /// <returns>A reference to this operation after this method has been performed.</returns>
        public IOperationCNot FlipTargetAndControlIndexes()
        {
            this.listTargetQubits = this.listTargetQubits = new System.Collections.Generic.List<int>(new int[] {
                this.listTargetQubits[TARGET_INDEX], this.listTargetQubits[CONTROL_INDEX] });

            return this;
        }


        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IOperationCNot Clone()
        {
            OperationCNot cRetVal = new OperationCNot();

            cRetVal.OperationMatrix = this.OperationMatrix.Clone();
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(this.listTargetQubits);

            return (IOperationCNot)cRetVal;
        }


        /// <summary>
        /// Get a string that shows the operations name and target. An example might
        /// be something like "CNOT: Control = 0, Target = 2".
        /// </summary>
        /// <returns>A string representing the operation and what it targets</returns>
        public override string GetOperationAndTargets()
        {
            return string.Format("{0}: Control = {1}, Target = {2}", System.Reflection.MethodBase.GetCurrentMethod().Name,
                this.listTargetQubits[CONTROL_INDEX], this.listTargetQubits[TARGET_INDEX]);
        }

    }
}
