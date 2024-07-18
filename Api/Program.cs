using Application;
using Infrastructure;
using Infrastructure.Data;
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

        // Add authentication services
    /*    builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
        .AddGoogle(options =>
        {
            options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
            options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
        });
*/

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        await InitialiserExtensions.InitialiseDatabaseAsync(app.Services);

  /*      app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
        { 
            ClientId = "987146834868-oatrvf1gf2d0sevte0uum9ik0jtq3bsm.apps.googleusercontent.com",
            ClientSecret = "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"
        });*/

        app.UseCors(cfg =>
        {
            cfg.AllowAnyHeader();
            cfg.AllowAnyMethod();
            cfg.AllowAnyOrigin();
        });

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
