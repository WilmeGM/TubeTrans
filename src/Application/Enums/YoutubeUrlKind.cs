namespace Application.Enums
{
    /// <summary>
    /// Classifies a user-supplied URL by the kind of YouTube content it points to.
    /// </summary>
    public enum YoutubeUrlKind
    {
        /// <summary>The value is empty, malformed, or does not point to YouTube.</summary>
        NotYoutube,

        /// <summary>A YouTube Shorts link (<c>/shorts/{id}</c>). Recognized, but not supported.</summary>
        Shorts,

        /// <summary>A regular YouTube video link (watch, embed or youtu.be).</summary>
        Video
    }
}
