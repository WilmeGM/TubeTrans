using Application.Models;

namespace Application.Interfaces.Services
{
    public interface IYoutubeTranscriptService
    {
        Task<VideoTranscript> GetTranscriptAsync(string videoUrl);
    }
}
