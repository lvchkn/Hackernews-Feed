using Api;
using Xunit;

namespace Tests.Integration;

[CollectionDefinition("Test server collection")]
public class CollectionDefinition : ICollectionFixture<TestServerFixture<Program>>
{
    
}