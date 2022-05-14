using HackerNewsCommentsFeed.ApiConnections;
using HackerNewsCommentsFeed.Controllers;
using HackerNewsCommentsFeed.DI;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();
app.UseMiddleware(builder.Configuration);

RecurringJob.AddOrUpdate<ApiConnector>("Fetcher", job => job.GetLastComment(), Cron.Minutely);

app.MapItemsEndpoints()
    .MapAuthEndpoints()
    .MapUsersEndpoints();

app.Run();

