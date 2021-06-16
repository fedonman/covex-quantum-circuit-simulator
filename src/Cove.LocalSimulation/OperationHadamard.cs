using CoveX.Base;
using CoveX.ClassicalUtilities;
using System;

namespace CoveX.LocalSimulation
{
    /// <summary>
    /// The Hadamard operation.
    /// </summary>
    /// <remarks>
    /// The set (OperationHadamard, OperationTGate) is universal for 1-qubit gates
    /// [1]	P. Kayne, R. Laflamme, and M. Mosca, An Introduction to Quantum Computing. 
    /// New York City, New York: Oxford University Press, 2007.
    /// </remarks>
    public class OperationHadamard : GeneralSimulatedQubitOperation, IOperationHadamard
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public OperationHadamard() : base()
        {
            this.OperationMatrix = (1.0 / Constants.SQUARE_ROOT_OF_2) * (new ComplexMatrix(new Complex[,] {
                {new Complex(1), new Complex(1)},
                {new Complex(1), new Complex(-1)}}));
        }


        /// <summary>
        /// Overloaded constructor to set the target index as well.
        /// </summary>
        /// <param name="iTargetIndex">The target index</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// is less than 0.</exception>
        public OperationHadamard(int iTargetIndex) : base()
        {
            this.SetTargetQubit(iTargetIndex);
        }


        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IOperationHadamard Clone()
        {
            OperationHadamard cRetVal = new OperationHadamard();

            cRetVal.OperationMatrix = this.OperationMatrix.Clone();
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(this.listTargetQubits);

            return (IOperationHadamard)cRetVal;
        }
    }
}
