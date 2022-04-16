using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace OurAuth.Demo.Api.IntegrationTests;

#pragma warning disable CS1591

/// <summary>
/// An AuthenticationHandler service as a Bearer scheme, normally named 'TestBearer'
/// </summary>
/// <seealso href="https://stackoverflow.com/questions/69188522/how-to-mock-jwt-bearer-token-for-integration-tests"/>
/// <seealso cref="JwtBearerHandler"/>
public class TestBearerAuthHandler : AuthenticationHandler<JwtBearerOptions>
{
    public TestBearerAuthHandler(IOptionsMonitor<JwtBearerOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        await Task.FromResult(0);

        var authorization = Request.Headers[HeaderNames.Authorization];
        if (!AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
        {
            return AuthenticateResult.NoResult();
        }

        var scheme = headerValue.Scheme;
        var token = headerValue.Parameter;

        if (string.CompareOrdinal(scheme, Scheme.Name) != 0)
        {
            return AuthenticateResult.NoResult();
        }

        // For test, we accept "TestToken" or "BobToken" only
        // Build ticket
        var claims = token switch
        {
            "TestToken" => TestUser,
            "BobToken" => Bob,
            _ => null
        };
        if (claims == null)
            return AuthenticateResult.Fail("Invalid token");

        var identity = new ClaimsIdentity(claims, "TestBearerType");
        var principal = new ClaimsPrincipal(identity);
        var tokenValidatedContext = new TokenValidatedContext(Context, Scheme, Options)
        {
            Principal = principal,
            SecurityToken = new JwtSecurityToken(claims: claims)
        };
        tokenValidatedContext.Properties.ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(7200);
        tokenValidatedContext.Properties.IssuedUtc = DateTimeOffset.UtcNow;
        if (Options.SaveToken)
        {
            tokenValidatedContext.Properties.StoreTokens(new[]
            {
                new AuthenticationToken { Name = "access_token", Value = token }
            });
        }
        tokenValidatedContext.Success();
        return tokenValidatedContext.Result;
    }

    private static Claim[] TestUser = new[]
    {
        new Claim(ClaimTypes.Name, "TestUserName"),
        new Claim(ClaimTypes.NameIdentifier, "TestUserId"),
        new Claim(ClaimTypes.Role, "TestRoleName"),

        new Claim(JwtClaimTypes.Subject, "TestUserId"),
        new Claim(JwtClaimTypes.Name, "TestUserName"),
        new Claim(JwtClaimTypes.Role, "TestRoleName")
    };

    private static Claim[] Bob = new[]
    {
        new Claim(ClaimTypes.Name, "Bob"),
        new Claim(ClaimTypes.NameIdentifier, "BobId"),
        new Claim(ClaimTypes.Role, "TestRoleName"),

        new Claim(JwtClaimTypes.Subject, "BobId"),
        new Claim(JwtClaimTypes.Name, "Bob"),
        new Claim(JwtClaimTypes.Role, "TestRoleName")
    };
}

