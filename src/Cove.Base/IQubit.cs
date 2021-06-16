
namespace CoveX.Base
{

    /// <summary>
    /// Interface definition of a qubit. Direct implementations of this interface should be 
    /// named Qubit.
    /// </summary>
    public interface IQubit : ICoveObject
    {
        /// <summary>
        /// Returns the location of the quantum resource
        /// </summary>
        /// <returns>
        /// The location of the quantum resource
        /// </returns>
        string GetLocation();

        /// <summary>
        /// Set the location of the quantum resource
        /// </summary>
        /// <param name="Location">
        /// The location of the quantum resource</param>
        void SetLocation(string Location);


        /// <summary>
        /// Measure the qubit- collapses it absolutely to
        /// |0> xor |1>
        /// </summary>
        /// <returns>The value of the qubit after measurement-
        /// 0 xor 1.</returns>
        int Measure();


        /// <summary>
        /// Measure the qubit- collapses it absolutely to |0> xor |1>. 
        /// Instead of returning 0 or 1 like Measure() it returns the 
        /// string label of the result.
        /// </summary>
        /// <returns>The string label of what the qubit collapsed to.
        /// </returns>
        string MeasureWithLabel();


        /// <summary>
        /// Apply the specified operation to the qubit
        /// </summary>
        ///<param name="Operation">The operation to apply</param>
        void ApplyOperation(IQubitOperation Operation);


        /// <summary>
        /// Apply mulitiple operations to a qubit
        /// </summary>
        /// <param name="Operations">Operations to apply</param>
        void ApplyOperations(IQubitOperation[] Operations);


        /// <summary>
        /// Get the representation of one
        /// </summary>
        /// <returns>The representation of one</returns>
        string GetLabelOne();


        /// <summary>
        /// Get the representation of zero
        /// </summary>
        /// <returns>The representatio of zero</returns>
        string GetLabelZero();


        /// <summary>
        /// Perform the Hadamard operation on the qubit. This operation is
        /// also known as Hadamard-Walsh and the square root of not.
        /// </summary>
        void OperationHadamard();


        /// <summary>
        /// Perform the identity operation on the qubit. This does not 
        /// change the state of the qubit.
        /// </summary>
        void OperationIdentity();


        /// <summary>
        /// Perform the Not operation on the qubit. This operation is also
        /// known as the X gate.
        /// </summary>
        void OperationNot();


        /// <summary>
        /// Perform the S gate operation, the phase gate, on the qubit.
        /// </summary>
        void OperationSGate();


        /// <summary>
        /// Perform the T gate operation, the pi/8 phase gate, on
        /// the qubit.
        /// </summary>
        void OperationTGate();


        /// <summary>
        /// Perform the Y Gate operation on the qubit
        /// </summary>
        void OperationYGate();


        /// <summary>
        /// Perform the Z Gate operation on the qubit.
        /// </summary>
        void OperationZGate();


        /// <summary>
        /// Reset the qubit to an arbitrary state
        /// </summary>
        /// <param name="Zero">Value of zero</param>
        /// <param name="One">Value of one</param>
        void ResetTo(object Zero, object One);

        /// <summary>
        /// Reset the qubit to |0>
        /// </summary>
        void ResetToZero();


        /// <summary>
        /// Reset the qubit to |1>
        /// </summary>
        void ResetToOne();


        /// <summary>
        /// Set the label for one
        /// </summary>
        /// <param name="LabelOne">The desired label for one</param>
        void SetLabelOne(string LabelOne);


        /// <summary>
        /// Set both the label for |0> and label for |1> in one call
        /// </summary>
        /// <param name="LabelZero">The label of zero</param>
        /// <param name="LabelOne">The label of one</param>
        void SetLabels(string LabelZero, string LabelOne);


        /// <summary>
        /// Set the label for zero
        /// </summary>
        /// <param name="LabelZero">The desired label for zero</param>
        void SetLabelZero(string LabelZero);


        /// <summary>
        /// Get the string representation with the labels instead of 
        /// |0> and |1>
        /// </summary>
        /// <returns>The string representation of the qubit</returns>
        string ToStringWithLabels();

    }                                            //end of interface
}
