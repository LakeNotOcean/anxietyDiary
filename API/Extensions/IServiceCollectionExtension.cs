using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Persistance;
using Microsoft.Extensions.Logging;
using System;
using API.Services;
using MediatR;
using Api.CRUD;
using API.Extensions;

namespace API.Controllers.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddLogging(loggerBuilder =>
            {
                loggerBuilder.ClearProviders();
                loggerBuilder.AddConsole();
            });
            services.AddDbContext<DataContext>(opt =>
            {
                Console.WriteLine(config.GetConnectionString("DefaultConnection"));
                var serverVersion = new MySqlServerVersion(new System.Version(8, 0, 27));
                opt.UseMySql(config.GetConnectionString("DefaultConnection"),
                             serverVersion,
                             b => b.UseMicrosoftJson()).LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:3000").AllowAnyMethod().WithExposedHeaders("WWW-Authenticate", "Pagination").AllowAnyOrigin().AllowAnyHeader();
                });
            });


            services.AddSingleton<DiaryService>(sp =>
            {
                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
                    return new DiaryService(db.GetDbSetDiariesTypes());
                }
            });
            services.AddMediatR(typeof(List.Handler).Assembly);
            return services;
        }
    }
}