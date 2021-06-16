namespace CoveX.ClassicalUtilities
{
    /// <summary>
    /// Exception that is thrown when trying to apply operations to matrices
    /// whose sizes mismatch for the operation.
    /// </summary>
    /// <example>Trying to add a 2 x 2 matrix to a 3 x 3 matrix</example>
    public class MatrixSizeMismatchException : CoveX.Base.CoveException
    {
        /// <summary>
        /// Default constructor for the exception
        /// </summary>
        public MatrixSizeMismatchException()
            : base()
        { }


        /// <summary>
        /// Constructor to set the message of the exception at time of construction
        /// </summary>
        /// <param name="Message">The message of the exception</param>
        public MatrixSizeMismatchException(string Message)
            : base(Message)
        {
        }
    }
}
