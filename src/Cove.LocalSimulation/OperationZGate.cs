using CoveX.Base;
using CoveX.ClassicalUtilities;
using System;

namespace CoveX.LocalSimulation
{
    /// <summary>
    /// The Pauli Z gate
    /// </summary>
    public class OperationZGate : GeneralSimulatedQubitOperation, IOperationZGate
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public OperationZGate() : base()
        {
            OperationMatrix = new ComplexMatrix(new Complex[,] {
                { new Complex(1), new Complex(0) },
                { new Complex(0), new Complex(-1) } });
        }


        /// <summary>
        /// Overloaded constructor to specify the target at time of construction.
        /// </summary>
        /// <param name="TargetIndex">The target index of this operation.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// is less than 0.</exception>
        public OperationZGate(int TargetIndex) : this()
        {
            this.SetTargetQubit(TargetIndex);
        }


        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IOperationZGate Clone()
        {
            OperationZGate cRetVal = new OperationZGate();

            cRetVal.OperationMatrix = this.OperationMatrix.Clone();
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(this.listTargetQubits);

            return (IOperationZGate)cRetVal;
        }
    }
}
