using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TubeTrans.Models;

namespace TubeTrans.Controllers
{
    /// <summary>
    /// Serves the site pages that are not part of the transcript feature.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Shows the privacy policy.
        /// </summary>
        /// <returns>The privacy policy page.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Shows a generic error page.
        /// </summary>
        /// <returns>The error page, with the identifier of the failed request.</returns>
        /// <remarks>
        /// This is the target of the exception handler registered in <see cref="Program.Main(string[])"/>,
        /// which only runs outside Development. In Development, the framework's detailed exception page is used instead.
        /// The response is marked as non-cacheable so an error page is never served from a cache.
        /// </remarks>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
