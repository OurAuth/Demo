using FluentAssertions;
using OurAuth.Demo.Shared;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace OurAuth.Demo.Api.IntegrationTests
{
    public class TestBearerAuthorizedTests : DefaultClassFixture
    {
        public TestBearerAuthorizedTests(ProgramWebApplicationFactory factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("WeatherForecast")]
        public async Task Authorized_Should200(string url)
        {
            var Client = CreateTestBearerClient(false);

            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task WeatherForecastBearer_ShouldSuccess()
        {
            var Client = CreateTestBearerClient(false);

            var result = await Client.GetFromJsonAsync<WeatherForecast[]>("WeatherForecast");

            // Assert
            result.Should().NotBeNull();
            result.Length.Should().BeGreaterThan(0);
        }
    }
}