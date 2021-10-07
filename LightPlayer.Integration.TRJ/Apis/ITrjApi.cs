using System;
using System.Threading.Tasks;
using Refit;

namespace LightPlayer.Integration.Apis
{
    internal interface ITrjApi
    {
        [Get("/vod/search.html")]
        Task<string> GetSearchPageAsync([AliasAs("wd")] string word, [AliasAs("submit")] string submit);

        [Get("/vod/detail/id/{id}.html")]
        Task<string> GetPlayListPageAsync([AliasAs("id")] string id);

        [Get("/{path}.html")]
        [QueryUriFormat(UriFormat.Unescaped)]
        Task<string> GetPageAsync([AliasAs("path")] string path);
    }
}