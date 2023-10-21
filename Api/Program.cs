using HackerNewsCommentsFeed.Controllers;
using HackerNewsCommentsFeed.Middleware;
using HackerNewsCommentsFeed.ServiceCollectionExtensions;

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
public partial class Program {}
