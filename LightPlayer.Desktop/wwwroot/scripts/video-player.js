let currentHls = null;
let currentSrc = null;

const getVideo = () =>
{
    return document.getElementById("light-player-video");
};

const init = (video, src, dotNetHelper) =>
{
    if (currentHls != null)
    {
        currentHls.destroy();
    }

    let hls = new Hls();

    hls.attachMedia(video);
    
    hls.on(Hls.Events.MEDIA_ATTACHED, () =>
    {
        hls.loadSource(src);

        hls.on(Hls.Events.MANIFEST_PARSED, () =>
        {
            video.play();
        });
    });

    hls.on(Hls.Events.ERROR, (event, data) =>
    {
        if (!data.fatal)
        {
            return;
        }
        
        dotNetHelper
            .invokeMethodAsync("NotifyError", data.details)
            .then(() =>
            {
                switch(data.type)
                {
                    case Hls.ErrorTypes.NETWORK_ERROR:
                        hls.startLoad();
                        break;
                        
                    case Hls.ErrorTypes.MEDIA_ERROR:
                        hls.recoverMediaError();
                        break;

                    default:
                        console.log("Failed to load video", data);
                        break;
                }
            });
    });
    
    currentHls = hls;
    currentSrc = src;
};

// The following methods will be invoked from Razor-side,
// see `VideoService` to understand more details.

window._lightPlayer_getCurrentSrc = () =>
{
    const video = getVideo()
    
    if (video)
    {
        return currentSrc;
    }
    
    return null;
};

window._lightPlayer_load = (videoSrc, dotNetHelper) =>
{
    const video = getVideo();

    if (video)
    {
        init(video, videoSrc, dotNetHelper);
    }
};

window._lightPlayer_isPlaying = () =>
{
    const video = getVideo();

    if (video)
    {
        return !(video.paused);
    }
    
    return false;
};

window._lightPlayer_play = () =>
{
    const video = getVideo();
    
    if (video)
    {
        video.play();
    }
};

window._lightPlayer_pause = () =>
{
    const video = getVideo();

    if (video)
    {
        video.pause();
    }
};

window._lightPlayer_getDuration = () =>
{
    const video = getVideo();

    if (video && !isNaN(video.duration))
    {
        return video.duration;
    }

    return 1;
};

window._lightPlayer_getCurrentTime = () =>
{
    const video = getVideo();
    
    if (video && !isNaN(video.currentTime))
    {
        return video.currentTime;
    }
    
    return 0;
};

window._lightPlayer_setCurrentTime = (currentTime) =>
{
    const video = getVideo();
    
    if (video)
    {
        video.currentTime = currentTime;
    }
};

window._lightPlayer_getMuted = () =>
{
    const video = getVideo();

    if (video)
    {
        return video.muted;
    }

    return false;
};

window._lightPlayer_setMuted = (muted) =>
{
    const video = getVideo();

    if (video)
    {
        video.muted = muted;
    }
};

window._lightPlayer_getVolume = () =>
{
    const video = getVideo();
    
    if (video)
    {
        return video.volume;
    }
    
    return 1;
}

window._lightPlayer_setVolume = (volume) =>
{
    const video = getVideo();
    
    if (video)
    {
        video.volume = volume;
    }
}

window._lightPlayer_openFullscreen = () =>
{
    const video = getVideo();
    
    const requestFullscreen =
        video.requestFullscreen ||
        video.webkitRequestFullscreen ||
        video.msRequestFullscreen;
    
    if (requestFullscreen)
    {
        requestFullscreen.bind(video)();
    }
}