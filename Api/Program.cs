using Api.Controllers;
using Api.Middleware;
using Api.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

app.AddMiddleware();

app.MapItemsEndpoints()
    .MapAuthEndpoints()
    .MapUsersEndpoints()
    .MapInterestsEndpoints();

app.Run();
namespace Api
{
    public partial class Program {}
}
