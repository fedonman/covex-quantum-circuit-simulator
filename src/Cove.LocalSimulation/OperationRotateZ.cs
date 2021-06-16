using CoveX.Base;
using System;

namespace CoveX.LocalSimulation
{
    /// <summary>
    /// This operation rotates a qubit about the Z axis by an arbitrary angle.
    /// </summary>
    public class OperationRotateZ : GeneralSimulatedQubitOperation, IOperationRotateZ
    {
        /// <summary>
        /// Current angle of rotation
        /// </summary>
        protected double dAngle = 0.0;


        /// <summary>
        /// Default constructor, construct an instance of the Rotate Z operation. The angle
        /// is 0, so this is effectively an identity operation until it is set.
        /// </summary>
        public OperationRotateZ() : this(0, 0)
        {
        }


        /// <summary>
        /// Overloaded constructor to specify the target at time of construction.
        /// </summary>
        /// <param name="TargetIndex">The target index of this operation.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// is less than 0.</exception>
        public OperationRotateZ(int TargetIndex) : this(TargetIndex, 0)
        {
            this.SetTargetQubit(TargetIndex);
        }


        /// <summary>
        /// Construct an instance of the Rotate Z operation, which will rotate a qubit about the 
        /// Z axis by RotateBy.
        /// </summary>
        /// <param name="TargetIndex">Target index of this operation.</param>
        /// <param name="RotateBy">Angle to rotate about the Z axis by.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// is less than 0.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public OperationRotateZ(int TargetIndex, double RotateBy) : base()
        {
            this.dAngle = RotateBy;

            //TODO: still need to set the operation matrix
            throw new NotImplementedException("Not yet implemented");
        }


        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IOperationRotateZ Clone()
        {
            OperationRotateZ cRetVal = new OperationRotateZ();

            cRetVal.OperationMatrix = this.OperationMatrix.Clone();
            cRetVal.dAngle = this.dAngle;
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(this.listTargetQubits);

            return (IOperationRotateZ)cRetVal;
        }


        /// <summary>
        /// Get the amount that this operation rotates about the Z axis by.
        /// </summary>
        /// <returns>The amount rotated by.</returns>
        public double GetRotateBy()
        {
            return dAngle;
        }


        /// <summary>
        /// Set the amount that this operation rotates about the Z axis by.
        /// </summary>
        /// <param name="RotateBy">Amount to rotate by.</param>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public void SetRotateBy(double RotateBy)
        {
            this.dAngle = RotateBy;

            //TODO: still need to set the operation matrix
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }
    }
}
