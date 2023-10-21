using Api;
using Xunit;

namespace Tests.Integration;

[CollectionDefinition("Custom WAF collection")]
public class CollectionFixture : ICollectionFixture<CustomWebApplicationFactory<Program>>
{

}