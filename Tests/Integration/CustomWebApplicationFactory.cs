using Infrastructure.Db;
using Infrastructure.Workers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Xunit;

namespace Tests.Integration;

public class CustomWebApplicationFactory<Program> : WebApplicationFactory<Program>, IAsyncLifetime 
    where Program: class, new()
{
    private readonly PostgreSqlContainer _postgresContainer;
    private readonly RabbitMqContainer _rmqContainer;

    public CustomWebApplicationFactory()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithDatabase("feed")
            .WithUsername("testuser")
            .WithPassword("testpw")
            .WithImage("postgres:15.1")
            .WithExposedPort(5432)
            .WithPortBinding(5432, true)
            .WithCleanUp(true)
            .Build();

         _rmqContainer = new RabbitMqBuilder()
            .WithUsername("testuser")
            .WithPassword("testpw")
            .WithImage("rabbitmq:3.7.28")
            .WithExposedPort(5672)
            .WithCleanUp(true)
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("development");

        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
            .AddInMemoryCollection(new Dictionary<string, string?>()
            {
                ["ConnectionStrings:Postgres"] = _postgresContainer.GetConnectionString(),
                ["RabbitMq:Uri"] = _rmqContainer.GetConnectionString(),
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

            var workerDescriptor = services.SingleOrDefault(s => s.ImplementationType == typeof(StoryFetcher));
            
            if (workerDescriptor is not null) 
            {
                services.Remove(workerDescriptor);
            }

            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (dbContextDescriptor is not null) 
            {
                services.Remove(dbContextDescriptor);
            }

            services.AddDbContext<AppDbContext>(options => 
                options.UseNpgsql(_postgresContainer.GetConnectionString()));
        });
    }

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
        await _rmqContainer.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
        await _rmqContainer.DisposeAsync();
    }
}