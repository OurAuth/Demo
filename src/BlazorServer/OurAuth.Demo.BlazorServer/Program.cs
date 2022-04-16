using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using OurAuth.Demo.BlazorServer.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(o =>
    {
        o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect(o =>
    {
        o.Authority = builder.Configuration["OurAuth:Authority"];
        o.RequireHttpsMetadata = true;
        o.ClientId = builder.Configuration["OurAuth:ClientId"];
        o.ClientSecret = builder.Configuration["OurAuth:ClientSecret"];
        o.ResponseType = "code";

        o.Scope.Clear();
        o.Scope.Add("openid");
        o.Scope.Add("profile");
        o.Scope.Add("ourauth.demo.api");
        o.Scope.Add("ourauth.demo.api.local");

        o.SaveTokens = true;
        o.GetClaimsFromUserInfoEndpoint = true;

        o.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "name",
            RoleClaimType = "role"
        };

        o.UsePkce = true;
        o.Events = new OpenIdConnectEvents
        {
            OnAccessDenied = ctx =>
            {
                ctx.HandleResponse();
                ctx.Response.Redirect("/");
                return Task.CompletedTask;
            },
            //OnMessageReceived = ctx =>
            //{
            //    ctx.Properties.IsPersistent = true;
            //    ctx.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(12));
            //    return Task.CompletedTask;
            //},
            //OnRedirectToIdentityProvider = ctx =>
            //{
            //    //ctx.ProtocolMessage.RedirectUri = "https://localhost:5001/signin-oidc";
            //    return Task.CompletedTask;
            //}
        };
    });

builder.Services.AddTransient<BlazorTimerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
