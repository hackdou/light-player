using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using LightPlayer.Core.Extensions;
using LightPlayer.Core.Facades;
using LightPlayer.Core.Models;
using LightPlayer.Integration.Apis;
using Refit;

namespace LightPlayer.Integration.Services
{
    internal sealed class TvProvider : ITvProvider
    {
        private const string Host = "www.tangrenjie.tv";

        private static class XPaths
        {
            public static class Search
            {
                public const string Item = "//li[@class = 'searchlist_item']";

                public const string ItemLink = "//h4[@class = 'vodlist_title']/a";

                public const string ItemImageLink = "//a[contains(@class, 'vodlist_thumb')]";
            }

            public static class PlatList
            {
                public const string Item = "//div[contains(@class, 'play_list_box')]";
                
                public const string ItemName = "//div[@id = 'NumTab']/a/b";

                public const string VideoLink = "//div[@class = 'playlist_full']/ul/li/a";
            }
        }

        public string ProviderId => Host;

        private static ITrjApi GetApi() => RestService.For<ITrjApi>($"https://{Host}");

        public async Task<IEnumerable<Episode>> SearchMoviesAsync(string word)
        {
            var html = await GetApi().GetSearchPageAsync(word, string.Empty).AsHtmlAsync();

            var items = html.SelectNodes(XPaths.Search.Item);
            
            if (items == null)
            {
                return Array.Empty<Episode>();
            }

            var episodes = new List<Episode>();

            foreach (var node in items.Select(item => HtmlNode.CreateNode(item.OuterHtml)))
            {
                var linkNode = node.SelectSingleNode(XPaths.Search.ItemLink);

                if (linkNode == null)
                {
                    continue;
                }
                
                var title = linkNode.Attributes["title"].Value;
                var url = linkNode.Attributes["href"].Value;

                if (!int.TryParse(Regex.Replace(url, @"(^/vod/detail/id/|\.html$)", string.Empty), out var externalId))
                {
                    continue;
                }

                var imageNode = node.SelectSingleNode(XPaths.Search.ItemImageLink);

                if (imageNode == null)
                {
                    continue;
                }

                var imageUrl = imageNode.Attributes["data-original"].Value;
                if (!(imageUrl.StartsWith("http:") || imageUrl.StartsWith("https:")))
                {
                    imageUrl = $"https://{Host}{imageUrl}";
                }

                episodes.Add(new Episode
                {
                    ProviderId = ProviderId,
                    ExternalId = externalId.ToString(),
                    Title = title,
                    ImageUrl = imageUrl
                });
            }

            return episodes;
        }

        public async Task<IEnumerable<PlayList>> QueryPlayListsAsync(Episode episode)
        {
            var html = await GetApi().GetPlayListPageAsync(episode.ExternalId).AsHtmlAsync();

            var items = html.SelectNodes(XPaths.PlatList.Item);

            if (items == null)
            {
                return Array.Empty<PlayList>();
            }

            var itemNames = html.SelectNodes(XPaths.PlatList.ItemName);

            if (itemNames == null)
            {
                return Array.Empty<PlayList>();
            }

            if (items.Count != itemNames.Count)
            {
                return Array.Empty<PlayList>();
            }

            var lists = new List<PlayList>();
            var i = 0;
            
            foreach (var node in items.Select(item => HtmlNode.CreateNode(item.OuterHtml)))
            {
                var title = itemNames[i++].InnerText;

                var links = node.SelectNodes(XPaths.PlatList.VideoLink);

                if (links == null)
                {
                    continue;
                }

                var videos = links.Select(link => new Video
                {
                    ProviderId = ProviderId,
                    ExternalId = link.Attributes["href"].Value,
                    Title = link.InnerText
                });
                
                lists.Add(new PlayList
                {
                    Title = title,
                    Videos = videos.ToList()
                });
            }

            return lists;
        }

        public async Task<string> GetStreamUrlAsync(Video video)
        {
            var html = await GetApi().GetPageAsync(video.ExternalId[1..]);
            var json = Regex.Match(html, @"player_aaaa\=\{[^}]+\}").Groups[0].Value[12..];
            var encodedUrl = JsonSerializer.Deserialize<Dictionary<string, object>>(json)!["url"].ToString();

            return HttpUtility.UrlDecode(Encoding.UTF8.GetString(Convert.FromBase64String(encodedUrl!)!));
        }
    }
}