using LightPlayer.Desktop.Facades;
using LightPlayer.Desktop.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LightPlayer.Desktop.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDownloaderService(this IServiceCollection services)
        {
            services.AddScoped<IDownloaderService, DownloaderService>();
        }
        
        public static void AddVideoPlayerService(this IServiceCollection services)
        {
            services.AddScoped<IVideoPlayerService, VideoPlayerService>();
        }
    }
}