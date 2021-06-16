using System;

namespace CoveX.Base
{
    /// <summary>
    /// Interface for the Fredkin operation, which is a controlled swap. Direct 
    /// implementations of this interface should be named OperationFredkin.
    /// </summary>
    public interface IOperationFredkin : IQuantumOperation
    {
        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        IOperationFredkin Clone();


        /// <summary>
        /// Get the index of the control qubit when this operation is applied.
        /// </summary>
        /// <returns>The index of the control qubit in the register when this operation
        /// is applied.</returns>
        int GetControlIndex();


        /// <summary>
        /// Get the index of the first qubit in the register to swap when this operation is applied.
        /// </summary>
        /// <returns>The index of the first qubit in the register to swap when this operation is
        /// applied.</returns>
        int GetFirstSwapIndex();


        /// <summary>
        /// Get the index of the second qubit in the register to swap when this operation is applied.
        /// </summary>
        /// <returns>The index of the first qubit in the register to swap when this operation is
        /// applied.</returns>
        int GetSecondSwapIndex();


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
        IOperationFredkin SetIndexes(int ControlIndex, int FirstSwapIndex, int SecondSwapIndex);
    }
}
