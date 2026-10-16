using Application.Exceptions;
using Application.Helpers;
using Application.Interfaces.Services;
using Application.Models;
using YoutubeExplode;
using YoutubeExplode.Exceptions;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.ClosedCaptions;

namespace Application.Services
{
    /// <summary>
    /// Retrieves YouTube transcripts through YoutubeExplode.
    /// </summary>
    /// <remarks>
    /// This is the only class in the application that talks to YoutubeExplode, so replacing that
    /// library (for example, if YouTube blocks it) means changing this class and nothing else.
    /// <para>
    /// <see cref="GetTranscriptAsync(string)"/> is the single public entry point. It works in these steps:
    /// </para>
    /// <list type="number">
    ///   <item><description>Extracts the video ID from the URL.</description></item>
    ///   <item><description>Fetches the video metadata and the list of caption tracks.</description></item>
    ///   <item><description>Picks the track that most likely matches the original language (see <see cref="CaptionTrackSelector"/>).</description></item>
    ///   <item><description>Downloads that track and joins all its lines into one block of plain text.</description></item>
    /// </list>
    /// <para>
    /// The service only needs an extractable video ID. Rejecting unsupported links, such as Shorts, is the
    /// job of the request validation that runs before this class is called.
    /// Nothing is stored: every call is independent and keeps no state between requests.
    /// </para>
    /// </remarks>
    public class YoutubeTranscriptService : IYoutubeTranscriptService
    {
        private readonly YoutubeClient _youtubeClient;

        /// <summary>
        /// Initializes a new instance of the service.
        /// </summary>
        /// <param name="youtubeClient">The YoutubeExplode client, supplied by dependency injection.</param>
        public YoutubeTranscriptService(YoutubeClient youtubeClient) => _youtubeClient = youtubeClient;

        /// <summary>
        /// Extracts the video ID from a URL, or fails if there is none.
        /// </summary>
        /// <exception cref="InvalidOperationException">The URL does not contain a recognizable video ID.</exception>
        private string ExtractVideoId(string videoUrl)
        {
            var videoId = YoutubeUrlHelper.ExtractVideoId(videoUrl);

            if (string.IsNullOrWhiteSpace(videoId))
            {
                throw new InvalidOperationException("Could not extract a video ID from the provided URL.");
            }

            return videoId;
        }

        /// <summary>
        /// Lists every caption track the video offers, as reported by YouTube.
        /// </summary>
        private async Task<IReadOnlyList<ClosedCaptionTrackInfo>> GetAvailableCaptionTracksAsync(string videoId)
        {
            var trackManifest = await _youtubeClient.Videos.ClosedCaptions.GetManifestAsync(videoId);
            return trackManifest.Tracks;
        }

        /// <summary>
        /// Gets the track chosen by <see cref="CaptionTrackSelector"/>, or <c>null</c> if the video has no captions.
        /// </summary>
        private async Task<ClosedCaptionTrackInfo?> GetPrimaryCaptionTrackAsync(string videoId)
        {
            var tracks = await GetAvailableCaptionTracksAsync(videoId);
            return CaptionTrackSelector.SelectPrimary(tracks);
        }

        /// <inheritdoc />
        public async Task<VideoTranscript> GetTranscriptAsync(string videoUrl)
        {
            var videoId = ExtractVideoId(videoUrl);

            Video video;
            ClosedCaptionTrackInfo? primaryTrack;

            // Both calls ask YouTube about the same video, and either one can fail with
            // VideoUnavailableException. That library-specific exception is translated into our own
            // domain exception, so no caller depends on YoutubeExplode. Any other failure (network
            // errors, rate limiting) is deliberately left to propagate.
            try
            {
                video = await _youtubeClient.Videos.GetAsync(videoId);
                primaryTrack = await GetPrimaryCaptionTrackAsync(videoId);
            }
            catch (VideoUnavailableException)
            {
                throw new VideoNotFoundException("This video doesn't exist, is private, or is no longer available.");
            }

            if (primaryTrack is null)
            {
                throw new TranscriptNotAvailableException("This video doesn't have any captions available.");
            }

            var captionTrack = await _youtubeClient.Videos.ClosedCaptions.GetAsync(primaryTrack);

            // Each caption line can contain line breaks; replacing them keeps the result as one
            // continuous block of text. Timestamps are dropped on purpose: only the text is needed.
            var text = string.Join(" ",
                captionTrack.Captions.Select(c => c.Text.Replace('\n', ' ').Trim()));

            return new VideoTranscript
            {
                VideoTitle = video.Title,
                Text = text
            };
        }
    }
}