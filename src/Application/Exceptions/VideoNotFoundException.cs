namespace Application.Exceptions
{
    /// <summary>
    /// Thrown when the requested video does not exist, is private, or is no longer available.
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
        public VideoNotFoundException(string message) : base(message)
        {
        }
    }
}
