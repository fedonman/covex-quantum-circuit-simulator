using CoveX.Base;
using CoveX.ClassicalUtilities;
using System;

namespace CoveX.LocalSimulation
{
    /// <summary>
    /// Arbitrary rotation, a more general S or T gate. Used by the quantum Fourier transform
    /// as the target of control operations
    /// </summary>
    public class OperationRotateK : GeneralSimulatedQubitOperation, IOperationRotateK
    {
        /// <summary>
        /// The current K value
        /// </summary>
        protected double dK = 0.0;


        /// <summary>
        /// Default constructor, construct an instance of the Rotate K operation. The
        /// default K value is 0.
        /// </summary>
        public OperationRotateK() : this(0)
        {
        }


        /// <summary>
        /// Overloaded constructor to specify the target at time of construction.
        /// </summary>
        /// <param name="TargetIndex">The target index of this operation.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// is less than 0.</exception>
        public OperationRotateK(int TargetIndex) : base()
        {
            this.SetTargetQubit(TargetIndex);
        }


        /// <summary>
        /// Construct an instance of the RotateK operation.
        /// </summary>
        /// <param name="TargetIndex">The target index of this operation.</param>
        /// <param name="K">Initial K value</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target
        /// is less than 0.</exception>
        public OperationRotateK(int TargetIndex, double K) : base()
        {
            this.dK = K;

            this.SetTargetQubit(TargetIndex);
            this.SetK(K);
        }


        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IOperationRotateK Clone()
        {
            OperationRotateK cRetVal = new OperationRotateK();

            cRetVal.OperationMatrix = this.OperationMatrix.Clone();
            cRetVal.dK = this.dK;
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(this.listTargetQubits);

            return (IOperationRotateK)cRetVal;
        }


        /// <summary>
        /// Get the K value of this operation
        /// </summary>
        /// <returns>The current K value.</returns>
        public double GetK()
        {
            return this.dK;
        }


        /// <summary>
        /// Set the K value of this operation.
        /// </summary>
        /// <param name="K">New K value.</param>
        public void SetK(double K)
        {
            double dY = ((2 * Math.PI) / (Math.Pow(2, K)));
            this.dK = K;

            //e^(iy) = cos(y) + i sin(y)
            //The Cos and Sin functions take radians, so this is ok.
            this.OperationMatrix = new ComplexMatrix(new Complex[,] {
                {1, 0},
                {0, new Complex(Math.Cos(dY), Math.Sin(dY))}
            });
        }
    }
}
