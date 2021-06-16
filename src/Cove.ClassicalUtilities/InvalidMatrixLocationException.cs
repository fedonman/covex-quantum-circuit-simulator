namespace CoveX.ClassicalUtilities
{
    /// <summary>
    /// The exception thrown when specifying a location in the matrix that does not exist.
    /// An example would be specifying row 3, column 7 in a 2 x 2 matrix.
    /// </summary>
    /// <example>Specifying row 5, column 3 in a 2 x 2 matrix</example>
    public class InvalidMatrixLocationException : CoveX.Base.CoveException
    {
        /// <summary>
        /// Default constructor for the exception
        /// </summary>
        public InvalidMatrixLocationException()
            : base()
        {
        }


        /// <summary>
        /// Constructor to set the message of the exception at time of construction
        /// </summary>
        /// <param name="Message">The message of the exception</param>
        public InvalidMatrixLocationException(string Message) : base(Message)
        {
            //this.Message = Message;
        }

    }
}
