using LightPlayer.Core.Facades;
using LightPlayer.Integration.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LightPlayer.Integration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTrjTvProvider(this IServiceCollection services)
        {
            services.AddSingleton<ITvProvider, TvProvider>();
        }
    }
}