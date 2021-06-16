namespace CoveX.Base
{
    /// <summary>
    /// Thrown if duplicate indexes are specified as a parameter.
    /// </summary>
    /// <example>The same qubit is specified as the control and target of a CNot 
    /// operation.</example>
    public class DuplicateIndexesException : CoveException
    {
        /// <summary>
        /// Default constructor for the exception
        /// </summary>
        public DuplicateIndexesException()
            : base()
        {
        }


        /// <summary>
        /// Constructor to set the message of the exception at time 
        /// of construction.
        /// </summary>
        /// <param name="Message">The message of the exception</param>
        public DuplicateIndexesException(string Message)
            : base(Message)
        {
        }
    }
}
