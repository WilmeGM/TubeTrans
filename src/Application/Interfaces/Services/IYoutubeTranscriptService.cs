using Application.Models;

namespace Application.Interfaces.Services
{
    /// <summary>
    /// Retrieves the transcript of a YouTube video.
    /// </summary>
    /// <remarks>
    /// Consumers depend on this abstraction instead of a concrete implementation, which allows
    /// replacing the transcript source and testing the controller with a fake.
    /// </remarks>
    public interface IYoutubeTranscriptService
    {
        /// <summary>
        /// Gets the transcript of a video, in its original language, as plain text.
        /// </summary>
        /// <param name="videoUrl">A YouTube video URL.</param>
        /// <returns>The video title and its consolidated transcript text.</returns>
        /// <exception cref="InvalidOperationException">The URL does not contain a recognizable video ID.</exception>
        /// <exception cref="Application.Exceptions.VideoNotFoundException">The video does not exist, is private, or is no longer available.</exception>
        /// <exception cref="Application.Exceptions.TranscriptNotAvailableException">The video has no captions.</exception>
        /// <remarks>
        /// Other failures, such as network errors or rate limiting by YouTube, are not translated
        /// and propagate as-is. Callers must be prepared to handle them.
        /// </remarks>
        Task<VideoTranscript> GetTranscriptAsync(string videoUrl);
    }
}
