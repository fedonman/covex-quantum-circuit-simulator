using CoveX.Base;
using CoveX.ClassicalUtilities;
using System;
using System.Collections.Generic;

namespace CoveX.LocalSimulation
{
    /// <summary>This is the base class for all single qubit operations.</summary>
    public class GeneralSimulatedOperation : IQuantumOperation
    {
        /// <summary>The matrix representing the operation.</summary>
        protected internal ComplexMatrix OperationMatrix;


        /// <summary>
        /// The default constructor
        /// </summary>
        public GeneralSimulatedOperation()
        {
        }


        /// <summary>
        /// Represents the targets for the various qubits the operation
        /// operates on. Index represents the standard target, value
        /// represents the actual target. Example: In a standard CNot
        /// the first (index 0) qubit is the control and the second (index 1)
        /// is the target. The list will be 2 elements with element 0 representing
        /// the control and element 1 representing the target. If the list is
        /// then {3, 0} this means that the control targets index 3 of the target
        /// register and the target of the operation is index 0 of the target
        /// register.
        /// </summary>
        protected internal List<int> listTargetQubits;


        /// <summary>
        /// Get the matrix object that represents this operation. The ComplexMatrix
        /// returned is a deep copy, so changes the object returned do not effect this object.
        /// </summary>
        /// <returns>The object (ComplexMatrix) that represents this operation.</returns>
        public object GetOperationMatrix()
        {
            ComplexMatrix cRetVal = this.GetOperationComplexMatrix();

            return ((object)cRetVal);
        }


        /// <summary>
        /// Return the  object that represents the operation. The result is a deep copy
        /// of the operation matrix.
        /// </summary>
        /// <returns>The ComplexMatrix representing this operation</returns>
        public ComplexMatrix GetOperationComplexMatrix()
        {
            return OperationMatrix.Clone();
        }


        /// <summary>
        /// Return the number of qubits that this operation operates on. The number
        /// of qubits it operates on is the highest target qubit. Example: If a Toffolli
        /// operation targets qubit indexes 4, 2, 6 in a register then this will return
        /// 7 (index 6 is the 7th qubit).
        /// </summary>
        /// <returns></returns>
        public int NumberOfQubitsOperatesOn()
        {
            int iRetVal = 0;

            //guard against empty operations
            if ((this.listTargetQubits == null) || (this.listTargetQubits.Count <= 0))
                return 0;

            //go through and find the highest index
            foreach (int iCurTargetIndex in this.listTargetQubits)
            {
                if (iCurTargetIndex > iRetVal)
                {
                    iRetVal = iCurTargetIndex;
                }
            }

            //the number it operates on is 1 higher than the index
            return (iRetVal + 1);

        }


        /// <summary>
        /// Checks to see if the operation is a valid quantum operation, that it is unitary.
        /// </summary>
        /// <returns>True if it is a valid operation.</returns>
        /// <exception cref="NotImplementedException">This method is not yet
        /// implemented.</exception>
        public bool IsValidOperation()
        {
            throw new NotImplementedException("Not yet implemented");
        }


        /// <summary>
        /// Get the string representation of this operation.
        /// </summary>
        /// <returns>The string representation of this operation.</returns>
        public override string ToString()
        {
            return this.OperationMatrix.ToString();
        }


        /// <summary>
        /// Tensor two operations into one combined one.
        /// </summary>
        /// <param name="Operation">Operation to tensor this one with.</param>
        /// <returns>The tensored operation.</returns>
        /// <exception cref="ArgumentException">Thrown if the Operation parameter does not
        /// derive from GeneralSimulatedOperation.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if any of the targets of the
        /// two operations are the same.</exception>
        public IQuantumOperation Tensor(IQuantumOperation Operation)
        {
            GeneralSimulatedOperation cRetVal = new GeneralSimulatedOperation();
            GeneralSimulatedOperation cCastOp = null;
            Dictionary<int, bool> dictUsedTargets = new Dictionary<int, bool>();

            //verify that the operation is usable by this implementation.
            if (Operation is GeneralSimulatedOperation)
                cCastOp = Operation as GeneralSimulatedOperation;
            else
                throw new ArgumentException("Can only Tensor operations that are both Cove.LocalSimulation.GeneralSimulatedOperation derived objects.");

            //make sure there are no duplicate indexes
            foreach (int iCurTarget in this.listTargetQubits)
            {
                if (dictUsedTargets.ContainsKey(iCurTarget) == true)
                    throw new DuplicateIndexesException(string.Format("The target index {0} is specified more than once in the called operation", iCurTarget));
                else
                    dictUsedTargets[iCurTarget] = true;
            }
            foreach (int iCurTarget in cCastOp.listTargetQubits)
            {
                if (dictUsedTargets.ContainsKey(iCurTarget) == true)
                    throw new DuplicateIndexesException(string.Format("The target index {0} is specified more than once in the passed operation", iCurTarget));
                else
                    dictUsedTargets[iCurTarget] = true;
            }

            //construct and return the return object
            cRetVal.OperationMatrix = (this.OperationMatrix.Clone()).Tensor(cCastOp.OperationMatrix);
            cRetVal.listTargetQubits = new List<int>();
            foreach (int iCurTarget in this.listTargetQubits)
            {
                cRetVal.listTargetQubits.Add(iCurTarget);
            }
            foreach (int iCurTarget in cCastOp.listTargetQubits)
            {
                cRetVal.listTargetQubits.Add(iCurTarget);
            }

            return (IQuantumOperation)cCastOp;
        }


        /// <summary>
        /// Combine two same sized operations into one.
        /// </summary>
        /// <param name="Operation">Operation to combine with this one.</param>
        /// <returns>The combined operation.</returns>
        /// /// <exception cref="ArgumentException">Thrown if the Operation parameter does not
        /// derive from GeneralSimulatedOperation.</exception>
        public IQuantumOperation Combine(IQuantumOperation Operation)
        {
            GeneralSimulatedOperation cRetVal = new GeneralSimulatedOperation();
            GeneralSimulatedOperation cCastOp = null;

            //verify that the operation is usable by this implementation.
            if (Operation is GeneralSimulatedOperation)
                cCastOp = Operation as GeneralSimulatedOperation;
            else
                throw new ArgumentException("Can only Tensor operations that are both Cove.LocalSimulation.GeneralSimulatedOperation derived objects.");

            cRetVal.OperationMatrix = this.OperationMatrix * cCastOp.OperationMatrix;
            return (IQuantumOperation)cCastOp;
        }


        /// <summary>
        /// Get a string that shows the operations name and target. An example might
        /// be something like "CNOT: Control = 0, Target = 2".
        /// </summary>
        /// <returns>A string representing the operation and what it targets</returns>
        public virtual string GetOperationAndTargets()
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