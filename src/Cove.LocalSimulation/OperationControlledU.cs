using CoveX.Base;
using CoveX.ClassicalUtilities;
using System;

namespace CoveX.LocalSimulation
{
    /// <summary>
    /// For this n qubit operation there is 1 control qubit and n - 1 target qubits. The 
    /// single qubit operation functions on all target qubits if the control qubit is |1>.
    /// </summary>
    public class OperationControlledU : GeneralSimulatedOperation, IOperationControlledU
    {
        /// <summary>
        /// The element in listTargetQubits that is the control qubit.
        /// </summary>
        protected const int CONTROL_INDEX = 0;


        /// <summary>
        /// The element in listTargetQubits that is the TargetIndex.
        /// </summary>
        protected const int TARGET_INDEX = 1;


        GeneralSimulatedOperation cTargetOperation = null;

        /// <summary>
        /// The default constructor, constructs a controled identity- which is
        /// effectively the identity on two qubits.
        /// </summary>
        public OperationControlledU() : this(new OperationIdentity(), 0, 1)
        {
        }


        /// <summary>
        /// Overloaded constructor to set the target operation.
        /// </summary>
        /// <param name="TargetOperation">The target operation</param>
        /// <exception cref="ArgumentException">Thrown if TargetOperation does
        /// not derive from GeneralSimulatedQubitOperation. The local simulation can
        /// only work on operations derived from it.</exception>
        public OperationControlledU(IQubitOperation TargetOperation) : this(TargetOperation, 0, 1)
        {
        }


        /// <summary>
        /// Overloaded constructor to set the target operation along with the 
        /// control and target indexes
        /// </summary>
        /// <param name="TargetOperation"></param>
        /// <param name="ControlIndex"></param>
        /// <param name="TargetIndex"></param>
        /// <exception cref="ArgumentException">Thrown if TargetOperation does
        /// not derive from GeneralSimulatedQubitOperation. The local simulation can
        /// only work on operations derived from it.</exception>
        public OperationControlledU(IQubitOperation TargetOperation, int ControlIndex, int TargetIndex)
        {
            this.SetTargetOperation(TargetOperation);
            this.listTargetQubits = new System.Collections.Generic.List<int>(new int[] { ControlIndex, TargetIndex });
        }


        /// <summary>
        /// Set the target operation. This will be applied if the control qubit is set. The target
        /// index of this operation is ignored- it will be the target index of this operation.
        /// </summary>
        /// <param name="TargetOperation">The new target operation</param>
        /// <exception cref="ArgumentException">Thrown if TargetOperation does
        /// not derive from GeneralSimulatedQubitOperation. The local simulation can
        /// only work on operations derived from it.</exception>
        public void SetTargetOperation(IQubitOperation TargetOperation)
        {
            if ((TargetOperation is GeneralSimulatedQubitOperation) == false)
                throw new ArgumentException("TargetOperation must derive from GeneralSimulatedQubitOperation");

            //deep copy the operation
            this.cTargetOperation = new GeneralSimulatedOperation();
            this.cTargetOperation.OperationMatrix = (TargetOperation as GeneralSimulatedQubitOperation).OperationMatrix.Clone();
            this.cTargetOperation.listTargetQubits = new System.Collections.Generic.List<int>(new int[] { 0 });

            //not standard for so indexes match up, so pluck off the matrix values from the operation
            ComplexMatrix cOp = this.cTargetOperation.OperationMatrix;
            this.OperationMatrix = new ComplexMatrix(new Complex[,] {
                {1, 0, 0, 0},
                {0, cOp.GetValue(0, 0), 0, cOp.GetValue(0, 1)},
                {0, 0, 1, 0},
                {0, cOp.GetValue(1, 0), 0, cOp.GetValue(1, 1)},
            });
        }


        /// <summary>
        /// Get the current target operation.
        /// </summary>
        /// <returns>The current target operation.</returns>
        public IQubitOperation GetTargetOperation()
        {
            GeneralSimulatedQubitOperation cRetVal = new GeneralSimulatedQubitOperation();

            //construct a deep copy
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(new int[] { 0 });
            cRetVal.OperationMatrix = this.cTargetOperation.OperationMatrix.Clone();

            return (IQubitOperation)cRetVal;
        }


        /// <summary>
        /// Get the current control index
        /// </summary>
        /// <returns>The current control index</returns>
        public int GetControlIndex()
        {
            return this.listTargetQubits[CONTROL_INDEX];
        }


        /// <summary>
        /// Get the target index.
        /// </summary>
        /// <returns>The current target index</returns>
        public int GetTargetIndex()
        {
            return this.listTargetQubits[TARGET_INDEX];
        }


        /// <summary>
        /// Set the control and target indexes
        /// </summary>
        /// <param name="ControlIndex">The control index</param>
        /// <param name="TargetIndex">The target index</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the
        /// indexes are less than 0. All indexes must be at least 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the
        /// indexes are the same. The control and target indexes must be
        /// different.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the
        /// indexes are less than 0. All indexes must be at least 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the
        /// indexes are the same. The control and target indexes must be
        /// different.</exception>
        public void SetIndexes(int ControlIndex, int TargetIndex)
        {
            //verify input
            if ((ControlIndex < 0) || (TargetIndex < 0))
                throw new ArgumentOutOfRangeException("The indexes must be at least 0. Negative indexes are invalid.");
            if (ControlIndex == TargetIndex)
                throw new DuplicateIndexesException("The indexes must be different.");

            this.listTargetQubits[CONTROL_INDEX] = ControlIndex;
            this.listTargetQubits[TARGET_INDEX] = TargetIndex;
        }


        /// <summary>
        /// Set the control index
        /// </summary>
        /// <param name="ControlIndex">The control index</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the
        /// indexes are less than 0. All indexes must be at least 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the
        /// indexes are the same. The control and target indexes must be
        /// different.</exception>
        public void SetControlIndex(int ControlIndex)
        {
            this.SetIndexes(ControlIndex, this.GetTargetIndex());
        }


        /// <summary>
        /// Set the target index.
        /// </summary>
        /// <param name="TargetIndex">The target index</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the
        /// indexes are less than 0. All indexes must be at least 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the
        /// indexes are the same. The control and target indexes must be
        /// different.</exception>
        public void SetTargetIndex(int TargetIndex)
        {
            this.SetIndexes(this.GetControlIndex(), TargetIndex);
        }


        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IOperationControlledU Clone()
        {
            OperationControlledU cRetVal = new OperationControlledU();

            cRetVal.OperationMatrix = this.OperationMatrix.Clone();
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(this.listTargetQubits);

            return (IOperationControlledU)cRetVal;
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
