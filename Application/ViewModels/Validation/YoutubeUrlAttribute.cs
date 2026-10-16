using Application.Enums;
using Application.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.Validation
{
    /// <summary>
    /// Validates that a string is a link to a regular YouTube video.
    /// </summary>
    /// <remarks>
    /// Unlike the stock data annotations, the error message is chosen at validation time: Shorts
    /// links get a specific message and anything else gets a generic one. That is why this attribute
    /// overrides the <c>IsValid</c> overload that receives a <see cref="ValidationContext"/>,
    /// instead of setting a fixed <c>ErrorMessage</c>.
    /// Only the shape of the URL is checked. Whether the video actually exists is decided later,
    /// by the transcript service. Null or empty values are rejected too.
    /// </remarks>
    public class YoutubeUrlAttribute : ValidationAttribute
    {
        private const string ShortsMessage =
            "Shorts are not supported yet. Please paste a regular YouTube video link.";
        private const string InvalidMessage =
            "That doesn't look like a valid YouTube video link.";

        /// <summary>
        /// Classifies the value and returns a result that matches the kind of URL found.
        /// </summary>
        /// <param name="value">The value to validate. Expected to be a string.</param>
        /// <param name="validationContext">Identifies the member being validated.</param>
        /// <returns>
        /// <see cref="ValidationResult.Success"/> for a regular video link. Otherwise, a failure bound
        /// to the validated member, with a Shorts-specific or a generic message.
        /// </returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var kind = YoutubeUrlHelper.Classify(value as string);

            return kind switch
            {
                YoutubeUrlKind.Video => ValidationResult.Success,
                YoutubeUrlKind.Shorts => new ValidationResult(ShortsMessage, new[] { validationContext.MemberName! }),
                _ => new ValidationResult(InvalidMessage, new[] { validationContext.MemberName! })
            };
        }
    }
}