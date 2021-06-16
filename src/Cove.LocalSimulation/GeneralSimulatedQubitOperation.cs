using System;
using System.Collections.Generic;

namespace CoveX.LocalSimulation
{
    /// <summary>
    /// A base class for single qubit operations, just wrap the 
    /// target qubit ops into a base class.
    /// </summary>
    public class GeneralSimulatedQubitOperation : GeneralSimulatedOperation
    {
        /// <summary>
        /// Default constructor, set the target index to 0 by default.
        /// </summary>
        public GeneralSimulatedQubitOperation() : base()
        {
            this.listTargetQubits = new List<int>(new int[] { 0 });
        }


        /// <summary>
        /// Get the target index of this single qubit operation.
        /// </summary>
        /// <returns>The current target index.</returns>
        public int GetTargetQubit()
        {
            return this.listTargetQubits[0];
        }


        /// <summary>
        /// Set the target index of this single qubit operation.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// is less than 0.</exception>
        public void SetTargetQubit(int NewTarget)
        {
            if (NewTarget < 0)
                throw new ArgumentOutOfRangeException("The NewTarget index must be greater than or equal to 0.");

            this.listTargetQubits[0] = NewTarget;
        }


        /// <summary>
        /// Get a string that shows the operations name and target. An example might
        /// be something like "CNOT: Control = 0, Target = 2".
        /// </summary>
        /// <returns>A string representing the operation and what it targets</returns>
        public override string GetOperationAndTargets()
        {
            return string.Format("{0}: Target = {1}", System.Reflection.MethodBase.GetCurrentMethod().Name,
                this.listTargetQubits[0]);
        }

    }
}
