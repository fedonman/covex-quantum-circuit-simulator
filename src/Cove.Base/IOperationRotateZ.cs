﻿namespace CoveX.Base
{
    /// <summary>
    /// Interface for all operations that perform arbitrary rotations about the Z axis. Direct 
    /// implementations of this interface should be named OperationRotateZ.
    /// </summary>
    public interface IOperationRotateZ : IQubitOperation
    {
        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        IOperationRotateZ Clone();


        /// <summary>
        /// Get the amount that this operation rotates about the Z axis by.
        /// </summary>
        /// <returns>The amount rotated by.</returns>
        double GetRotateBy();


        /// <summary>
        /// Set the amount that this operation rotates about the Z axis by.
        /// </summary>
        /// <param name="RotateBy">Amount to rotate by.</param>
        void SetRotateBy(double RotateBy);
    }
}
