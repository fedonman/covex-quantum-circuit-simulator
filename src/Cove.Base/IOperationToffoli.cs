using System;

namespace CoveX.Base
{
    /// <summary>
    /// Interface for the Toffoli operation, which is a double controlled not or
    /// controlled controlled not. Direct implementations of this interface should 
    /// be named OperationToffoli.
    /// </summary>
    public interface IOperationToffoli : IQuantumOperation
    {
        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        IOperationToffoli Clone();


        /// <summary>
        /// Get the index of the target qubit when this operation is applied.
        /// </summary>
        /// <returns>The index of the target qubit in the register when this operation
        /// is applied.</returns>
        int GetTargetIndex();


        /// <summary>
        /// Get the index of the first control qubit in the register when this operation is applied.
        /// </summary>
        /// <returns>The index of the first control qubit in the register when this operation is
        /// applied.</returns>
        int GetFirstControlIndex();


        /// <summary>
        /// Get the index of the second control qubit in the register when this operation is applied.
        /// </summary>
        /// <returns>The index of the second control qubit in the register when this operation is
        /// applied.</returns>
        int GetSecondControlIndex();


        /// <summary>
        /// Set the indexes of qubits in a register when this operation is applied.
        /// </summary>
        /// <param name="FirstControlIndex">The index of the first control qubit.</param>
        /// <param name="SecondControlIndex">The index of the second control qubit.</param>
        /// <param name="TargetIndex">The index of the target qubit. The Not operation
        /// will be applied to this qubit if the control indexes are |1>.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if any
        /// of the parameters are less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the same index is specified
        /// for more than one parameter.</exception>
        /// <returns>A reference to this operation after this method has been performed.</returns>
        void SetIndexes(int FirstControlIndex, int SecondControlIndex, int TargetIndex);


        /// <summary>
        /// Set the first control index.
        /// </summary>
        /// <param name="FirstControlIndex">The first control index.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the
        /// index is less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the index
        /// matches one of the other targets.</exception>
        void SetFirstControlIndex(int FirstControlIndex);


        /// <summary>
        /// Set the second control index.
        /// </summary>
        /// <param name="SecondControlIndex">The second control index.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the
        /// index is less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the index
        /// matches one of the other targets.</exception>
        void SetSecondControlIndex(int SecondControlIndex);


        /// <summary>
        /// Set the target index.
        /// </summary>
        /// <param name="TargetIndex">The target index.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the
        /// index is less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the index
        /// matches one of the other targets.</exception>
        void SetTargetlIndex(int TargetIndex);
    }
}
