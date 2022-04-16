using FluentAssertions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace OurAuth.Demo.Api.IntegrationTests
{
    public class UnauthorizedTests : DefaultClassFixture
    {
        public UnauthorizedTests(ProgramWebApplicationFactory factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("WeatherForecast/Anonymous")]
        public async Task Anonymous_ShouldSuccess(string url)
        {
            var Client = CreateEmptyClient(false);
            var response = await Client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("UserInfo")]
        [InlineData("WeatherForecast")]
        [InlineData("WeatherForecast/BobOnly")]
        public async Task Secured_Should401(string url)
        {
            var Client = CreateEmptyClient(false);
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}