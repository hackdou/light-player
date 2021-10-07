using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LightPlayer.Business.Facades;
using LightPlayer.Test.Support;
using Xunit;

namespace LightPlayer.Test.Services
{
    [Collection("LightPlayer")]
    public class EpisodeServiceTest
    {
        private readonly TestFixture _fixture;

        public EpisodeServiceTest(TestFixture fixture)
        {
            _fixture = fixture;
        }

        private IEpisodeService EpisodeService => _fixture.GetRequiredService<IEpisodeService>();

        [Fact]
        public async Task Search()
        {
            var episode = await EpisodeService.SearchAsync("乔家的儿女");
            episode.Count().Should().BePositive();
        }
    }
}