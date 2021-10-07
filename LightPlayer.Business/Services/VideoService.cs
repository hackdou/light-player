using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightPlayer.Business.Facades;
using LightPlayer.Core.Facades;
using LightPlayer.Core.Models;

namespace LightPlayer.Business.Services
{
    public class VideoService : IVideoService
    {
        private readonly IEnumerable<ITvProvider> _providers;

        public VideoService(IEnumerable<ITvProvider> providers)
        {
            _providers = providers;
        }

        public Task<string> GetUrlAsync(Video video)
        {
            return _providers
                .First(provider => provider.ProviderId == video.ProviderId)
                .GetStreamUrlAsync(video);
        }
    }
}