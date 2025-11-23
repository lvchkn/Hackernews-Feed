using Api.Controllers;
using Api.Middleware;
using Api.ServiceCollectionExtensions;
using Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

app.AddMiddleware(builder.Environment);

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.MapItemsEndpoints()
    .MapAuthEndpoints()
    .MapUsersEndpoints()
    .MapInterestsEndpoints();

app.Run();

namespace Api
{
    public partial class Program {}
}
