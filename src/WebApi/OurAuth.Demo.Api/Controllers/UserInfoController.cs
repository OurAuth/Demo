using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurAuth.Demo.Api.Shared;

namespace OurAuth.Demo.Api.Controllers;

/// <summary>
/// The UserInfo WebApi.
/// </summary>
[ApiController]
[Route("[controller]")]
public class UserInfoController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="logger"></param>
    public UserInfoController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Gets current user info.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [ProducesDefaultResponseType]
    [HttpGet]
    public UserInfo GetUserInfo()
    {
        var userInfo = new UserInfo
        {
            Identity = new UserInfoIdentity
            {
                IsAuthenticated = HttpContext.User.Identity.IsAuthenticated,
                Name = HttpContext.User.Identity.Name,
                AuthenticationType = HttpContext.User.Identity.AuthenticationType
            },
            Claims = HttpContext.User.Claims.Select(x => new UserInfoClaim
            {
                Type = x.Type,
                ValueType = x.ValueType,
                Value = x.Value,
                OriginalIssuer = x.OriginalIssuer,
                Issuer = x.Issuer
            })
        };

        _logger.LogDebug("Response: {json}", JsonSerializer.Serialize(userInfo, new JsonSerializerOptions { WriteIndented = true }));
        return userInfo;
    }
}
