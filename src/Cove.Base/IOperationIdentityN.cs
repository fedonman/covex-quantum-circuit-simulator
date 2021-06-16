using System;

namespace CoveX.Base
{
    /// <summary>
    /// The identity operation for n qubits. The identity operation is essentially a 
    /// no operation (noop) since it does not change the state of the qubit(s). Direct 
    /// implementations of this interface should be named OperationIdentityN.
    /// </summary>
    public interface IOperationIdentityN : IQuantumOperation
    {
        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        IOperationIdentityN Clone();

        /// <summary>
        /// Get the size of this operation, the number of qubits it operates on.
        /// </summary>
        /// <returns>The number of qubits this operation operates on.</returns>
        int GetSize();


        /// <summary>
        /// Set the size of this operation, the number of qubits it operates on.
        /// </summary>
        /// <param name="Size">The new size of the opeartion</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if Size is less than
        /// or equal to 0.</exception>
        void SetSize(int Size);


        /// <summary>
        /// Get the target indexes for this operation.
        /// </summary>
        /// <returns></returns>
        int[] GetTargets();


        /// <summary>
        /// Set the target indexes for this operation. This also resizes
        /// the operation to match the number of indexes passed in.
        /// </summary>
        /// <param name="SetTargets">Target indexes of the operation</param>
        /// /// <exception cref="ArgumentNullException">Thrown if null is passed
        /// in.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if any
        /// of the indexes are less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any
        /// of the indexes in SetTargets are duplicates.</exception>
        void SetTargets(int[] SetTargets);

    }
}
