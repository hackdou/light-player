using LightPlayer.Business.Extensions;
using LightPlayer.Integration.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LightPlayer.Test.Support
{
    public class TestStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Providers.
            services.AddTrjTvProvider();
            
            // Business Services.
            services.AddEpisodeService();
            services.AddPlayListService();
            services.AddVideoService();
        }

        public static void Configure(IApplicationBuilder app)
        {
        }
    }
}