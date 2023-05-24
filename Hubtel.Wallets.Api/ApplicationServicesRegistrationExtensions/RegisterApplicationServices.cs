using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System;
using Hubtel.Wallets.Api.Configurations;
using Hubtel.Wallets.Api.DataAccess;
using Hubtel.Wallets.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using Hubtel.Wallets.Api.Dtos;
using Hubtel.Wallets.Api.Interfaces;
using Hubtel.Wallets.Api.Services;
using Hubtel.Wallets.Api.Validations;

namespace Hubtel.Wallets.Api.ApplicationServicesRegistrationExtensions
{
    public static class RegisterApplicationServices
    {
        public static IServiceCollection ConfigureSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Hubtel Wallet Api",
                    Description = "An API to enable user wallet management",
                    Contact = new OpenApiContact
                    {
                        Name = "Clinton Asiedu Boamah",
                        Email = "clinton.boamah@outlook.com",
                        Url = new Uri("https://github.com/1973-clinton"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Apache License, 2.0",
                        Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0"),
                    }
                });

                options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string in this format: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },

                        new List<string>()
                    }
                });
            });

            return services;
        }

        public static IServiceCollection ConfigureDbContextAndIdentityServices(this  IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection ConfigureAuthenticationAndAuthorizationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Jwt>(configuration.GetSection("Jwt"));
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
            });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

            return services;
        }

        public static IServiceCollection ConfigureApplicationDomainModelServices(this IServiceCollection services)
        {
            services.AddScoped<IValidator<WalletDto>, WalletValidator>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
