using Microsoft.AspNetCore.Authentication.JwtBearer;
using OurAuth.Demo.Api.Services;

namespace OurAuth.Demo.Api;

/// <summary>
/// 
/// </summary>
public static class HostExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.Authority = builder.Configuration["OurAuth:Authority"];
                o.Audience = builder.Configuration["OurAuth:Audience"];
            });
        builder.Services.AddSingleton<IWeatherForecastService, WeatherForecastService>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.ConfigureServicesForSwagger();

        // Add cors
        builder.Services.AddCors(o =>
        {
            o.AddPolicy("AllowAny", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });

        // Add authorization policies
        builder.Services.AddAuthorizationPolicies();

        return builder;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication Configure(this WebApplication app)
    {
        app.ConfigureForSwagger();

        app.UseHttpsRedirection();

        app.UseRouting();

        // UseCors must be placed after UseRouting, but before UseAuthorization
        app.UseCors("AllowAny");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        //var fullAccessRoleName = "Administrators";
        //services.AddAuthorization(options =>
        //{
        //    options.AddPolicy("full_access",
        //        policy =>
        //            policy.RequireAssertion(context => context.User.HasClaim(c =>
        //                    (c.Type == JwtClaimTypes.Role && c.Value == fullAccessRoleName) ||
        //                    (c.Type == $"client_{JwtClaimTypes.Role}" && c.Value == fullAccessRoleName)
        //                )
        //            ));
        //});

        // This is for demo a test only!
        // '/WeatherForecast/Bob' requires bob only
        services.AddAuthorization(options =>
        {
            options.AddPolicy("BobOnly", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireUserName("Bob");
            });
        });

        return services;
    }
}
