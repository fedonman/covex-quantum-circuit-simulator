namespace CoveX.Base
{
    /// <summary>
    /// This is the base class for any exception caused by the quantum hardware.
    /// This exception is different from ImplementationException, which is the base
    /// class for exceptions due to incorrect or incomplete implementations and not
    /// hardware problems.
    /// </summary>
    public class QuantumHardwareException : CoveException, ICoveObject
    {
        /// <summary>
        /// Default constructor for the exception
        /// </summary>
        public QuantumHardwareException()
            : base()
        {
        }


        /// <summary>
        /// Constructor to set the message of the exception at time 
        /// of construction.
        /// </summary>
        /// <param name="Message">The message of the exception</param>
        public QuantumHardwareException(string Message)
            : base(Message)
        {
        }
    }
}
