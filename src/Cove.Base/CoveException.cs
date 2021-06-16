namespace CoveX.Base
{
    /// <summary>
    /// The base class for all exceptions in Cove
    /// </summary>
    public class CoveException : System.Exception, ICoveObject
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CoveException() : base()
        {
        }


        /// <summary>
        /// Constructor that also sets the message exception
        /// </summary>
        /// <param name="Message">The message of the exception</param>
        public CoveException(string Message)
            : base(Message)
        {
        }

    }
}
