using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace OurAuth.Demo.Api.IntegrationTests;

#pragma warning disable CS1591

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        await Task.FromResult(0);

        //if (Scheme.Name != "TestAuthScheme")
        //    return AuthenticateResult.NoResult();

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "TestUserName"),
            new Claim(ClaimTypes.NameIdentifier, "TestUserId"),
            new Claim(ClaimTypes.Role, "TestRoleName"),

            new Claim(JwtClaimTypes.Subject, "TestUserId"),
            new Claim(JwtClaimTypes.Name, "TestUserName"),
            new Claim(JwtClaimTypes.Role, "TestRoleName")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestAuthScheme");

        return AuthenticateResult.Success(ticket);
    }
}

