namespace CoveX.Base
{
    /// <summary>
    /// This exception occurs when there is a size mismatch between a quantum 
    /// operation and register it is being applied to.
    /// </summary>
    /// <example>
    /// A CNot operation applies to two qubits, so if it is applied to a 3 qubit
    /// register than this exception will occur.
    /// </example>
    public class SizeMismatchException : CoveException, ICoveObject
    {
        /// <summary>
        /// Default constructor for the exception
        /// </summary>
        public SizeMismatchException()
            : base()
        {
        }


        /// <summary>
        /// Constructor to set the message of the exception at time 
        /// of construction.
        /// </summary>
        /// <param name="Message">The message of the exception</param>
        public SizeMismatchException(string Message)
            : base(Message)
        {
        }
    }
}
