using System.Collections.Generic;
using System.Threading.Tasks;
using LightPlayer.Core.Models;

namespace LightPlayer.Business.Facades
{
    public interface IPlayListService
    {
        Task<IEnumerable<PlayList>> QueryAsync(Episode episode);
    }
}