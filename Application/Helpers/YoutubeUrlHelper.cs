using Application.Enums;
using System.Text.RegularExpressions;

namespace Application.Helpers
{
    internal class YoutubeUrlHelper
    {
        private static readonly Regex VideoIdPattern = new(
            @"^(?:https?:\/\/)?(?:www\.|m\.)?(?:youtube\.com\/(?:watch\?v=|embed\/|v\/)|youtu\.be\/)([a-zA-Z0-9_-]{11})",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex ShortsIdPattern = new(
            @"^(?:https?:\/\/)?(?:www\.|m\.)?youtube\.com\/shorts\/([a-zA-Z0-9_-]{11})",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static YoutubeUrlKind Classify(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return YoutubeUrlKind.NotYoutube;

            if (VideoIdPattern.IsMatch(url))
                return YoutubeUrlKind.Video;

            if (ShortsIdPattern.IsMatch(url))
                return YoutubeUrlKind.Shorts;

            return YoutubeUrlKind.NotYoutube;
        }

        public static bool IsYoutubeUrl(string? url) => Classify(url) == YoutubeUrlKind.Video;

        public static string? ExtractVideoId(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            var match = VideoIdPattern.Match(url);
            if (match.Success)
                return match.Groups[1].Value;

            match = ShortsIdPattern.Match(url);
            return match.Success ? match.Groups[1].Value : null;
        }
    }
}
