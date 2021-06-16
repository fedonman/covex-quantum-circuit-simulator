using CoveX.Base;
using CoveX.ClassicalUtilities;
using System;

namespace CoveX.LocalSimulation
{
    /// <summary>
    /// The T Gate operation
    /// </summary>
    public class OperationTGate : GeneralSimulatedQubitOperation, IOperationTGate
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public OperationTGate() : base()
        {
            Complex Phase = new Complex(0, System.Math.Exp(System.Math.PI / 4.0));
            this.OperationMatrix = new ComplexMatrix(new Complex[,] {
                { new Complex(1), new Complex(0) },
                { new Complex(0), Phase } });
        }


        /// <summary>
        /// Overloaded constructor to specify the target at time of construction.
        /// </summary>
        /// <param name="TargetIndex">The target index of this operation.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// is less than 0.</exception>
        public OperationTGate(int TargetIndex) : this()
        {
            this.SetTargetQubit(TargetIndex);
        }


        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IOperationTGate Clone()
        {
            OperationTGate cRetVal = new OperationTGate();

            cRetVal.OperationMatrix = this.OperationMatrix.Clone();
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(this.listTargetQubits);

            return (IOperationTGate)cRetVal;
        }
    }
}
