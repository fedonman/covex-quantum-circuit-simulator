using CoveX.Base;
using CoveX.ClassicalUtilities;
using System;

namespace CoveX.LocalSimulation
{
    /// <summary>
    /// The Pauli Y Gate
    /// </summary>
    public class OperationYGate : GeneralSimulatedQubitOperation, IOperationYGate
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public OperationYGate() : base()
        {
            this.OperationMatrix = new ComplexMatrix(new Complex[,] {
                { new Complex(0), new Complex(0, -1) },
                { new Complex(0, 1), new Complex(0) } });
        }


        /// <summary>
        /// Overloaded constructor to specify the target at time of construction.
        /// </summary>
        /// <param name="TargetIndex">The target index of this operation.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// is less than 0.</exception>
        public OperationYGate(int TargetIndex) : this()
        {
            this.SetTargetQubit(TargetIndex);
        }


        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IOperationYGate Clone()
        {
            OperationYGate cRetVal = new OperationYGate();

            cRetVal.OperationMatrix = this.OperationMatrix.Clone();
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(this.listTargetQubits);

            return (IOperationYGate)cRetVal;
        }
    }
}
