using Application.ViewModels.Validation;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class TranscriptRequestViewModel
    {
        [Required(ErrorMessage = "Please paste a YouTube video URL.")]
        [Url(ErrorMessage = "That doesn't look like a valid URL.")]
        [YoutubeUrl]
        public string Url { get; set; } = string.Empty;
    }
}
