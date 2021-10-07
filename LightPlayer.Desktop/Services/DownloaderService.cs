using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using LightPlayer.Desktop.Facades;
using Microsoft.Extensions.Logging;

namespace LightPlayer.Desktop.Services
{
    /// <summary>
    /// Downloader service.
    /// </summary>
    public class DownloaderService : IDownloaderService
    {
        private readonly ILogger<DownloaderService> _logger;

        public DownloaderService(ILogger<DownloaderService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Download a given resource to a local temporary file and return the file name.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> DownloadAsync(string url)
        {
            // Generate a temporary file name.
            var fileName = Path.GetTempFileName();

            try
            {
                _logger.LogDebug("Downloading {Url} to {FileName}", url, fileName);

                using var webClient = new WebClient();
                await webClient.DownloadFileTaskAsync(url, fileName);
                
                _logger.LogDebug("Downloaded {Url} to {FileName}", url, fileName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to download {Url} to {FileName}", url, fileName);
                throw;
            }
            
            return fileName;
        }
    }
}