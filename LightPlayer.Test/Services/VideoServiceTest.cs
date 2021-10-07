using System.Threading.Tasks;
using FluentAssertions;
using LightPlayer.Business.Facades;
using LightPlayer.Core.Models;
using LightPlayer.Test.Support;
using Xunit;

namespace LightPlayer.Test.Services
{
    [Collection("LightPlayer")]
    public class VideoServiceTest
    {
        private readonly TestFixture _fixture;

        public VideoServiceTest(TestFixture fixture)
        {
            _fixture = fixture;
        }

        private IVideoService VideoService => _fixture.GetRequiredService<IVideoService>();

        [Fact]
        public async Task Query()
        {
            var url = await VideoService.GetUrlAsync(new Video
            {
                ProviderId = "www.tangrenjie.tv",
                ExternalId = "/vod/play/id/144320/sid/1/nid/1.html",
                Title = "第01集"
            });

            url.Should().NotBeNullOrEmpty();
        }
    }
}