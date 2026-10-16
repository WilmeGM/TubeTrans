namespace Application.Models
{
    /// <summary>
    /// Domain model: the result of a successful transcript lookup.
    /// </summary>
    /// <remarks>
    /// Produced by the service layer, which knows nothing about how it will be displayed.
    /// The controller maps it to a <see cref="Application.ViewModels.TranscriptResponseViewModel"/>,
    /// keeping presentation concerns out of the service.
    /// </remarks>
    public class VideoTranscript
    {
        /// <summary>The title of the YouTube video.</summary>
        public string VideoTitle { get; set; } = string.Empty;

        /// <summary>The full transcript as one continuous block of plain text, without timestamps.</summary>
        public string Text { get; set; } = string.Empty;
    }
}
