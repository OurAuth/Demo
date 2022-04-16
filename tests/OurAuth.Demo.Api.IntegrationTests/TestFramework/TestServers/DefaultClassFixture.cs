using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;

namespace OurAuth.Demo.Api.IntegrationTests;

public class DefaultClassFixture : IClassFixture<ProgramWebApplicationFactory>
{
    protected readonly ProgramWebApplicationFactory Factory;

    public DefaultClassFixture(ProgramWebApplicationFactory factory)
    {
        Factory = factory;
    }

    public virtual HttpClient CreateEmptyClient(bool allowAutoRedirect)
    {
        var client = Factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => { });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = allowAutoRedirect
            });

        //client.DefaultRequestHeaders.Clear();
        return client;
    }

    public virtual HttpClient CreateTestAuthClient(bool allowAutoRedirect)
    {
        var client = Factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("TestAuthScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestAuthScheme", options => { });

                services.AddAuthorization();
            });
        })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = allowAutoRedirect
            });

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestAuthScheme");

        return client;
    }

    public virtual HttpClient CreateTestBearerClient(bool allowAutoRedirect)
    {
        var client = Factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("TestBearer")
                    .AddScheme<JwtBearerOptions, TestBearerAuthHandler>("TestBearer", options => { });
            });
        })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = allowAutoRedirect
            });

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestBearer", "TestToken");

        return client;
    }

    public virtual HttpClient CreateBobBearerClient(bool allowAutoRedirect)
    {
        var client = Factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("TestBearer")
                    .AddScheme<JwtBearerOptions, TestBearerAuthHandler>("TestBearer", options => { });
            });
        })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = allowAutoRedirect
            });

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestBearer", "BobToken");

        return client;
    }
}
