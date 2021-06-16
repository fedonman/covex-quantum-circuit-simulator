namespace CoveX.Base
{
    /// <summary>
    /// Interface for the Rotation by K, a more general S or T gate. Used in
    /// the quantum Fourier transform as the target of a control operation
    /// </summary>
    public interface IOperationRotateK : IQubitOperation
    {
        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        IOperationRotateK Clone();


        /// <summary>
        /// Get the K value of this operation
        /// </summary>
        /// <returns>The current K value.</returns>
        double GetK();


        /// <summary>
        /// Set the K value of this operation.
        /// </summary>
        /// <param name="K">New K value.</param>
        void SetK(double K);
    }
}
