using Microsoft.OpenApi.Models;
using System.Reflection;

namespace OurAuth.Demo.Api;

/// <summary>
/// 
/// </summary>
internal static class SwaggerExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    internal static WebApplicationBuilder ConfigureServicesForSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(o =>
        {
            o.SwaggerDoc("v1", new OpenApiInfo { Title = "OurAuth.Demo.Api", Version = "v1" });

            // Add xml comments;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            // Add OAuth2
            o.AddSecurityDefinition(nameof(SecuritySchemeType.OAuth2), new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(new Uri(builder.Configuration["OurAuth:Authority"]), "connect/authorize"),
                        TokenUrl = new Uri(new Uri(builder.Configuration["OurAuth:Authority"]), "connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            [builder.Configuration["OurAuth:Audience"]] = builder.Configuration["OurAuth:Audience"]
                        }
                    }
                }
            });
            o.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = nameof(SecuritySchemeType.OAuth2)
                        }
                    },
                    new string[]{ builder.Configuration["OurAuth:Audience"] }
                }
            });
        });

        return builder;
    }

    internal static WebApplication ConfigureForSwagger(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "OurAuth.Demo.Api v1");

                o.OAuthAppName(app.Configuration["SwaggerUI:OAuthClientId"]);
                o.OAuthClientId(app.Configuration["SwaggerUI:OAuthClientId"]);
                o.OAuthClientSecret(app.Configuration["SwaggerUI:OAuthClientSecret"]);
                o.OAuthScopes(app.Configuration["OurAuth:Audience"]);
                o.OAuthUsePkce();
            });
        }

        return app;
    }

}
