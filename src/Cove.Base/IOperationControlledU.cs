using System;

namespace CoveX.Base
{
    /// <summary>
    /// Interface for the controlled-U operation. The controlled U operation tests a
    /// control qubit, and if true applies the single qubit operation U to all other
    /// (target) qubits. Direct implementations of this interface should be named OperationControlledU.
    /// </summary>
    public interface IOperationControlledU : IQuantumOperation
    {
        /// <summary>
        /// Set the target operation. This will be applied if the control qubit is set.
        /// </summary>
        /// <param name="TargetOperation">The new target operation</param>
        void SetTargetOperation(IQubitOperation TargetOperation);


        /// <summary>
        /// Get the current target operation.
        /// </summary>
        /// <returns>The current target operation.</returns>
        IQubitOperation GetTargetOperation();


        /// <summary>
        /// Get the current control index
        /// </summary>
        /// <returns>The current control index</returns>
        int GetControlIndex();


        /// <summary>
        /// Get the target index.
        /// </summary>
        /// <returns>The current target index</returns>
        int GetTargetIndex();


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
        void SetIndexes(int ControlIndex, int TargetIndex);


        /// <summary>
        /// Set the control index
        /// </summary>
        /// <param name="ControlIndex">The control index</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the
        /// indexes are less than 0. All indexes must be at least 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the
        /// indexes are the same. The control and target indexes must be
        /// different.</exception>
        void SetControlIndex(int ControlIndex);


        /// <summary>
        /// Set the target index.
        /// </summary>
        /// <param name="TargetIndex">The target index</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the
        /// indexes are less than 0. All indexes must be at least 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the
        /// indexes are the same. The control and target indexes must be
        /// different.</exception>
        void SetTargetIndex(int TargetIndex);


        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        IOperationControlledU Clone();

    }
}
