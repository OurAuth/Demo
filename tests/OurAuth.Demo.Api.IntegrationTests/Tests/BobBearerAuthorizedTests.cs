using FluentAssertions;
using OurAuth.Demo.Shared;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace OurAuth.Demo.Api.IntegrationTests
{
    public class BobBearerAuthorizedTests : DefaultClassFixture
    {
        public BobBearerAuthorizedTests(ProgramWebApplicationFactory factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("WeatherForecast/BobOnly")]
        public async Task Authorized_BobOnly_Should200(string url)
        {
            var Client = CreateBobBearerClient(false);

            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task WeatherForecastBearer_BobOnly_ShouldSuccess()
        {
            var Client = CreateBobBearerClient(false);

            var result = await Client.GetFromJsonAsync<WeatherForecast[]>("WeatherForecast/BobOnly");

            // Assert
            result.Should().NotBeNull();
            result.Length.Should().BeGreaterThan(0);
        }
    }
}