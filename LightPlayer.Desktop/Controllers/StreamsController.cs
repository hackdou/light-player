using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LightPlayer.Desktop.Facades;
using LightPlayer.Desktop.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LightPlayer.Desktop.Controllers
{
    [Route("/streams")]
    public class StreamsController : Controller
    {
        private readonly IDownloaderService _downloaderService;

        public StreamsController(IDownloaderService downloaderService)
        {
            _downloaderService = downloaderService;
        }

        // GET
        [HttpGet("{id}.m3u8")]
        public async Task<IActionResult> GetM3U8(string id)
        {
            // Parse original URL.
            var originalUrl = Base64Utils.FromBase64(id);

            using var client = new HttpClient();
            var content = await client.GetStringAsync(originalUrl);
            var lines = content
                .Split(new[] { "\r", "\n", "\r\n" }, StringSplitOptions.None)
                .Select(line =>
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        return line;
                    }

                    if (line.StartsWith("#"))
                    {
                        return line;
                    }

                    if (!line.StartsWith("http:") || !line.StartsWith("https:"))
                    {
                        line = new Uri(new Uri(originalUrl), line).ToString();
                    }

                    if (line.EndsWith(".m3u8"))
                    {
                        return $"/streams/{Base64Utils.ToBase64(line)}.m3u8";
                    }

                    if (line.EndsWith(".ts"))
                    {
                        return $"/streams/{Base64Utils.ToBase64(line)}.ts";
                    }

                    return line;
                });
            
            return Content(string.Join("\r\n", lines));
        }

        [HttpGet("{id}.ts")]
        public async Task<IActionResult> GetTsAsync(string id)
        {
            // Parse original URL.
            var originalUrl = Base64Utils.FromBase64(id);
            
            // Download to a temporary file.
            var fileName = await _downloaderService.DownloadAsync(originalUrl);

            // Response the file to client side.
            return File(new FileStream(fileName, FileMode.Open), "application/octet-stream");
        }
    }
}