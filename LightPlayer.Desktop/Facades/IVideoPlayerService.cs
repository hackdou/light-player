using System.Threading.Tasks;
using LightPlayer.Desktop.Components;

namespace LightPlayer.Desktop.Facades
{
    /// <summary>
    /// Video player service
    /// </summary>
    public interface IVideoPlayerService
    {
        Task<string> GetCurrentSrcAsync();

        Task LoadAsync(string src, VideoPlayer component);

        Task<bool> GetIsPlayingAsync();

        Task PlayAsync();

        Task PauseAsync();

        Task<int> GetDurationAsync();

        Task<int> GetCurrentTimeAsync();

        Task SetCurrentTimeAsync(int currentTime);

        Task<bool> GetIsMutedAsync();

        Task SetMutedAsync(bool muted);

        Task<int> GetVolumeAsync();

        Task SetVolumeAsync(int volume);

        Task OpenFullscreenAsync();
    }
}