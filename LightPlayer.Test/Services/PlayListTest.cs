using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LightPlayer.Business.Facades;
using LightPlayer.Core.Models;
using LightPlayer.Test.Support;
using Xunit;

namespace LightPlayer.Test.Services
{
    [Collection("LightPlayer")]
    public class PlayListTest
    {
        private readonly TestFixture _fixture;

        public PlayListTest(TestFixture fixture)
        {
            _fixture = fixture;
        }

        private IPlayListService PlayListService => _fixture.GetRequiredService<IPlayListService>();

        [Fact]
        public async Task Query()
        {
            var lists = await PlayListService.QueryAsync(new Episode
            {
                ProviderId = "www.tangrenjie.tv",
                ExternalId = "144320",
            });

            lists.Count().Should().BePositive();
        }
    }
}