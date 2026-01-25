using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Tests.Integration;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Only authenticate when the incoming request includes an Authorization header
        // with the scheme 'Test'. Otherwise, do not authenticate
        if (!Request.Headers.TryGetValue("Authorization", out StringValues value))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var authHeader = value.ToString();
        if (!authHeader.StartsWith("Test", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var claims = new List<Claim> 
        { 
            new(ClaimTypes.Name, "Test user"),
            new(ClaimTypes.NameIdentifier, "12345")
        };
        
        if (Request.Headers.TryGetValue("X-Test-Admin", out StringValues adminHeader) 
            && adminHeader.ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
        {
            claims.Add(new(ClaimTypes.Role, "Admin"));
        }
        
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}