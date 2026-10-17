using Application.Enums;
using Application.Helpers;

namespace Tests.Helpers
{
    public class YoutubeUrlHelperTests
    {
        [Theory]
        [InlineData("https://www.youtube.com/watch?v=pHdRimeTooE")]
        [InlineData("http://youtube.com/watch?v=pHdRimeTooE")]
        [InlineData("https://m.youtube.com/watch?v=pHdRimeTooE")]
        [InlineData("https://youtu.be/pHdRimeTooE")]
        [InlineData("https://www.youtube.com/embed/pHdRimeTooE")]
        [InlineData("https://www.youtube.com/watch?v=pHdRimeTooE&t=42s")]
        [InlineData("HTTPS://WWW.YOUTUBE.COM/WATCH?V=pHdRimeTooE")]
        public void Classify_RegularVideoUrl_ReturnsVideo(string url)
        {
            Assert.Equal(YoutubeUrlKind.Video, YoutubeUrlHelper.Classify(url));
        }

        [Theory]
        [InlineData("https://www.youtube.com/shorts/ZgdOfz6HWfY")]
        [InlineData("https://youtube.com/shorts/ZgdOfz6HWfY")]
        public void Classify_ShortsUrl_ReturnsShorts(string url)
        {
            Assert.Equal(YoutubeUrlKind.Shorts, YoutubeUrlHelper.Classify(url));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("hello")]
        [InlineData("https://google.com")]
        [InlineData("https://vimeo.com/123456789")]
        [InlineData("https://www.youtube.com/")]
        [InlineData("https://www.youtube.com/watch?v=short")]
        [InlineData("https://notyoutube.com/watch?v=pHdRimeTooE")]
        [InlineData("https://www.youtube.com.evil.com/watch?v=pHdRimeTooE")]
        public void Classify_NonYoutubeOrMalformed_ReturnsNotYoutube(string? url)
        {
            Assert.Equal(YoutubeUrlKind.NotYoutube, YoutubeUrlHelper.Classify(url));
        }

        [Fact]
        public void Classify_VideoIdNotFirstQueryParameter_ReturnsVideo()
        {
            var url = "https://www.youtube.com/watch?feature=shared&v=pHdRimeTooE";
            Assert.Equal(YoutubeUrlKind.Video, YoutubeUrlHelper.Classify(url));
        }

        [Fact]
        public void IsYoutubeUrl_RegularVideo_ReturnsTrue()
        {
            Assert.True(YoutubeUrlHelper.IsYoutubeUrl("https://youtu.be/5PtrPrJczpw"));
        }

        [Fact]
        public void IsYoutubeUrl_Shorts_ReturnsFalse()
        {
            Assert.False(YoutubeUrlHelper.IsYoutubeUrl("https://www.youtube.com/shorts/26BO8VRV6Zk"));
        }

        [Theory]
        [InlineData("https://www.youtube.com/watch?v=dQw4w9WgXcQ&t=42s", "dQw4w9WgXcQ")]
        [InlineData("https://youtu.be/dQw4w9WgXcQ", "dQw4w9WgXcQ")]
        [InlineData("https://www.youtube.com/embed/dQw4w9WgXcQ", "dQw4w9WgXcQ")]
        [InlineData("https://www.youtube.com/shorts/ZgdOfz6HWfY", "ZgdOfz6HWfY")]
        [InlineData("hello", null)]
        [InlineData(null, null)]
        public void ExtractVideoId_ReturnsExpectedId(string? url, string? expectedId)
        {
            Assert.Equal(expectedId, YoutubeUrlHelper.ExtractVideoId(url));
        }
    }
}