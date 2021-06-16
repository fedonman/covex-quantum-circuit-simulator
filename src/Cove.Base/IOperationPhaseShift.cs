namespace CoveX.Base
{
    /// <summary>
    /// Interface for all operations to perform arbitrary phase shifts. Direct implementations 
    /// of this interface should be named OperationPhaseShift.
    /// </summary>
    public interface IOperationPhaseShift : IQubitOperation
    {
        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        IOperationPhaseShift Clone();

        /// <summary>
        /// Get the amount that this operation phase shifts by.
        /// </summary>
        /// <returns>The amount phase shifted by.</returns>
        double GetPhaseShiftBy();


        /// <summary>
        /// Set the amount that this operation phase shifts by.
        /// </summary>
        /// <param name="PhaseShiftBy">Amount to rotate by.</param>
        void SetPhaseShiftBy(double PhaseShiftBy);
    }
}
