using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Diagnostics;

namespace OurAuth.Demo.Api.IntegrationTests;

public static class SerilogExtensions
{
    public static IWebHostBuilder UseSerilog(this IWebHostBuilder host)
    {
#pragma warning disable CS0618
        return host.UseSerilog((hostContext, loggerConfig) =>
        {
            loggerConfig
                .WriteTo.Console()
                .ReadFrom.Configuration(hostContext.Configuration);
            //.ReadFrom.Services(services);
            //.Enrich.WithProperty("ApplicationName", hostContext.HostingEnvironment.ApplicationName);

            Debug.WriteLine($"UseSerilogOld() by {new StackTrace().GetFrame(2)?.GetMethod()?.DeclaringType?.FullName}");
        });
#pragma warning restore CS0618
    }

    public static IHostBuilder UseSerilog(this IHostBuilder host)
    {
        return host.UseSerilog((hostContext, services, loggerConfig) =>
        {
            loggerConfig
                .WriteTo.Console()
                .ReadFrom.Configuration(hostContext.Configuration)
                .ReadFrom.Services(services);
            //.Enrich.WithProperty("ApplicationName", hostContext.HostingEnvironment.ApplicationName);

            Debug.WriteLine($"UseSerilogNew() by {new StackTrace().GetFrame(2)?.GetMethod()?.DeclaringType?.FullName}");
        });
    }
}