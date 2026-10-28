namespace Application.Exceptions
{
    /// <summary>
    /// Thrown when the video cannot be accessed: it does not exist, is private, is no longer available, or YouTube 
    /// refuses to serve it to the server (for example, with a bot check).
    /// </summary>
    /// <remarks>
    /// The service translates <see cref="YoutubeExplode.Exceptions.VideoUnavailableException"/>
    /// into this type, so callers never depend on the third-party library.
    /// </remarks>
    public class VideoNotFoundException : TubeTransException
    {
        /// <summary>
        /// Initializes a new instance with a user-facing message.
        /// </summary>
        /// <param name="message">A message that is safe to display to the end user.</param>
        /// <param name="innerException">The original exception, kept for diagnostics. It is logged but never shown to the user.</param>
        public VideoNotFoundException(string message, Exception? innerException = null) : base(message, innerException)
        {
        }
    }
}
