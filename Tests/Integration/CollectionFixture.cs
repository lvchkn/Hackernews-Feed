using Tests.Integration;
using Xunit;

[CollectionDefinition("Custom WAF collection")]
public class CollectionFixture : ICollectionFixture<CustomWebApplicationFactory<Program>>
{

}