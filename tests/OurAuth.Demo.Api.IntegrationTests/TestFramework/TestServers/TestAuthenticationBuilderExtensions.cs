using Microsoft.AspNetCore.Builder;

namespace OurAuth.Demo.Api.IntegrationTests;

public static class TestAuthenticationBuilderExtensions { 

    public static IApplicationBuilder UseAuthentication(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseMiddleware<TestAuthenticationMiddleware>();
        return app;
    }
}

