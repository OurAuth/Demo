using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace OurAuth.Demo.Api.IntegrationTests;

public class SerilogWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        return base.CreateHost(builder);
    }

    protected override IWebHostBuilder CreateWebHostBuilder()
    {
        return base.CreateWebHostBuilder()
            ?.UseSerilog();
    }

    protected override IHostBuilder CreateHostBuilder()
    {
        return base.CreateHostBuilder()
            ?.UseSerilog();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseSerilog();
    }

    protected override TestServer CreateServer(IWebHostBuilder builder)
    {
        builder.UseSerilog();
        return base.CreateServer(builder);
    }
}
