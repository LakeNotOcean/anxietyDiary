using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Persistance;
using Microsoft.AspNetCore.Identity;
using Domain.User;
using API.Extensions;
using Persistance.Lib;

namespace anxietyDiary
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<DataContext>();
                await Seed.SeedDescriptionData(context);
                var dbSetDiaries = context.GetDbSetDiariesTypes();
                foreach (var diary in dbSetDiaries)
                {
                    await DiariesCheck.Check(diary.Value.PropertyTypeInfo, context);
                }
                context.Database.Migrate();
                await Seed.SeedDiaryData(context);
                var userManager = services.GetRequiredService<UserManager<User>>();
                await Seed.SeedUserData(context, userManager, services.GetRequiredService<ILogger<Seed>>());
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "migration error");
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
