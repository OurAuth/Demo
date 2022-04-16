﻿using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OurAuth.Demo.Api.IntegrationTests;

public class TestAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    public static readonly string TestAuthorizationHeader = "FakeAuthorization";
    public TestAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.Keys.Contains(TestAuthorizationHeader))
        {
            var token = context.Request.Headers[TestAuthorizationHeader].Single();
            var jwt = new JwtSecurityToken(token);
            var claimsIdentity = new ClaimsIdentity(jwt.Claims, IdentityServerAuthenticationDefaults.AuthenticationScheme, JwtClaimTypes.Name, JwtClaimTypes.Role);
            context.User = new ClaimsPrincipal(claimsIdentity);
        }

        await _next(context);
    }
}

