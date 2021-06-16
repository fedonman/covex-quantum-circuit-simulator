
namespace CoveX.Base
{

    /// <summary>
    /// Interface for all classes that perform the Hadamard operation. Direct 
    /// implementations of this interface should be named OperationHadamard.
    /// </summary>
    public interface IOperationHadamard : IQubitOperation
    {
        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        IOperationHadamard Clone();
    }
}
