namespace CoveX.Base
{
    /// <summary>
    /// If a user creates an operation that is not unitary, then throw this
    /// exception since all quantum operators must be unitary.
    /// </summary>
    public class NotUnitaryOperationException : CoveException, ICoveObject
    {
        /// <summary>
        /// Default constructor for the exception
        /// </summary>
        public NotUnitaryOperationException()
            : base()
        {
        }


        /// <summary>
        /// Constructor to set the message of the exception at time 
        /// of construction.
        /// </summary>
        /// <param name="Message">The message of the exception</param>
        public NotUnitaryOperationException(string Message)
            : base(Message)
        {
        }
    }
}
