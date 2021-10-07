using System.IO;
using System.Threading.Tasks;
using LightPlayer.Desktop.Facades;
using Microsoft.AspNetCore.Mvc;

namespace LightPlayer.Desktop.Controllers
{
    /// <summary>
    /// Images controller.
    /// </summary>
    [Route("/images")]
    public class ImagesController : Controller
    {
        private readonly IDownloaderService _downloaderService;

        public ImagesController(IDownloaderService downloaderService)
        {
            _downloaderService = downloaderService;
        }
        
        public async Task<IActionResult> GetImageAsync([FromQuery] string srcUrl)
        {
            // Download the image to a temporary file.
            var fileName = await _downloaderService.DownloadAsync(srcUrl);
            
            // Response the file to client side.
            return File(new FileStream(fileName, FileMode.Open), "application/octet-stream");
        }
    }
}