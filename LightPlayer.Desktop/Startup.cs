using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using LightPlayer.Business.Extensions;
using LightPlayer.Desktop.Extensions;
using LightPlayer.Integration.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace LightPlayer.Desktop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Razor pages.
            services.AddRazorPages();
            
            // Server side blazor.
            services.AddServerSideBlazor();

            // Electron.
            services.AddElectron();
            
            // Mub services.
            services.AddMudServices();
            
            // TV providers.
            services.AddTrjTvProvider();
            
            // Business services.
            services.AddEpisodeService();
            services.AddPlayListService();
            services.AddVideoService();

            // Downloader service.
            services.AddDownloaderService();
            
            // Video player service.
            services.AddVideoPlayerService();

            // Controllers.
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
            });

            Task.Run(async () => await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                TitleBarStyle = TitleBarStyle.hidden,
                MinWidth = 600,
                MinHeight = 400
            }));
        }
    }
}
