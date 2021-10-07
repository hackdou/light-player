using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightPlayer.Business.Facades;
using LightPlayer.Core.Facades;
using LightPlayer.Core.Models;

namespace LightPlayer.Business.Services
{
    internal sealed class PlayListService : IPlayListService
    {
        private readonly IEnumerable<ITvProvider> _providers;

        public PlayListService(IEnumerable<ITvProvider> providers)
        {
            _providers = providers;
        }

        public async Task<IEnumerable<PlayList>> QueryAsync(Episode episode)
        {
            var playLists = await _providers
                .First(provider => provider.ProviderId == episode.ProviderId)
                .QueryPlayListsAsync(episode);

            return playLists.OrderBy(playList => playList.Title);
        }
    }
}