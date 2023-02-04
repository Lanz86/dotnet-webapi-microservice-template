using MicroserviceTemplate.Application.Common.Interfaces;
using MicroserviceTemplate.Application;
using MicroserviceTemplate.Infrastructure;
using MicroserviceTemplate.WebApi.Filters;
using MicroserviceTemplate.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using MicroserviceTemplate.WebApi.Results;
using LnzSoftware.Swashbuckle.GenericTypeResponseFilter;

namespace MicroserviceTemplate.WebApi;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddHttpContextAccessor();
        services.AddLogging();

        var allowedHost = configuration.GetValue<string>("Cors:ValidHost").Split(',');
        services.AddCors(options =>
        {
            options.AddPolicy(name: configuration.GetValue<string>("Cors:Policy"),
                            policy =>
                            {
                                policy.WithOrigins(allowedHost).AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
                            });
        });


        services.AddControllersWithViews(options =>
            options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration.GetValue<string>("ServicesEndpoint:IdentityServerMicroservice");
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = configuration.GetValue("IdentityServer:TokenValidationParameters:ValidateAudience", false),
                        ValidateIssuer = configuration.GetValue("IdentityServer:TokenValidationParameters:ValidateIssuer", false)
                    };
                });

        services.AddSwaggerGen(c =>
        {
            c.OperationFilter<GenericTypeResponseFilter>(typeof(ApiResult<>));
            c.SwaggerDoc("v1", new OpenApiInfo { Title = configuration.GetValue<string>("Swagger:MicroserviceApiTitle"), Version = "v1" });
            c.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
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

        return services;
    }
}
