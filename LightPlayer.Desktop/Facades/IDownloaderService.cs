using System.Threading.Tasks;

namespace LightPlayer.Desktop.Facades
{
    /// <summary>
    /// Downloader service.
    /// </summary>
    public interface IDownloaderService
    {
        /// <summary>
        /// Download a given resource to a local temporary file and return the file name.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<string> DownloadAsync(string url);
    }
}