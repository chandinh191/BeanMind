using Infrastructure.Common.Email;
using Infrastructure.Data;
using Domain.Entities;
using Infrastructure.Services.Impl;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("ConnectionString 'DefaultConnection' not found");
        //var sqliteConnectionString = configuration.GetConnectionString("SqliteConnection") ?? throw new ArgumentNullException("ConnectionString 'SqliteConnection' not found");
        var applicationName = configuration.GetValue<string>("ApplicationName") ?? throw new ArgumentNullException("ApplicationName not found");

        // register db context
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
            //opt.UseSqlite(sqliteConnectionString);
        });

        // interface for ApplicationDbContext
        //services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>()); // using factory method to instantiate ApplicationDbContext
        services.AddScoped<ApplicationDbContext>();

        // ApplicationDbContextInitialiser to seed database and pre-action
        services.AddScoped<ApplicationDbContextInitialiser>();

        // setup AspNetCore.Identity
        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()                                                   // no customization IdentityRole from AspNetCore.Identity
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddTokenProvider(applicationName, typeof(DataProtectorTokenProvider<ApplicationUser>))
            .AddDefaultTokenProviders()
            .AddSignInManager();
        //.AddApiEndpoints();

        // email sender service implementation
        //services.AddTransient<IEmailSender<ApplicationUser>, EmailSender>();

        // don't know why we need it yet
        services.AddSingleton(TimeProvider.System);

        // register Email service
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }

    public static void AddSmtpConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection("SmtpSettings").Get<SmtpSettings>());
    }
}
    