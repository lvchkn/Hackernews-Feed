using Application.ApiConnections;
using HackerNewsCommentsFeed.Configuration;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();
app.UseMiddleware(builder.Configuration);

RecurringJob.AddOrUpdate<ApiConnector>("Fetcher", job => job.GetLastComment(), Cron.Minutely);

app.MapEndpoints();

app.Run();

