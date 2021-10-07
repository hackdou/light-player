using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightPlayer.Business.Facades;
using LightPlayer.Core.Facades;
using LightPlayer.Core.Models;

namespace LightPlayer.Business.Services
{
    internal sealed class EpisodeService : IEpisodeService
    {
        private readonly IEnumerable<ITvProvider> _providers;

        public EpisodeService(IEnumerable<ITvProvider> providers)
        {
            _providers = providers;
        }

        public async Task<IEnumerable<Episode>> SearchAsync(string word)
        {
            var results = await Task.WhenAll(_providers.Select(provider => provider.SearchMoviesAsync(word)));
            return results.SelectMany(x => x);
        }
    }
}