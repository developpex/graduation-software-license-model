using LicenseManager.Domain.Helpers;
using LicenseManager.Domain.Interfaces;
using LicenseManager.Domain.Services;
using LicenseManager.Infrastructure;
using LicenseManager.Infrastructure.Helpers;
using Microsoft.OpenApi.Models;

namespace LicenseManager;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddCors();
        builder.Services.AddControllers();

        /*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
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
            });*/

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "LicenseManager API",
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
                c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LicenseManager v1"));
        }

        app.UseCors(options =>
            options.WithOrigins("http://localhost:4201")
                .AllowAnyHeader()
                .AllowAnyMethod());

        app.UseHttpsRedirection();

        app.UseRouting();

        //app.UseAuthentication();

        //app.UseAuthorization();

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
                services.AddScoped<ICertificateService, CertificateService>();
                services.AddScoped<ICertificateRepository, CertificateRepository>();
                services.AddScoped<IUserService, UserService>();
                services.AddSingleton<IUserRepository, UserRepository>();
                services.AddSingleton<ICertificateHelper, CertificateHelper>();
                services.AddScoped<IRsaHelper, RsaHelper>();
                services.AddSingleton<IHostedService, CertificateGetter>();
            });
    }
}