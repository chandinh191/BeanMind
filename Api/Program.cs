using Application;
using Infrastructure;
using Infrastructure.Data;
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

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();


        app.UseSwagger();
        app.UseSwaggerUI();
        //await InitialiserExtensions.InitialiseDatabaseAsync(app.Services);


        //app.UseHttpsRedirection();

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
