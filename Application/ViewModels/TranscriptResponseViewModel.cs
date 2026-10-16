namespace Application.ViewModels
{
    /// <summary>
    /// Output of the transcript form: either the transcript to display or an error message.
    /// </summary>
    /// <remarks>
    /// Presentation model built by the controller from a <see cref="Application.Models.VideoTranscript"/>,
    /// or from a caught exception. It carries the error state, which the service layer knows nothing about.
    /// </remarks>
    public class TranscriptResponseViewModel
    {
        /// <summary>The title of the video. Empty when <see cref="HasError"/> is <c>true</c>.</summary>
        public string VideoTitle { get; set; } = string.Empty;

        /// <summary>The transcript as plain text. Empty when <see cref="HasError"/> is <c>true</c>.</summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Indicates that the request failed, so <see cref="ErrorMessage"/> must be shown
        /// instead of the transcript.
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>A message that is safe to display to the user. <c>null</c> when <see cref="HasError"/> is <c>false</c>.</summary>
        public string? ErrorMessage { get; set; }
    }
}
