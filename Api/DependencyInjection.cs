using Application.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddWebService(this IServiceCollection services, IConfiguration configuration) { 
        var jwtIssuer = configuration.GetValue<string>("Jwt:Issuer");
        var jwtAudience = configuration.GetValue<string>("Jwt:Audience");
        var jwtSecretKey = configuration.GetValue<string>("Jwt:SecretKey");
        //var jwtExpiresTime = configuration.GetValue<int>("Jwt:ExpiresIn");

        // add authentication scheme
        services.AddAuthentication(opts => opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Bearer", opts =>
        {
            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey!)),
                ValidateLifetime = true,
            };
        }).AddIdentityCookies();

        // register HttpContextAccessor
        services.AddHttpContextAccessor();

        // setup JWT swagger to perform authentication
        AddSwaggerAuthentication(services);

        // add controller
        services.AddControllers().ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = ValidationErrorResponseFactory.Initialize;
        }).AddJsonOptions(conf =>
        {
            conf.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        return services;
    }

    public static void AddSwaggerAuthentication(IServiceCollection services)
    {
        services.AddSwaggerGen(setup =>
        {
            setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer YOUR_TOKEN')",
                Name = "Authorization",
                In = ParameterLocation.Header,
                //Type = SecuritySchemeType.ApiKey,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}
