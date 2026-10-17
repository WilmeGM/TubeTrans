using Application.Enums;
using System.Text.RegularExpressions;

namespace Application.Helpers
{
    /// <summary>
    /// Recognizes YouTube URLs and extracts the video ID from them.
    /// </summary>
    /// <remarks>
    /// Pure string logic: it never touches the network, so it cannot tell whether a video exists.
    /// Recognized regular-video formats are <c>watch?v=</c>, <c>embed/</c>, <c>v/</c> and <c>youtu.be/</c>,
    /// on <c>youtube.com</c>, <c>www.</c> or <c>m.</c> hosts, with or without the scheme.
    /// The input is not trimmed.
    /// </remarks>
    public static class YoutubeUrlHelper
    {
        // Regular video links, for example:
        //   https://www.youtube.com/watch?v=ID
        //   https://www.youtube.com/watch?feature=shared&v=ID   (v= may follow other parameters)
        //   https://youtu.be/ID
        //   https://www.youtube.com/embed/ID
        // Anchored with ^ so look-alike hosts (notyoutube.com, youtube.com.evil.com) do not match.
        // A video ID is exactly 11 characters: letters, digits, '_' or '-'. Group 1 captures it.
        // Built once as a static field, so the pattern is compiled a single time.
        private static readonly Regex VideoIdPattern = new(
            @"^(?:https?:\/\/)?(?:www\.|m\.)?(?:youtube\.com\/(?:watch\?(?:[^#\s]*&)?v=|embed\/|v\/)|youtu\.be\/)([a-zA-Z0-9_-]{11})",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Shorts links: https://www.youtube.com/shorts/ID. Same ID rules as above.
        private static readonly Regex ShortsIdPattern = new(
            @"^(?:https?:\/\/)?(?:www\.|m\.)?youtube\.com\/shorts\/([a-zA-Z0-9_-]{11})",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Determines what kind of YouTube content a URL points to, if any.
        /// </summary>
        /// <param name="url">The URL to classify. May be <c>null</c> or empty.</param>
        /// <returns>
        /// <see cref="YoutubeUrlKind.Video"/> for regular videos, <see cref="YoutubeUrlKind.Shorts"/> for
        /// Shorts, and <see cref="YoutubeUrlKind.NotYoutube"/> for anything else, including empty values.
        /// </returns>
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

        /// <summary>
        /// Indicates whether a URL is a regular, supported YouTube video link.
        /// </summary>
        /// <param name="url">The URL to check. May be <c>null</c> or empty.</param>
        /// <returns><c>true</c> only for regular videos. Shorts links return <c>false</c>.</returns>
        public static bool IsYoutubeUrl(string? url) => Classify(url) == YoutubeUrlKind.Video;

        /// <summary>
        /// Extracts the 11-character video ID from a URL.
        /// </summary>
        /// <param name="url">The URL to read. May be <c>null</c> or empty.</param>
        /// <returns>The video ID, or <c>null</c> if the URL is not recognized.</returns>
        /// <remarks>
        /// IDs are also extracted from Shorts links, even though Shorts are not supported. Callers that
        /// need to reject them must check <see cref="Classify(string?)"/> first.
        /// </remarks>
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
