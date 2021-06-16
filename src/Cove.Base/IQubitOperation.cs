using System;

namespace CoveX.Base
{

    /// <summary>
    /// The interface that all operations on qubits must implement. This is a base
    /// interface and there should not be direct implementations.
    /// </summary>
    public interface IQubitOperation : IQuantumOperation
    {

        /// <summary>
        /// Return the matrix that represents this operation
        /// </summary>
        /// <returns>The matrix representing this operation</returns>
        object GetOperationMatrix();


        /// <summary>
        /// Get the target index of this single qubit operation.
        /// </summary>
        /// <returns>The current target index.</returns>
        int GetTargetQubit();


        /// <summary>
        /// Set the target index of this single qubit operation.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// is less than 0.</exception>
        void SetTargetQubit(int NewTarget);
    }
}
