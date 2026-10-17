using Application.Exceptions;
using Application.Interfaces.Services;
using Application.Models;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TubeTrans.Controllers;

namespace Tests.Controllers
{
    public class TranscriptControllerTests
    {
        private static TranscriptController CreateController(FakeTranscriptService service) =>
            new(service, NullLogger<TranscriptController>.Instance);

        // ViewBag.Response y ViewData["Response"] son el mismo almacén.
        private static TranscriptResponseViewModel? GetResponse(Controller controller) =>
            controller.ViewData["Response"] as TranscriptResponseViewModel;

        [Fact]
        public void Index_Get_ReturnsViewWithEmptyRequestModel()
        {
            var controller = CreateController(new FakeTranscriptService());

            var view = Assert.IsType<ViewResult>(controller.Index());

            Assert.IsType<TranscriptRequestViewModel>(view.Model);
        }

        [Fact]
        public async Task Index_Post_InvalidModelState_ReturnsSameViewWithoutCallingService()
        {
            var service = new FakeTranscriptService();
            var controller = CreateController(service);
            controller.ModelState.AddModelError("Url", "Invalid");
            var vm = new TranscriptRequestViewModel { Url = "bad" };

            var view = Assert.IsType<ViewResult>(await controller.Index(vm));

            Assert.Same(vm, view.Model);
            Assert.Null(GetResponse(controller));
            Assert.Equal(0, service.Calls);
        }

        [Fact]
        public async Task Index_Post_ServiceSucceeds_ReturnsMappedResponse()
        {
            var service = new FakeTranscriptService
            {
                Result = new VideoTranscript { VideoTitle = "My video", Text = "Hello world" }
            };
            var controller = CreateController(service);

            await controller.Index(new TranscriptRequestViewModel { Url = "https://youtu.be/dQw4w9WgXcQ" });

            var response = GetResponse(controller);
            Assert.NotNull(response);
            Assert.False(response.HasError);
            Assert.Equal("My video", response.VideoTitle);
            Assert.Equal("Hello world", response.Text);
        }

        [Fact]
        public async Task Index_Post_DomainError_ShowsItsOwnMessage()
        {
            var service = new FakeTranscriptService
            {
                Throw = new TranscriptNotAvailableException("No captions here")
            };
            var controller = CreateController(service);

            await controller.Index(new TranscriptRequestViewModel { Url = "https://youtu.be/dQw4w9WgXcQ" });

            var response = GetResponse(controller);
            Assert.NotNull(response);
            Assert.True(response.HasError);
            Assert.Equal("No captions here", response.ErrorMessage);
        }

        [Fact]
        public async Task Index_Post_UnexpectedError_ShowsGenericMessageWithoutInternalDetails()
        {
            var service = new FakeTranscriptService
            {
                Throw = new HttpRequestException("internal detail: www.youtube.com:443")
            };
            var controller = CreateController(service);

            await controller.Index(new TranscriptRequestViewModel { Url = "https://youtu.be/dQw4w9WgXcQ" });

            var response = GetResponse(controller);
            Assert.NotNull(response);
            Assert.True(response.HasError);
            Assert.False(string.IsNullOrWhiteSpace(response.ErrorMessage));
            Assert.DoesNotContain("internal detail", response.ErrorMessage);
        }
    }

    public class FakeTranscriptService : IYoutubeTranscriptService
    {
        public int Calls { get; private set; }
        public VideoTranscript Result { get; set; } = new();
        public Exception? Throw { get; set; }

        public Task<VideoTranscript> GetTranscriptAsync(string videoUrl)
        {
            Calls++;
            if (Throw is not null) throw Throw;
            return Task.FromResult(Result);
        }
    }
}
