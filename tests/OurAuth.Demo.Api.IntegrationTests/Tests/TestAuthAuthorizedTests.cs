using FluentAssertions;
using OurAuth.Demo.Shared;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace OurAuth.Demo.Api.IntegrationTests;

public class TestAuthAuthorizedTests : DefaultClassFixture
{
    public TestAuthAuthorizedTests(ProgramWebApplicationFactory factory) : base(factory)
    {
    }

    [Theory]
    [InlineData("UserInfo")]
    [InlineData("WeatherForecast")]
    public async Task Authorized_Should200(string url)
    {
        var Client = CreateTestAuthClient(false);
        Client.DefaultRequestHeaders.Clear();

        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UserInfo_ShouldSuccess()
    {
        var Client = CreateTestAuthClient(false);

        var result = await Client.GetFromJsonAsync<UserInfo>("UserInfo");

        // Assert
        result.Should().NotBeNull();
        result.Identity.IsAuthenticated.Should().BeTrue();
        result.Identity.Name.Should().Be("TestUserName");
    }

    [Fact]
    public async Task WeatherForecast_ShouldSuccess()
    {
        var Client = CreateTestAuthClient(false);

        var result = await Client.GetFromJsonAsync<WeatherForecast[]>("WeatherForecast");

        // Assert
        result.Should().NotBeNull();
        result.Length.Should().BeGreaterThan(0);
    }
}
