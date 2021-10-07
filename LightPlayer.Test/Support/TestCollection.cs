using Xunit;

namespace LightPlayer.Test.Support
{
    [CollectionDefinition("LightPlayer")]
    public class TestCollection : ICollectionFixture<TestFixture>
    {
    }
}