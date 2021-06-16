namespace CoveX.Base
{
    /// <summary>
    /// This exception is thrown when the implementation is incomplete or
    /// reaches an unexpected state. Ideally this exception should never 
    /// occur. This is different than <see cref="QuantumHardwareException"/>,
    /// which is the base class for any hardware problems. This class
    /// is for problems with the implementation.
    /// </summary>
    public class ImplementationException : CoveException
    {
        /// <summary>
        /// Default constructor for the exception
        /// </summary>
        public ImplementationException()
            : base()
        {
        }


        /// <summary>
        /// Constructor to set the message of the exception at time 
        /// of construction.
        /// </summary>
        /// <param name="Message">The message of the exception</param>
        public ImplementationException(string Message)
            : base(Message)
        {
            //this.Message = Message;
        }

    }
}
