using System.Collections.Generic;
using System.Threading.Tasks;
using LightPlayer.Core.Models;

namespace LightPlayer.Business.Facades
{
    public interface IEpisodeService
    {
        Task<IEnumerable<Episode>> SearchAsync(string word);
    }
}