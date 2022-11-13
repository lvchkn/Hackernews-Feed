using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Infrastructure.Workers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.Integration;

public class CustomWebApplicationFactory<Program> : WebApplicationFactory<Program>, IAsyncLifetime, IAsyncDisposable 
    where Program: class, new()
{
    private readonly TestcontainerDatabase _mongoTestContainer;
    private readonly TestcontainerMessageBroker _rmqContainer;

    public CustomWebApplicationFactory()
    {
        _mongoTestContainer = new TestcontainersBuilder<MongoDbTestcontainer>()
            .WithDatabase(new MongoDbTestcontainerConfiguration()
            {
                Database = "feed",
                Username = "testuser",
                Password = "testpw"
            })
            .WithImage("mongo:5.0.6")
            .WithName("mongotests")
            .WithExposedPort(27017)
            .WithCleanUp(true)
            .Build();

         _rmqContainer = new TestcontainersBuilder<RabbitMqTestcontainer>()
            .WithMessageBroker(new RabbitMqTestcontainerConfiguration 
            {
                Username = "testuser", 
                Password = "testpw" 
            })
            .WithImage("rabbitmq:3.7.28")
            .WithName("rmqtests")
            .WithExposedPort(5672)
            .WithCleanUp(true)
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "development");
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
            .AddJsonFile("appsettings.test.json")
            .AddInMemoryCollection(new Dictionary<string, string?>()
            {
                ["MongoDb:ConnectionString"] = _mongoTestContainer.ConnectionString,
                ["MongoDB:Url"] = _mongoTestContainer.ConnectionString,
                ["RabbitMq:Port"] = _rmqContainer.Port.ToString(),
                ["RabbitMq:UserName"] = _rmqContainer.Username.ToString(),
                ["RabbitMq:Password"] = _rmqContainer.Password.ToString(),
            })
            .Build();
        
        builder.UseConfiguration(config);

        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(authOptions => 
            {
                authOptions.DefaultAuthenticateScheme = "Test";
                authOptions.DefaultChallengeScheme = "Test";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                "Test", options => {});

            var descriptor = services.SingleOrDefault(s => s.ImplementationType == typeof(StoryFetcher));
            
            if (descriptor is not null) 
            {
                services.Remove(descriptor);
            }
        });
    }

    public async Task InitializeAsync()
    {
        await _mongoTestContainer.StartAsync();
        await _rmqContainer.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _mongoTestContainer.DisposeAsync();
        await _rmqContainer.DisposeAsync();
    }
}