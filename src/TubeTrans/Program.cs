using Application.Interfaces.Services;
using Application.Services;
using YoutubeExplode;

namespace TubeTrans
{
    /// <summary>
    /// Application entry point: registers the services and defines the HTTP request pipeline.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Builds and runs the web application.
        /// </summary>
        /// <param name="args">Command-line arguments, forwarded to the host builder.</param>
        /// <remarks>
        /// Every request goes through the pipeline from top to bottom, so the order matters:
        /// <list type="number">
        ///   <item><description>Error handling and HSTS, outside Development only.</description></item>
        ///   <item><description>HTTP to HTTPS redirection.</description></item>
        ///   <item><description>Routing.</description></item>
        ///   <item><description>Authorization. The app has no authentication, so this has no effect today.</description></item>
        ///   <item><description>Endpoints: the static assets in <c>wwwroot</c> and the default route, <c>Transcript/Index</c>.</description></item>
        /// </list>
        /// </remarks>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            // One instance per request. Both are stateless, so nothing is shared between users.
            builder.Services.AddScoped<YoutubeClient>();
            builder.Services.AddScoped<IYoutubeTranscriptService, YoutubeTranscriptService>();

            // Static web assets are enabled automatically only in Development. To simulate a production-like
            // run locally (dotnet run / F5), the Staging environment enables them by hand; otherwise
            // MapStaticAssets cannot find the files and CSS/JS return 404.
            // A published deployment runs in Production and does not need this.
            if (builder.Environment.IsStaging())
            {
                builder.WebHost.UseStaticWebAssets();
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // Unhandled exceptions are re-executed at this path, so the user sees a friendly page
                // instead of technical details.
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Behind a reverse proxy that terminates TLS (most hosting providers), this redirect can
            // loop. If that happens, set ASPNETCORE_FORWARDEDHEADERS_ENABLED=true on the host.
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Transcript}/{action=Index}/{id?}")
                .WithStaticAssets();

            if (app.Configuration["ENABLE_YT_DIAG"] == "true")
            {
                app.MapGet("/diag/youtube", async () =>
                {
                    using var http = new HttpClient();
                    http.DefaultRequestHeaders.UserAgent.ParseAdd(
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0 Safari/537.36");
                    http.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");

                    using var response = await http.GetAsync("https://www.youtube.com/watch?v=dQw4w9WgXcQ&hl=en");
                    var html = await response.Content.ReadAsStringAsync();

                    var title = System.Text.RegularExpressions.Regex.Match(
                        html, "<title>(.*?)</title>", System.Text.RegularExpressions.RegexOptions.Singleline).Groups[1].Value;
                    var playability = System.Text.RegularExpressions.Regex.Match(
                        html, @"""playabilityStatus"":\{""status"":""(\w+)""(?:,""reason"":""([^""]*)"")?");

                    return Results.Json(new
                    {
                        status = (int)response.StatusCode,
                        finalUrl = response.RequestMessage?.RequestUri?.ToString(),
                        htmlLength = html.Length,
                        title,
                        hasOgUrl = html.Contains("property=\"og:url\""),
                        playabilityStatus = playability.Groups[1].Value,
                        playabilityReason = playability.Groups[2].Value
                    });
                });
            }

            app.Run();
        }
    }
}
