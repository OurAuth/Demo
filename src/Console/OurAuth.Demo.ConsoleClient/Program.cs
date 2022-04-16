using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using OurAuth.Demo.Api.Shared;
using System.Text.Json;
using System.Net.Http.Json;

// 0.0.Configures the Console.
Console.Title = "OurAuth.Demo.Api.ConsoleClient";
var version = typeof(Program).Assembly?.GetName()?.Version?.ToString();
Console.WriteLine("Version: {0}", version);

// 0.1.Gets the environment name.
var envName = "Development";
if (args.Length > 0 && "Production".Equals(args[0], StringComparison.OrdinalIgnoreCase))
{
    envName = "Production";
}
Console.WriteLine("ASPNETCORE_ENVIRONMENT = {0}", envName);

// 0.2.Gets the app settings.
var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{envName}.json", true)
    .AddUserSecrets<Program>();
var Configuration = configurationBuilder.Build();

// appsettings
var authority = Configuration["OurAuth:Authority"];
var clientId = Configuration["OurAuth:ClientId"];
var clientSecret = Configuration["OurAuth:ClientSecret"];
var scope = Configuration["OurAuth:Scope"];
Console.WriteLine("OurAuth:Authority = {0}", authority);
Console.WriteLine("OurAuth:ClientId = {0}", clientId);
Console.WriteLine("OurAuth:ClientSecret = {0}", clientSecret);
Console.WriteLine("OurAuth:Scope = {0}", scope);
Console.WriteLine();
Console.WriteLine("Press any key to continue...");
Console.ReadKey();

// 1.1.disco
var client = new HttpClient();
var disco = await client.GetDiscoveryDocumentAsync(authority);
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
    return;
}

// 1.2.Gets the token.
Console.WriteLine("--------------------------");
Console.WriteLine("          Token           ");
Console.WriteLine("--------------------------");
var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = disco.TokenEndpoint,
    ClientId = clientId,
    ClientSecret = clientSecret,
    Scope = scope
});
if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
    return;
}
Console.WriteLine(JsonSerializer.Serialize(tokenResponse.Json, new JsonSerializerOptions { WriteIndented = true }));
Console.WriteLine();
Console.WriteLine("Press any key to continue...");
Console.ReadKey();

// 2.1.Calls /UserInfo
Console.WriteLine("--------------------------");
Console.WriteLine("         UserInfo         ");
Console.WriteLine("--------------------------");
try
{
    var apiClient = new HttpClient();
    apiClient.SetBearerToken(tokenResponse.AccessToken);
    apiClient.BaseAddress = new Uri(Configuration["DemoApi:BaseAddress"]);
    var userInfo = await apiClient.GetFromJsonAsync<UserInfo>("UserInfo");
    var userInfoJson = JsonSerializer.Serialize(userInfo, new JsonSerializerOptions { WriteIndented = true });
    Console.WriteLine(userInfoJson);
    Console.WriteLine();
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
    return;
}

// 2.2.Calls /WeatherForecast
Console.WriteLine("--------------------------");
Console.WriteLine("     WeatherForecast      ");
Console.WriteLine("--------------------------");
try
{
    var apiClient2 = new HttpClient();
    apiClient2.SetBearerToken(tokenResponse.AccessToken);
    apiClient2.BaseAddress = new Uri(Configuration["DemoApi:BaseAddress"]);
    var weather = await apiClient2.GetFromJsonAsync<IEnumerable<WeatherForecast>>("WeatherForecast");
    var weatherJson = JsonSerializer.Serialize(weather, new JsonSerializerOptions { WriteIndented = true });
    Console.WriteLine(weatherJson);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    Console.WriteLine();
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
    return;
}

Console.WriteLine();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
