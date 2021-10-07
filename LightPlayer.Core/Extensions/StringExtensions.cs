using System.Threading.Tasks;
using HtmlAgilityPack;

namespace LightPlayer.Core.Extensions
{
    public static class StringExtensions
    {
        public static HtmlNode AsHtml(this string content)
        {
            var doc = new HtmlDocument();
            
            doc.LoadHtml(content);

            return doc.DocumentNode;
        }

        public static async Task<HtmlNode> AsHtmlAsync(this Task<string> contentTask)
        {
            return (await contentTask.ConfigureAwait(false)).AsHtml();
        }
    }
}