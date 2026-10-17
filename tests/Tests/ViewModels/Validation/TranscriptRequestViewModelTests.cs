using Application.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Tests.ViewModels.Validation
{
    public class TranscriptRequestViewModelTests
    {
        private static List<ValidationResult> Validate(string url)
        {
            var model = new TranscriptRequestViewModel { Url = url };
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
            return results;
        }

        [Fact]
        public void Validate_EmptyUrl_ReportsOnlyRequiredMessage()
        {
            var result = Assert.Single(Validate(""));
            Assert.Equal("Please paste a YouTube video URL.", result.ErrorMessage);
        }

        [Fact]
        public void Validate_NotAUrl_ReportsUrlFormatMessage()
        {
            var results = Validate("hello");
            Assert.Contains(results, r => r.ErrorMessage == "That doesn't look like a valid URL.");
        }

        [Fact]
        public void Validate_UrlFromAnotherSite_ReportsYoutubeMessage()
        {
            var result = Assert.Single(Validate("https://google.com"));
            Assert.Contains("YouTube", result.ErrorMessage);
        }

        [Fact]
        public void Validate_ShortsUrl_ReportsShortsMessage()
        {
            var result = Assert.Single(Validate("https://www.youtube.com/shorts/ZgdOfz6HWfY"));
            Assert.Contains("Shorts", result.ErrorMessage);
        }

        [Fact]
        public void Validate_RegularVideoUrl_HasNoErrors()
        {
            Assert.Empty(Validate("https://www.youtube.com/watch?v=dQw4w9WgXcQ"));
        }
    }
}
