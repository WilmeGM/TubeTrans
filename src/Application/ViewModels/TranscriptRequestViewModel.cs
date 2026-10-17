using Application.ViewModels.Validation;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{

    /// <summary>
    /// Input of the transcript form: the YouTube link pasted by the user.
    /// </summary>
    public class TranscriptRequestViewModel
    {
        /// <summary>
        /// The YouTube video URL submitted by the user.
        /// </summary>
        /// <remarks>
        /// Validated in layers, from generic to specific: it must be present, be a well-formed URL,
        /// and be a regular YouTube video link. When the value is empty, only the first message is reported.
        /// </remarks>
        [Required(ErrorMessage = "Please paste a YouTube video URL.")]
        [Url(ErrorMessage = "That doesn't look like a valid URL.")]
        [YoutubeUrl]
        public string Url { get; set; } = string.Empty;
    }
}
