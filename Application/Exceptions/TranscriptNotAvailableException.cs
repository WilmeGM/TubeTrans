namespace Application.Exceptions
{
    /// <summary>
    /// Thrown when a video exists but offers no caption tracks to build a transcript from.
    /// </summary>
    public class TranscriptNotAvailableException : TubeTransException
    {
        /// <summary>
        /// Initializes a new instance with a user-facing message.
        /// </summary>
        /// <param name="message">A message that is safe to display to the end user.</param>
        public TranscriptNotAvailableException (string message) : base(message) { }
    }
}
