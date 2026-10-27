namespace Application.Exceptions
{
    /// <summary>
    /// Base type for the expected, business-level errors of the application.
    /// </summary>
    /// <remarks>
    /// The message of a <see cref="TubeTransException"/> is written by the application and is safe
    /// to show to the user. Any other exception type is treated as unexpected: it is logged and
    /// replaced with a generic message, so internal details are never exposed.
    /// </remarks>
    public abstract class TubeTransException : Exception
    {
        /// <summary>
        /// Initializes a new instance with a user-facing message.
        /// </summary>
        /// <param name="message">A message that is safe to display to the end user.</param>
        /// <param name="innerException">The original exception, kept for diagnostics. It is logged but never shown to the user.</param>
        protected TubeTransException(string message, Exception? innerException = null) : base(message, innerException)
        {
        }
    }
}
