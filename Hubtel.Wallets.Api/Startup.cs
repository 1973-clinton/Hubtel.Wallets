using FluentValidation;
using Hubtel.Wallets.Api.ApplicationServicesRegistrationExtensions;
using Hubtel.Wallets.Api.Configurations;
using Hubtel.Wallets.Api.DataAccess;
using Hubtel.Wallets.Api.Dtos;
using Hubtel.Wallets.Api.Helpers;
using Hubtel.Wallets.Api.Interfaces;
using Hubtel.Wallets.Api.Models;
using Hubtel.Wallets.Api.Services;
using Hubtel.Wallets.Api.Validations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hubtel.Wallets.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDbContextAndIdentityServices(Configuration); // registers data layer and idenitity services
            services.ConfigureAuthenticationAndAuthorizationServices(Configuration); // registers auth services
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // registers automapper with mapping profile
            services.ConfigureApplicationDomainModelServices(); // registers dtos and model services
            services.ConfigureSwaggerServices();  // registers swagger services

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            SeedData.SeedAdminUser(userManager); //seeds an admin user if there isn't any in the data store

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hubtel Wallet Api");
                c.RoutePrefix = "";
            });

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
