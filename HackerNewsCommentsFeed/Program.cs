using HackerNewsCommentsFeed.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();
app.UseMiddleware(builder.Configuration);

app.MapEndpoints();

app.Run();

public partial class Program {}
