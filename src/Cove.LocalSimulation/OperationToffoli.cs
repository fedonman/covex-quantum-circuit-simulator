using CoveX.Base;
using CoveX.ClassicalUtilities;
using System;


namespace CoveX.LocalSimulation
{
    /// <summary>
    /// This class represents the Toffoli operation. This is also known as the double controlled
    /// not or controlled-controlled not. If the two control qubits are |1> then the single
    /// target qubit is flipped. If not, then nothing happens.
    /// </summary>
    public class OperationToffoli : GeneralSimulatedOperation, IOperationToffoli
    {
        /// <summary>
        /// The index in listTargetQubits that is the first control qubit.
        /// </summary>
        protected const int FIRST_CONTROL_INDEX = 2;

        /// <summary>
        /// The index in listTargetQubits that is the second control qubit.
        /// </summary>
        protected const int SECOND_CONTROL_INDEX = 1;

        /// <summary>
        /// The index in listTargetQubits that is the target qubit.
        /// </summary>
        protected const int TARGET_INDEX = 0;


        /// <summary>
        /// The default constructor.
        /// </summary>
        public OperationToffoli()
        {
            //NOTE: This is the standard op and flipped for now.
            this.OperationMatrix = new ComplexMatrix(new Complex[,] {
                {1, 0, 0, 0, 0, 0, 0, 0},
                {0, 1, 0, 0, 0, 0, 0, 0},
                {0, 0, 1, 0, 0, 0, 0, 0},
                {0, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 0, 0, 1, 0, 0, 0},
                {0, 0, 0, 0, 0, 1, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0, 1, 0}
            });

            this.listTargetQubits = new System.Collections.Generic.List<int>(new int[] { 2, 1, 0 });
            //this.listTargetQubits = new System.Collections.Generic.List<int>(new int[] { 0, 1, 2 });
        }


        /// <param name="FirstControlIndex">The index of the first control qubit.</param>
        /// <param name="SecondControlIndex">The index of the second control qubit.</param>
        /// <param name="TargetIndex">The index of the target qubit. The Not operation
        /// will be applied to this qubit if the control indexes are |1>.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if any
        /// of the parameters are less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the same index is specified
        /// for more than one parameter.</exception>
        public OperationToffoli(int FirstControlIndex, int SecondControlIndex, int TargetIndex) : this()
        {
            this.SetIndexes(FirstControlIndex, SecondControlIndex, TargetIndex);
        }


        /// <summary>
        /// Return a clone (deep copy) of the current operation. This operation returned can be modified
        /// without any impact to this object. Unlike quantum registers, quantum operations cannot be 
        /// in superposition- hence a clone of them does not violate the no-cloning theorem.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IOperationToffoli Clone()
        {
            OperationToffoli cRetVal = new OperationToffoli();

            cRetVal.OperationMatrix = this.OperationMatrix.Clone();
            cRetVal.listTargetQubits = new System.Collections.Generic.List<int>(this.listTargetQubits);

            return (IOperationToffoli)cRetVal;
        }


        /// <summary>
        /// Get the index of the target qubit when this operation is applied.
        /// </summary>
        /// <returns>The index of the target qubit in the register when this operation
        /// is applied.</returns>
        public int GetTargetIndex()
        {
            return this.listTargetQubits[TARGET_INDEX];
        }


        /// <summary>
        /// Get the index of the first control qubit in the register when this operation is applied.
        /// </summary>
        /// <returns>The index of the first control qubit in the register when this operation is
        /// applied.</returns>
        public int GetFirstControlIndex()
        {
            return this.listTargetQubits[FIRST_CONTROL_INDEX];
        }


        /// <summary>
        /// Get the index of the second control qubit in the register when this operation is applied.
        /// </summary>
        /// <returns>The index of the second control qubit in the register when this operation is
        /// applied.</returns>
        public int GetSecondControlIndex()
        {
            return this.listTargetQubits[SECOND_CONTROL_INDEX];
        }


        /// <summary>
        /// Set the indexes of qubits in a register when this operation is applied.
        /// </summary>
        /// <param name="FirstControlIndex">The index of the first control qubit.</param>
        /// <param name="SecondControlIndex">The index of the second control qubit.</param>
        /// <param name="TargetIndex">The index of the target qubit. The Not operation
        /// will be applied to this qubit if the control indexes are |1>.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if any
        /// of the parameters are less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the same index is specified
        /// for more than one parameter.</exception>
        public void SetIndexes(int FirstControlIndex, int SecondControlIndex, int TargetIndex)
        {
            //validate input
            if ((FirstControlIndex < 0) || (SecondControlIndex < 0) || (TargetIndex < 0))
                throw new ArgumentOutOfRangeException(string.Format("All parameters to OperationToffoli.SetIndexes() must be greater than or equal to 0. FirstControlIndex = {0}, SecondControlIndex = {1}, TargetIndex = {2}", FirstControlIndex, SecondControlIndex, TargetIndex));
            if ((FirstControlIndex == SecondControlIndex) || (FirstControlIndex == TargetIndex) || (SecondControlIndex == TargetIndex))
                throw new DuplicateIndexesException(string.Format("The indexes passed to OperationToffoli.SetIndexes() must unique, they cannot be the same value. FirstControlIndex = {0}, SecondControlIndex = {1}, TargetIndex = {2}", FirstControlIndex, SecondControlIndex, TargetIndex));

            this.listTargetQubits[FIRST_CONTROL_INDEX] = FirstControlIndex;
            this.listTargetQubits[SECOND_CONTROL_INDEX] = SecondControlIndex;
            this.listTargetQubits[TARGET_INDEX] = TargetIndex;
        }


        /// <summary>
        /// Set the first control index.
        /// </summary>
        /// <param name="FirstControlIndex">The first control index.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the
        /// index is less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the index
        /// matches one of the other targets.</exception>
        public void SetFirstControlIndex(int FirstControlIndex)
        {
            this.SetIndexes(FirstControlIndex, this.GetSecondControlIndex(), this.GetTargetIndex());
        }


        /// <summary>
        /// Set the second control index.
        /// </summary>
        /// <param name="SecondControlIndex">The second control index.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the
        /// index is less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the index
        /// matches one of the other targets.</exception>
        public void SetSecondControlIndex(int SecondControlIndex)
        {
            this.SetIndexes(this.GetFirstControlIndex(), SecondControlIndex, this.GetTargetIndex());
        }


        /// <summary>
        /// Set the target index.
        /// </summary>
        /// <param name="TargetIndex">The target index.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the
        /// index is less than 0.</exception>
        /// <exception cref="DuplicateIndexesException">Thrown if the index
        /// matches one of the other targets.</exception>
        public void SetTargetlIndex(int TargetIndex)
        {
            this.SetIndexes(this.GetFirstControlIndex(), this.GetSecondControlIndex(), TargetIndex);
        }


        /// <summary>
        /// Get a string that shows the operations name and target. An example might
        /// be something like "CNOT: Control = 0, Target = 2".
        /// </summary>
        /// <returns>A string representing the operation and what it targets</returns>
        public override string GetOperationAndTargets()
        {
            return string.Format("{0}: Control1 = {1}, Control2 = {2}, Target = {3}", System.Reflection.MethodBase.GetCurrentMethod().Name,
                this.listTargetQubits[FIRST_CONTROL_INDEX], this.listTargetQubits[SECOND_CONTROL_INDEX],
                this.listTargetQubits[TARGET_INDEX]);
        }
    }
}
