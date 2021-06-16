using CoveX.Base;
using System;

namespace CoveX.LocalSimulation
{
    /// <summary>
    /// Rotate a qubit by an arbitrary angle abou the X axis.
    /// </summary>
    public class OperationRotateX : GeneralSimulatedQubitOperation, IOperationRotateX
    {
        /// <summary>
        /// Current angle of rotation
        /// </summary>
        protected double dAngle = 0.0;

        /// <summary>
        /// Default constructor, construct an instance of the Rotate X operation. The angle
        /// is 0, so this is effectively an identity operation until it is set.
        /// </summary>
        public OperationRotateX() : this(0, 0)
        {
        }


        /// <summary>
        /// Overloaded constructor to specify the target at time of construction.
        /// </summary>
        /// <param name="TargetIndex">The target index of this operation.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// is less than 0.</exception>
        public OperationRotateX(int TargetIndex) : this(0, 0)
        {
            this.SetTargetQubit(TargetIndex);
        }


        /// <summary>
        /// Construct an instance of the Rotate X operation, which will rotate a qubit about the 
        /// X axis by RotateBy.
        /// </summary>
        /// <param name="TargetIndex">Target index of this operation.</param>
        /// <param name="RotateBy">Angle to rotate about the X axis by.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// is less than 0.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public OperationRotateX(int TargetIndex, double RotateBy) : base()
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
        public IOperationRotateX Clone()
        {
            OperationRotateX cRetVal = new OperationRotateX();

            cRetVal.OperationMatrix = this.OperationMatrix.Clone();
            cRetVal.dAngle = this.dAngle;
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(this.listTargetQubits);

            return (IOperationRotateX)cRetVal;
        }


        /// <summary>
        /// Get the amount that this operation rotates about the X axis by.
        /// </summary>
        /// <returns>The amount rotated by.</returns>
        public double GetRotateBy()
        {
            return this.dAngle;
        }


        /// <summary>
        /// Set the amount that this operation rotates about the X axis by.
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
