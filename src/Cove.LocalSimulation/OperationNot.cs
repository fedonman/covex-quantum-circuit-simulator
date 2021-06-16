using CoveX.Base;
using CoveX.ClassicalUtilities;
using System;

namespace CoveX.LocalSimulation
{
    /// <summary>
    /// The Not operation. Also known as the Pauli X gate.
    /// </summary>
    public class OperationNot : GeneralSimulatedQubitOperation, IOperationNot
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public OperationNot() : base()
        {
            this.OperationMatrix = new ComplexMatrix(new Complex[,] {
                { new Complex(0), new Complex(1) },
                { new Complex(1), new Complex(0) } });
        }


        /// <summary>
        /// Overloaded constructor to specify the target at time of construction.
        /// </summary>
        /// <param name="TargetIndex">The target index of this operation.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// is less than 0.</exception>
        public OperationNot(int TargetIndex) : this()
        {
            this.SetTargetQubit(TargetIndex);
        }


        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IOperationNot Clone()
        {
            OperationNot cRetVal = new OperationNot();

            cRetVal.OperationMatrix = this.OperationMatrix.Clone();
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(this.listTargetQubits);

            return (IOperationNot)cRetVal;
        }
    }
}
