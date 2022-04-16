using OurAuth.Demo.Api.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurAuth.Demo.Api.Services
{
    /// <summary>
    /// The weather forecast interface.
    /// </summary>
    public interface IWeatherForecastService
    {
        /// <summary>
        /// Gets the weather forecast.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<WeatherForecast>> GetAsync();
    }
}
