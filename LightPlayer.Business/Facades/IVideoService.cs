using System.Threading.Tasks;
using LightPlayer.Core.Models;

namespace LightPlayer.Business.Facades
{
    public interface IVideoService
    {
        Task<string> GetUrlAsync(Video video);
    }
}