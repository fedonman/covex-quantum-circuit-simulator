using System;

namespace CoveX.Base
{
    /// <summary>
    /// Interface for the swap operation. Direct implementations of this interface should 
    /// be named OperationSwap.
    /// </summary>
    public interface IOperationSwap : IQuantumOperation
    {
        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        IOperationSwap Clone();


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
        /// Set the target qubit of the first qubit to swap.
        /// </summary>
        /// <param name="FirstSwapIndex">The new target index of the first
        /// qubit to swap.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if FirstSwapIndex
        /// is less than 0. All indexes must be greater than or equal to zero.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if FirstSwapIndex
        /// is the same as the second swap index.</exception>
        void SetFirstSwapIndex(int FirstSwapIndex);


        /// <summary>
        /// Set the target qubit of the second qubit to swap.
        /// </summary>
        /// <param name="SecondSwapIndex">The new target index of the second
        /// qubit to swap.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if SecondSwapIndex
        /// is less than 0. All indexes must be greater than or equal to zero.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if SecondSwapIndex
        /// is the same as the first swap index.</exception>
        void SetSecondSwapIndex(int SecondSwapIndex);


        /// <summary>
        /// Set both target indexes to swap.
        /// </summary>
        /// <param name="FirstSwapIndex">The index of the first qubit to swap.</param>
        /// <param name="SecondSwapIndex">The index of the second qubit to swap.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if either parameter
        /// is less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the two 
        /// index parameters are the same. The indexes must be unique.</exception>
        void SetSwapIndexes(int FirstSwapIndex, int SecondSwapIndex);
    }
}
