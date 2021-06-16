using CoveX.Base;
using System;


namespace CoveX.LocalSimulation
{
    /// <summary>
    /// This operation rotates a qubit about the Y axis by an arbitrary angle.
    /// </summary>
    public class OperationRotateY : GeneralSimulatedQubitOperation, IOperationRotateY
    {
        /// <summary>
        /// Current angle of rotation
        /// </summary>
        protected double dAngle = 0.0;


        /// <summary>
        /// Default constructor, construct an instance of the Rotate Y operation. The angle
        /// is 0, so this is effectively an identity operation until it is set.
        /// </summary>
        public OperationRotateY() : this(0, 0)
        {
        }


        /// <summary>
        /// Overloaded constructor to specify the target at time of construction.
        /// </summary>
        /// <param name="TargetIndex">The target index of this operation.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// is less than 0.</exception>
        public OperationRotateY(int TargetIndex) : this(0, 0)
        {
            this.SetTargetQubit(TargetIndex);
        }


        /// <summary>
        /// Construct an instance of the Rotate Y operation, which will rotate a qubit about the 
        /// Y axis by RotateBy.
        /// </summary>
        /// <param name="RotateBy">Angle to rotate about the Y axis by.</param>
        /// <param name="TargetIndex">Target index of this operation.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// is less than 0.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public OperationRotateY(int TargetIndex, double RotateBy) : base()
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
        public IOperationRotateY Clone()
        {
            OperationRotateY cRetVal = new OperationRotateY();

            cRetVal.OperationMatrix = this.OperationMatrix.Clone();
            cRetVal.dAngle = this.dAngle;
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(this.listTargetQubits);

            return (IOperationRotateY)cRetVal;
        }


        /// <summary>
        /// Get the amount that this operation rotates about the Y axis by.
        /// </summary>
        /// <returns>The amount rotated by.</returns>
        public double GetRotateBy()
        {
            return this.dAngle;
        }


        /// <summary>
        /// Set the amount that this operation rotates about the Y axis by.
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
