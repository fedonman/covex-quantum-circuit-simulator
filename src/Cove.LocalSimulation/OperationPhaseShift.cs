using CoveX.Base;
using System;


namespace CoveX.LocalSimulation
{
    /// <summary>
    /// Perform an arbitrary phase shift on a qubit.
    /// </summary>
    public class OperationPhaseShift : GeneralSimulatedOperation
    {
        /// <summary>
        /// Default constructor, with no phase shift- effectively an identity operation
        /// until the phase shift is set.
        /// </summary>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public OperationPhaseShift() : this(0, 0)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Overload the constructor to specifiy the target index at construction.
        /// </summary>
        /// <param name="TargetIndex">The target index for this operation</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// index is less than 0.</exception>
        public OperationPhaseShift(int TargetIndex) : this(0, 0)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Construction a phase shift operator, shifting by PhaseShiftBy.
        /// </summary>
        /// <param name="TargetIndex">The target index for this operation</param>
        /// <param name="PhaseShiftBy">Amount to phase shift by</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// index is less than 0.</exception>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public OperationPhaseShift(int TargetIndex, double PhaseShiftBy)
        {
            if (TargetIndex < 0)
                throw new ArgumentOutOfRangeException("The target index must be at least 0.");

            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        IOperationPhaseShift Clone()
        {
            OperationPhaseShift cRetVal = new OperationPhaseShift();

            cRetVal.OperationMatrix = this.OperationMatrix.Clone();
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(this.listTargetQubits);

            return (IOperationPhaseShift)cRetVal;
        }


        /// <summary>
        /// Get the amount that this operation phase shifts by.
        /// </summary>
        /// <returns>The amount phase shifted by.</returns>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        double GetPhaseShiftBy()
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }


        /// <summary>
        /// Set the amount that this operation phase shifts by.
        /// </summary>
        /// <param name="PhaseShiftBy">Amount to rotate by.</param>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        void SetPhaseShiftBy(double PhaseShiftBy)
        {
            throw new NotImplementedException(Constants.NOT_IMPLEMENTED_EXCEPTION_MESSAGE);
        }
    }
}
