using Application;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
namespace Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // register DI service from Application layer
        builder.Services.AddApplicationServices();

        // register DI service from Infrastructure layer
        builder.Services.AddInfrastructureService(builder.Configuration);

        // register DI service from API Layer
        builder.Services.AddWebService(builder.Configuration);

        // register SmtpConfiguration
        builder.Services.AddSmtpConfiguration(builder.Configuration);

        // register default Api Service
        //builder.Services.AddControllers();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddGoogle(options =>
        {
            options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
            options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
            options.SaveTokens = true;
        });


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        // Example of converting server time to local time
        // Assume server time is UTC and local time zone is Pacific Standard Time (PST)
        DateTime serverTime = DateTime.UtcNow.AddHours(14);

        // Specify the target time zone (e.g., Pacific Standard Time)
        TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

        // Convert the server time to the local time
        DateTime localTime = TimeZoneInfo.ConvertTime(serverTime, TimeZoneInfo.Utc, localTimeZone);

        // Log the converted time or use it as needed
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Server Time (UTC): {ServerTime}", serverTime);
        logger.LogInformation("Local Time (PST): {LocalTime}", localTime);

        app.UseSwagger();
        app.UseSwaggerUI();

        //await InitialiserExtensions.InitialiseDatabaseAsync(app.Services);

        app.UseCors(cfg =>
        {
            cfg.AllowAnyHeader();
            cfg.AllowAnyMethod();
            cfg.AllowAnyOrigin();
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
