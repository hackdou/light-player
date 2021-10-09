using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LightPlayer.Core.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LightPlayer.Desktop.Controllers
{
    /// <summary>
    /// Proxy controller
    /// </summary>
    [Route("/proxy")]
    public class ProxyController : Controller
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var address = Base64Utils.FromBase64(id);
            var fileName = Path.GetTempFileName();
            
            using var client = new WebClient();
            await client.DownloadFileTaskAsync(address, fileName);

            return File(new FileStream(fileName, FileMode.Open), "application/octet-stream");
        }
        
        [HttpGet("{id}.m3u8")]
        public async Task<IActionResult> GetM3U8(string id)
        {
            var requestUri = Base64Utils.FromBase64(id);

            using var client = new HttpClient();
            var content = await client.GetStringAsync(requestUri);
            
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
                        line = new Uri(new Uri(requestUri), line).ToString();
                    }

                    if (line.EndsWith(".m3u8"))
                    {
                        return $"/proxy/{Base64Utils.ToBase64(line)}.m3u8";
                    }

                    return $"/proxy/{Base64Utils.ToBase64(line)}";
                });
            
            return Content(string.Join("\r\n", lines));
        }
    }
}