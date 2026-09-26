using Application.Exceptions;
using Application.Services;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace TubeTrans.Controllers
{
    public class TranscriptController : Controller
    {
        private readonly YoutubeTranscriptService _transcriptService;
        private readonly ILogger<TranscriptController> _logger;

        public TranscriptController(
            YoutubeTranscriptService transcriptService,
            ILogger<TranscriptController> logger)
        {
            _transcriptService = transcriptService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new TranscriptRequestViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(TranscriptRequestViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            TranscriptResponseViewModel response;

            try
            {
                var transcript = await _transcriptService.GetTranscriptAsync(vm.Url);

                _logger.LogInformation("Transcript successfully fetched for URL {Url}", vm.Url);

                response = new TranscriptResponseViewModel
                {
                    VideoTitle = transcript.VideoTitle,
                    Text = transcript.Text,
                    HasError = false
                };
            }
            catch (TubeTransException ex)
            {
                _logger.LogWarning(ex, "Known transcript error for URL {Url}", vm.Url);

                response = new TranscriptResponseViewModel
                {
                    HasError = true,
                    ErrorMessage = ex.Message
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching transcript for URL {Url}", vm.Url);

                response = new TranscriptResponseViewModel
                {
                    HasError = true,
                    ErrorMessage = "Something went wrong reading that video."
                };
            }

            ViewBag.Response = response;
            return View(vm);
        }
    }
}