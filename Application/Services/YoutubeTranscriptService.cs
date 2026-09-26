using Application.Exceptions;
using Application.Helpers;
using Application.Models;
using YoutubeExplode;
using YoutubeExplode.Exceptions;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.ClosedCaptions;

namespace Application.Services
{
    public class YoutubeTranscriptService
    {
        private readonly YoutubeClient _youtubeClient;

        public YoutubeTranscriptService(YoutubeClient youtubeClient)
        {
            _youtubeClient = youtubeClient;
        }

        public string ExtractVideoId(string videoUrl)
        {
            var videoId = YoutubeUrlHelper.ExtractVideoId(videoUrl);

            if (string.IsNullOrWhiteSpace(videoId))
            {
                throw new InvalidOperationException("Could not extract a video ID from the provided URL.");
            }

            return videoId;
        }

        public async Task<IReadOnlyList<ClosedCaptionTrackInfo>> GetAvailableCaptionTracksAsync(string videoId)
        {
            var trackManifest = await _youtubeClient.Videos.ClosedCaptions.GetManifestAsync(videoId);
            return trackManifest.Tracks;
        }

        public async Task<ClosedCaptionTrackInfo?> GetPrimaryCaptionTrackAsync(string videoId)
        {
            var tracks = await GetAvailableCaptionTracksAsync(videoId);
            return CaptionTrackSelector.SelectPrimary(tracks);
        }

        public async Task<VideoTranscript> GetTranscriptAsync(string videoUrl)
        {
            var videoId = ExtractVideoId(videoUrl);

            Video video;
            ClosedCaptionTrackInfo? primaryTrack;

            try
            {
                video = await _youtubeClient.Videos.GetAsync(videoId);
                primaryTrack = await GetPrimaryCaptionTrackAsync(videoId);
            }
            catch (VideoUnavailableException)
            {
                throw new VideoNotFoundException("This video doesn't exist, is private, or is no longer available.");
            }

            if (primaryTrack is null)
            {
                throw new TranscriptNotAvailableException("This video doesn't have any captions available.");
            }

            var captionTrack = await _youtubeClient.Videos.ClosedCaptions.GetAsync(primaryTrack);

            var text = string.Join(" ",
                captionTrack.Captions.Select(c => c.Text.Replace('\n', ' ').Trim()));

            return new VideoTranscript
            {
                VideoTitle = video.Title,
                Text = text
            };
        }
    }
}