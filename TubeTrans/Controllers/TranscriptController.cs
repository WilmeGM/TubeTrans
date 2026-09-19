using Application.Services;
using Application.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace TubeTrans.Controllers
{
    public class TranscriptController : Controller
    {
        private readonly YoutubeTranscriptService _transcriptService;

        public TranscriptController(YoutubeTranscriptService transcriptService)
        {
            _transcriptService = transcriptService;
        }

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
                response = new TranscriptResponseViewModel
                {
                    VideoTitle = transcript.VideoTitle,
                    Text = transcript.Text,
                    HasError = false
                };
            }
            catch (Exception)
            {
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