using Hangfire;

namespace HackerNewsCommentsFeed.DI;

public static class Middleware
{
    public static IApplicationBuilder UseMiddleware(this IApplicationBuilder app)
    {
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHangfireDashboard();

        return app;
    }
}