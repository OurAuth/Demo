using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurAuth.Demo.Api.Services;
using OurAuth.Demo.Api.Shared;

namespace OurAuth.Demo.Api.Controllers;

/// <summary>
/// The WeatherForecast WebApi.
/// </summary>
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastService _weatherForecastService;

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="weatherForecastService"></param>
    public WeatherForecastController(ILogger<WeatherForecastController> logger,
        IWeatherForecastService weatherForecastService)
    {
        _logger = logger;
        _weatherForecastService = weatherForecastService;
    }

    /// <summary>
    /// Gets the weather forecast (via default scheme).
    /// </summary>
    /// <remarks></remarks>
    /// <returns></returns>
    [ProducesDefaultResponseType]
    [HttpGet("Anonymous", Name = "GetWeatherForecastAnonymous")]
    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastAnonymous()
    {
        //Debug.Assert(HttpContext.User.Identity.IsAuthenticated);

        var result = await _weatherForecastService.GetAsync();

        _logger.LogDebug("Response: {json}", JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
        return result;
    }

    /// <summary>
    /// Gets the weather forecast (via default scheme).
    /// </summary>
    /// <remarks></remarks>
    /// <returns></returns>
    [Authorize]
    [ProducesDefaultResponseType]
    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast()
    {
        Debug.Assert(HttpContext.User.Identity.IsAuthenticated);

        var result = await _weatherForecastService.GetAsync();

        _logger.LogDebug("Response: {json}", JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
        return result;
    }

    /// <summary>
    /// Gets the weather forecast (via Bearer scheme).
    /// </summary>
    /// <returns></returns>
    [Authorize("BobOnly")]
    [ProducesDefaultResponseType]
    [HttpGet("BobOnly", Name = "GetWeatherForecastBearer")]
    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastBearer()
    {
        Debug.Assert(HttpContext.User.Identity.IsAuthenticated);

        var result = await _weatherForecastService.GetAsync();

        _logger.LogDebug("Response: {json}", JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
        return result;
    }
}
