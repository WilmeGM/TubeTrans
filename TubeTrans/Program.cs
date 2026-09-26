using Application.Services;
using YoutubeExplode;

namespace TubeTrans
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<YoutubeClient>();
            builder.Services.AddScoped<YoutubeTranscriptService>();

            if (!builder.Environment.IsProduction())
            {
                builder.WebHost.UseStaticWebAssets(); // required to simulate production env in launchSettings.json
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

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
