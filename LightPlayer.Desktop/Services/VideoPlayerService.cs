using System;
using System.Threading.Tasks;
using LightPlayer.Desktop.Components;
using LightPlayer.Desktop.Facades;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace LightPlayer.Desktop.Services
{
    /// <summary>
    /// Video player service
    /// </summary>
    public class VideoPlayerService : IVideoPlayerService
    {
        private readonly ILogger<VideoPlayerService> _logger;
        
        private readonly IJSRuntime _jsRuntime;

        public VideoPlayerService(ILogger<VideoPlayerService> logger, IJSRuntime jsRuntime)
        {
            _logger = logger;
            _jsRuntime = jsRuntime;
        }

        private static string GetJsMethodName(string name) => $"_lightPlayer_{name}";

        public async Task<string> GetCurrentSrcAsync()
        {
            try
            {
                _logger.LogDebug("Getting src");
                var src = await _jsRuntime.InvokeAsync<string>(GetJsMethodName("getCurrentSrc"));
                _logger.LogDebug("Got src: {Src}", src);
                return src;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get src");
                throw;
            }
        }

        public async Task LoadAsync(string src, VideoPlayer component)
        {
            try
            {
                _logger.LogDebug("Loading src: {Src}", src);
                await _jsRuntime.InvokeVoidAsync(GetJsMethodName("load"), src, DotNetObjectReference.Create(component));
                _logger.LogDebug("Loaded src: {Src}", src);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to load src: {Src}", src);
                throw;
            }
        }

        public async Task<bool> GetIsPlayingAsync()
        {
            try
            {
                _logger.LogDebug("Getting isPlaying");
                var isPlaying = await _jsRuntime.InvokeAsync<bool>(GetJsMethodName("isPlaying"));
                _logger.LogDebug("Got isPlaying: {IsPlaying}", isPlaying);
                return isPlaying;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get isPlaying");
                throw;
            }
        }

        public async Task PlayAsync()
        {
            try
            {
                _logger.LogDebug("Trying to play");
                await _jsRuntime.InvokeVoidAsync(GetJsMethodName("play"));
                _logger.LogDebug("Started to play");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to play");
                throw;
            }
        }

        public async Task PauseAsync()
        {
            try
            {
                _logger.LogDebug("Trying to pause");
                await _jsRuntime.InvokeVoidAsync(GetJsMethodName("pause"));
                _logger.LogDebug("Finished to pause");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to pause");
                throw;
            }
        }

        public async Task<int> GetDurationAsync()
        {
            try
            {
                _logger.LogDebug("Trying to get duration");
                var duration = Convert.ToInt32(1000 * await _jsRuntime.InvokeAsync<double>(GetJsMethodName("getDuration")));
                _logger.LogDebug("Got duration: {Duration}", duration);
                return duration;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get duration");
                throw;
            }
        }

        public async Task<int> GetCurrentTimeAsync()
        {
            try
            {
                _logger.LogDebug("Trying to get current time");
                var currentTime = Convert.ToInt32(1000 * await _jsRuntime.InvokeAsync<double>(GetJsMethodName("getCurrentTime")));
                _logger.LogDebug("Got current time: {CurrentTime}", currentTime);
                return currentTime;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get current time");
                throw;
            }
        }

        public async Task SetCurrentTimeAsync(int currentTime)
        {
            try
            {
                _logger.LogDebug("Trying to set current time: {CurrentTime}", currentTime);
                await _jsRuntime.InvokeVoidAsync(GetJsMethodName("setCurrentTime"), currentTime / 1000.0);
                _logger.LogDebug("Set the current time to {CurrentTime}", currentTime);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to set current time");
                throw;
            }
        }

        public async Task<bool> GetIsMutedAsync()
        {
            try
            {
                _logger.LogDebug("Getting muted");
                var muted = await _jsRuntime.InvokeAsync<bool>(GetJsMethodName("getMuted"));
                _logger.LogDebug("Got muted: {Muted}", muted);
                return muted;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get muted");
                throw;
            }
        }

        public async Task SetMutedAsync(bool muted)
        {
            try
            {
                _logger.LogDebug("Trying to set muted: {Muted}", muted);
                await _jsRuntime.InvokeVoidAsync(GetJsMethodName("setMuted"), muted);
                _logger.LogDebug("Set muted: {Muted}", muted);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to set muted");
                throw;
            }
        }

        public async Task<int> GetVolumeAsync()
        {
            try
            {
                _logger.LogDebug("Trying to get volume");
                var volume = Convert.ToInt32(await _jsRuntime.InvokeAsync<double>(GetJsMethodName("getVolume")) * 100);
                _logger.LogDebug("Got volume: {Volume}", volume);
                return volume;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get volume");
                throw;
            }
        }

        public async Task SetVolumeAsync(int volume)
        {
            try
            {
                _logger.LogDebug("Trying to set volume: {Volume}", volume);
                await _jsRuntime.InvokeVoidAsync(GetJsMethodName("setVolume"), volume / 100.0);
                _logger.LogDebug("Set volume: {Volume}", volume);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to set volume: {Volume}", volume);
                throw;
            }
        }

        public async Task OpenFullscreenAsync()
        {
            try
            {
                _logger.LogDebug("Trying to open fullscreen");
                await _jsRuntime.InvokeVoidAsync(GetJsMethodName("openFullscreen"));
                _logger.LogDebug("Failed to open fullscreen");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to open fullscreen");
                throw;
            }
        }
    }
}