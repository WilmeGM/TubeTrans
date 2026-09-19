using Application.Enums;
using Application.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.Validation
{
    public class YoutubeUrlAttribute : ValidationAttribute
    {
        public YoutubeUrlAttribute() { }

        private const string ShortsMessage =
            "Shorts are not supported yet. Please paste a regular YouTube video link.";
        private const string InvalidMessage =
            "That doesn't look like a valid YouTube video link.";

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