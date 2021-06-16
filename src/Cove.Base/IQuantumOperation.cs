namespace CoveX.Base
{
    /// <summary>
    /// The interface for operations applied to qubits. This is meant to serve as a
    /// base interface, and there are no direct implementations.
    /// </summary>
    public interface IQuantumOperation : ICoveObject
    {
        /// <summary>
        /// Returns the number of qubits that the operation operates on. For example
        /// a CNot operation would return 2 since there are two qubits, target and control.
        /// </summary>
        /// <returns>The number of qubits the operation operates on.</returns>
        int NumberOfQubitsOperatesOn();


        /// <summary>
        /// Is this a valid quantum operation? All quantum operations must be unitary,
        /// so this allows for all operations to be checked before they are applied to
        /// registers.
        /// </summary>
        /// <returns>True if the operation is valid (unitary).</returns>
        bool IsValidOperation();


        /// <summary>
        /// Tensor two operations into one combined one.
        /// </summary>
        /// <param name="Operation">Operation to tensor this one with.</param>
        /// <returns>The tensored operation.</returns>
        IQuantumOperation Tensor(IQuantumOperation Operation);


        /// <summary>
        /// Combine two same sized operations into one.
        /// </summary>
        /// <param name="Operation">Operation to combine with this one.</param>
        /// <returns>The combined operation.</returns>
        IQuantumOperation Combine(IQuantumOperation Operation);


        /// <summary>
        /// Get a string that shows the operations name and target. An example might
        /// be something like "CNOT: Control = 0, Target = 2".
        /// </summary>
        /// <returns>A string representing the operation and what it targets</returns>
        string GetOperationAndTargets();
    }
}
