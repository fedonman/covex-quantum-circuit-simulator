using CoveX.Base;
using CoveX.ClassicalUtilities;
using System;
using System.Collections.Generic;


namespace CoveX.LocalSimulation
{
    /// <summary>
    /// The identity operation over n qubits.
    /// </summary>
    public class OperationIdentityN : GeneralSimulatedOperation, IOperationIdentityN
    {
        /// <summary>
        /// The default constructor, construct an identity op for 1 qubit (2 x 2 matrix)
        /// </summary>
        public OperationIdentityN() : this(1)
        {
        }


        /// <summary>
        /// Create an identity operation that operates on the specified number of qubits.
        /// </summary>
        /// <param name="Size">Number of qubits this identity operator will operate on.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if Size is less than
        /// or equal to 0.</exception>
        public OperationIdentityN(int Size)
        {
            this.SetSize(Size);
        }



        /// <summary>
        /// Get the size of this operation, the number of qubits it operates on.
        /// </summary>
        /// <returns>The number of qubits this operation operates on.</returns>
        public int GetSize()
        {
            return this.listTargetQubits.Count;
        }


        /// <summary>
        /// Set the size of this operation, the number of qubits it operates on.
        /// </summary>
        /// <param name="Size">The new size of the opeartion</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if Size is less than
        /// or equal to 0.</exception>
        public void SetSize(int Size)
        {
            //verify input
            if (Size <= 0)
                throw new ArgumentOutOfRangeException("The size of the operation must be greater than 0.");

            //now set the size
            this.OperationMatrix = new ComplexMatrix((int)System.Math.Pow(2, Size));

            this.listTargetQubits = new System.Collections.Generic.List<int>(Size);
            for (int iCurIndex = 0; iCurIndex < Size; iCurIndex++)
                this.listTargetQubits.Add(iCurIndex);
        }


        /// <summary>
        /// Get the target indexes for this operation.
        /// </summary>
        /// <returns>The target indexes for this operation.</returns>
        public int[] GetTargets()
        {
            //simple type, so should be a deep copy
            return this.listTargetQubits.ToArray();
        }


        /// <summary>
        /// Set the target indexes for this operation. This also resizes
        /// the operation to match the number of indexes passed in.
        /// </summary>
        /// <param name="SetTargets">Target indexes of the operation</param>
        /// <exception cref="ArgumentNullException">Thrown if null is passed
        /// in.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if any
        /// of the indexes are less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any
        /// of the indexes in SetTargets are duplicates.</exception>
        public void SetTargets(int[] SetTargets)
        {
            Dictionary<int, bool> dictDuplicates = new Dictionary<int, bool>();

            //verify inputs
            if (SetTargets == null)
                throw new ArgumentNullException("Cannot pass a null SetTargets parameter");

            foreach (int iCurTarget in SetTargets)
            {
                if (iCurTarget < 0)
                    throw new ArgumentException("All target indexes must be at least 0, negative indexes are invalid");
                if (dictDuplicates.ContainsKey(iCurTarget) == true)
                    throw new DuplicateIndexesException(string.Format("The index {0} was specified more than once. All indexes must be unique.", iCurTarget));

                dictDuplicates[iCurTarget] = true;
            }

        }


        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IOperationIdentityN Clone()
        {
            OperationIdentityN cRetVal = new OperationIdentityN();

            cRetVal.OperationMatrix = this.OperationMatrix.Clone();
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(this.listTargetQubits);

            return (IOperationIdentityN)cRetVal;
        }


        /// <summary>
        /// Get a string that shows the operations name and target. An example might
        /// be something like "CNOT: Control = 0, Target = 2".
        /// </summary>
        /// <returns>A string representing the operation and what it targets</returns>
        public override string GetOperationAndTargets()
        {
            System.Text.StringBuilder sbRetVal = new System.Text.StringBuilder();

            sbRetVal.Append(string.Format("{0}: Targets = ", System.Reflection.MethodBase.GetCurrentMethod().Name));
            foreach (int iCurTarget in this.listTargetQubits)
            {
                sbRetVal.Append(iCurTarget.ToString() + ", ");
            }

            return sbRetVal.ToString();
        }
    }
}
