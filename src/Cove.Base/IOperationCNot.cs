using System;


namespace CoveX.Base
{
    /// <summary>
    /// Interface for the CNot (controlled Not) operation. Direct implementations of this interface
    /// should be named OperationCNot.
    /// </summary>
    public interface IOperationCNot : IQuantumOperation
    {
        /// <summary>
        /// Get the index of the control qubit when this operation is applied: either 0 or 1.
        /// </summary>
        /// <returns>The control index: 0 or 1</returns>
        int GetControlIndex();


        /// <summary>
        /// Get the index of the target qubit when this operation is applied: either 0 or 1.
        /// </summary>
        /// <returns>The target index: 0 or 1.</returns>
        int GetTargetIndex();


        /// <summary>
        /// Set the control and target indexes when this operation is applied. 
        /// The parameters must be (0, 1) or (1, 0).
        /// </summary>
        /// <param name="ControlIndex">Index of the control qubit: 0 or 1.</param>
        /// <param name="TargetIndex">Index of the target qubit: 0 or 1.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if either
        /// of the parameters is not 0 or 1.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the same index is specified
        /// for both parameters.</exception>
        /// <returns>A reference to this operation after this method has been performed.</returns>
        IOperationCNot SetIndexes(int ControlIndex, int TargetIndex);


        /// <summary>
        /// Whatever the target and control indexes are, they will be flipped. Example: if index 0 is control 
        /// and index 1 is target, after this call index 0 will be target and index 1 will be control.
        /// </summary>
        /// <returns>A reference to this operation after this method has been performed.</returns>
        IOperationCNot FlipTargetAndControlIndexes();


        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        IOperationCNot Clone();
    }
}
