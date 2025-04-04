using System.Text;
using LicenseService.Domain.Exceptions;
using LicenseService.Domain.Helpers;
using LicenseService.Domain.Interfaces;
using LicenseService.Domain.Services;
using LicenseService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace LicenseService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddCors();
        builder.Services.AddControllers();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
            options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JwtBearer:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JwtBearer:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["JwtBearer:TokenSecret"] ??
                                                   throw new NotFoundException("Appsetting",
                                                       "token secret")))
                };
            });

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "LicenseService API",
                    Description = "Calls regarding the licenses"
                });
            });

        

        // Required to use custom claims
        System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        Startup(builder);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LicenseService v1"));
        }

        app.UseCors(options =>
            options.WithOrigins("http://localhost:4200", "http://localhost:4201")
                .AllowAnyHeader()
                .AllowAnyMethod());

        app.UseHttpsRedirection();

        app.UseRouting();
        
        app.UseAuthentication();
        
        app.UseAuthorization(); 
        
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        app.MapControllers();

        app.Run();
    }

    private static void Startup(WebApplicationBuilder builder)
    {
        builder.Host.UseSystemd()
            .UseWindowsService();
        builder.Host.ConfigureAppConfiguration(
            (hostingContext, configuration) =>
            {
                var env = hostingContext.HostingEnvironment;

                configuration.AddJsonFile($"{env.ApplicationName}.json", optional: true,
                    reloadOnChange: true);
                configuration.AddJsonFile($"{env.ApplicationName}.{env.EnvironmentName}.json",
                    optional: true, reloadOnChange: true);
                configuration.AddEnvironmentVariables();
            });

        builder.Host.ConfigureServices(
            (hostContext, services) =>
            {
                services.AddRouting(options => options.LowercaseUrls = true);
                services.AddScoped<ILicenseService, Domain.Services.LicenseService>();
                services.AddScoped<ILicenseRepository, PostgresLicenseRepository>();
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<IUserRepository, PostgresUserRepository>();
                services.AddScoped<IRsaHelper, RsaHelper>();
                services.AddScoped<ICertificateService, CertificateService>();
                services.AddScoped<ITokenGenerator, TokenGenerator>();
                services.AddScoped<ITokenHelper, TokenHelper>();
                services.AddSingleton<IEmailService, EmailService>();
                services.AddSingleton<IHostedService, WatchdogService>();
                services.AddSingleton<IWatchdogHelper, WatchdogHelper>();

                services.AddDbContext<DatabaseContext>(options =>
                    options.UseNpgsql(
                        hostContext.Configuration.GetConnectionString("LicenseService")));
            });
    }
}
