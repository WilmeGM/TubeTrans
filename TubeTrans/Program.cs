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

            // Static web assets are enabled automatically only in Development. When the app runs
            // unpublished (dotnet run / F5) with the Production environment, they must be enabled by
            // hand, otherwise MapStaticAssets cannot find the files and CSS/JS return 404.
            // This is a workaround to simulate Production locally; a published deployment does not need it.
            if (builder.Environment.IsProduction())
            {
                builder.WebHost.UseStaticWebAssets(); // required to simulate production env in launchSettings.json
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

            app.Run();
        }
    }
}
