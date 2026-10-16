using Application.Exceptions;
using Application.Interfaces.Services;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace TubeTrans.Controllers
{
    /// <summary>
    /// Serves the transcript page: the search form and, after a submission, the result below it.
    /// </summary>
    /// <remarks>
    /// Both actions render the same view, <c>Views/Transcript/Index.cshtml</c>. The view's model is the
    /// request (<see cref="TranscriptRequestViewModel"/>) because the form binds to it. The outcome of a
    /// lookup travels separately, in <c>ViewBag.Response</c>, as a <see cref="TranscriptResponseViewModel"/>.
    /// <para>
    /// This controller is where domain results become presentation: it maps the service's
    /// <see cref="Application.Models.VideoTranscript"/> to a view model and decides what the user sees
    /// when something fails.
    /// </para>
    /// </remarks>
    public class TranscriptController : Controller
    {
        private readonly IYoutubeTranscriptService _transcriptService;
        private readonly ILogger<TranscriptController> _logger;

        /// <summary>
        /// Initializes a new instance of the controller.
        /// </summary>
        /// <param name="transcriptService">The service that retrieves transcripts, supplied by dependency injection.</param>
        /// <param name="logger">The logger for this controller, supplied by dependency injection.</param>
        public TranscriptController(
            IYoutubeTranscriptService transcriptService,
            ILogger<TranscriptController> logger)
        {
            _transcriptService = transcriptService;
            _logger = logger;
        }

        /// <summary>
        /// Shows the empty form. Default page of the application.
        /// </summary>
        /// <returns>The page with a new, empty <see cref="TranscriptRequestViewModel"/>.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View(new TranscriptRequestViewModel());
        }

        /// <summary>
        /// Looks up the transcript of the submitted URL and renders the page with the result.
        /// </summary>
        /// <param name="vm">The submitted form, already validated by its data annotations.</param>
        /// <returns>The same view, with the transcript or an error message in <c>ViewBag.Response</c>.</returns>
        /// <remarks>
        /// There are four possible outcomes:
        /// <list type="bullet">
        ///   <item><description>Invalid input: the form is shown again with its validation messages. The service is not called.</description></item>
        ///   <item><description>Success: the transcript is shown. Logged at <c>Information</c>, with the elapsed time.</description></item>
        ///   <item><description>Expected error (<see cref="TubeTransException"/>): its own message is shown, because it is written to be user-facing. Logged at <c>Warning</c>.</description></item>
        ///   <item><description>Unexpected error: a generic message is shown so no internal detail reaches the user, and the full exception is logged at <c>Error</c>.</description></item>
        /// </list>
        /// The result is returned directly instead of redirecting (Post/Redirect/Get). A redirect would need
        /// the transcript to survive until the next request, and the application stores nothing between requests.
        /// The submitted URL is written to the logs in every outcome except invalid input.
        /// </remarks>
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
                var stopwatch = Stopwatch.StartNew();
                var transcript = await _transcriptService.GetTranscriptAsync(vm.Url);
                stopwatch.Stop();

                _logger.LogInformation("Transcript successfully fetched for URL {Url} in {ElapsedMilliseconds} ms", vm.Url, stopwatch.ElapsedMilliseconds);

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