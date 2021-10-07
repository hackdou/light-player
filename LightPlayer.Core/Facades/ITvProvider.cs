using System.Collections.Generic;
using System.Threading.Tasks;
using LightPlayer.Core.Models;

namespace LightPlayer.Core.Facades
{
    public interface ITvProvider
    {
        string ProviderId { get; }

        Task<IEnumerable<Episode>> SearchMoviesAsync(string word);

        Task<IEnumerable<PlayList>> QueryPlayListsAsync(Episode episode);

        Task<string> GetStreamUrlAsync(Video video);
    }
}