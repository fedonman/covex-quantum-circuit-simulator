using CoveX.Base;
using CoveX.ClassicalUtilities;
using System;

namespace CoveX.LocalSimulation
{
    /// <summary>
    /// This operation resets a qubit to 0. This is not reversible unless
    /// bundled as a control in special cases, such as controlled reset in 
    /// the modular adder for Shor's.
    /// </summary>
    public class OperationReset : GeneralSimulatedQubitOperation, IQubitOperation
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public OperationReset() : base()
        {
            this.OperationMatrix = new ComplexMatrix(new Complex[,] {
                {1, 1},
                {0, 0}
            });

            this.listTargetQubits = new System.Collections.Generic.List<int>(new int[] { 0 });
        }


        /// <summary>Overloaded constructor to set the target qubit.</summary>
        /// <param name="TargetIndex">The target index of the operation.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// is less than 0.</exception>
        public OperationReset(int TargetIndex) : this()
        {
            this.SetTargetQubit(TargetIndex);
        }


        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IOperationReset Clone()
        {
            OperationReset cRetVal = new OperationReset();

            cRetVal.OperationMatrix = this.OperationMatrix.Clone();
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(this.listTargetQubits);

            return (IOperationReset)cRetVal;
        }
    }
}
