using IdentityModel;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;

namespace OurAuth.Demo.Api.IntegrationTests;

public static class TestHttpClientExtensions
{
    public static void SetAdminClaimsViaHeaders(this HttpClient client)
    {
        var claims = new[]
            {
                new Claim(JwtClaimTypes.Subject, Guid.NewGuid().ToString()),
                new Claim(JwtClaimTypes.Name, Guid.NewGuid().ToString()),
                new Claim(JwtClaimTypes.Role, "Administrators")
            };

        var token = new JwtSecurityToken(claims: claims);
        var t = new JwtSecurityTokenHandler().WriteToken(token);
        client.DefaultRequestHeaders.Add(TestAuthenticationMiddleware.TestAuthorizationHeader, t);
    }
}
