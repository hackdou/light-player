using LightPlayer.Business.Facades;
using LightPlayer.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LightPlayer.Business.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEpisodeService(this IServiceCollection services)
        {
            services.AddSingleton<IEpisodeService, EpisodeService>();
        }
        
        public static void AddPlayListService(this IServiceCollection services)
        {
            services.AddSingleton<IPlayListService, PlayListService>();
        }
        
        public static void AddVideoService(this IServiceCollection services)
        {
            services.AddSingleton<IVideoService, VideoService>();
        }
    }
}