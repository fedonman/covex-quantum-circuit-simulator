namespace CoveX.Base
{
    /// <summary>
    /// This operation collapses a qubit to 0. This is not a reversible operation.
    /// </summary>
    public interface IOperationReset : IQubitOperation
    {
        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        IOperationReset Clone();
    }
}
