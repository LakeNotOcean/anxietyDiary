using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.User;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Domain.DiaryExpensions;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistance
{
    public class Seed
    {
        public static async Task SeedUserData(DataContext context, UserManager<User> userManager, ILogger<Seed> logger)
        {
            logger.LogInformation("SeedData started");
            if (!context.Roles.Any())
            {
                var roles = new List<Role> { };
                foreach (RolesEnum role in (RolesEnum[])RolesEnum.GetValues(typeof(RolesEnum)))
                {
                    roles.Add(new Role
                    {
                        Id = (int)role,
                        Name = role.GetDescription()
                    });
                }
                logger.LogInformation("roles created");
                await context.Roles.AddRangeAsync(roles);
            }


            if (!context.Users.Any())
            {
                var userNames = new List<string> { "first", "second", "third", "fourth", "fifth", "sixth", "seventh", "cheshirskins" };
                var firstUsersNames = new List<string> { "Александр", "Илья", "Платон", "Василиса", "Дмитрий", "Милана", "Александр", "Владислав" };
                var secodnUsersNames = new List<string> { "Данилов", "Суханов", "Попов", "Алексеева", "Комаров", "Круглова", "Щукин", "Невский" };
                var users = new List<User> { };
                for (int i = 0; i < 7; ++i)
                {
                    await userManager.CreateAsync(new User
                    {
                        UserName = userNames[i],
                        FirstName = firstUsersNames[i],
                        SecondName = secodnUsersNames[i],
                        RoleId = 2,
                        Email = $"test{i}@test.com"
                    }, "12345Nn.");
                }
                await userManager.CreateAsync(new User
                {
                    UserName = "cheshirskins",
                    FirstName = "Владислав",
                    SecondName = "Невский",
                    RoleId = 4,
                    Email = "cheshirskins69@gmail.com"
                }, "12345Nn.");

            };
        }
        public static async Task SeedDescriptionData(DataContext context)
        {
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/categories.json";
            var Categories = readSeed<DiaryCategory>(path);
            foreach (var cat in Categories)
            {
                if (!context.Categories.Any(c => c.Id == cat.Id))
                {
                    await context.AddAsync(cat);
                }
            }
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverterWithAttributeSupport(null, false, false, false, true));
            path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/descriptions.json";

            var Descriptions = readSeed<DiaryDescription>(path, options);
            foreach (var descr in Descriptions)
            {
                if (!context.Descriptions.Any(d => d.ShortName == descr.ShortName))
                {
                    await context.AddAsync(descr);
                }
                else
                {
                    var currentDescription = await context.Descriptions.Include(d => d.ArbitraryColumns)
                        .Include(d => d.NonArbitraryColumns)
                        .Where(d => d.ShortName == descr.ShortName)
                        .SingleOrDefaultAsync();
                    foreach (var currCol in currentDescription.ArbitraryColumns)
                    {
                        var col = descr.ArbitraryColumns.Where(c => c.ShortName == currCol.ShortName).SingleOrDefault();
                        if (col is not null)
                        {
                            col.Id = currCol.Id;
                            context.Entry(currCol).CurrentValues.SetValues(col);
                        }
                        else
                        {
                            context.Remove(currCol);
                        }
                    }
                    foreach (var currCol in currentDescription.NonArbitraryColumns)
                    {
                        var col = descr.NonArbitraryColumns.Where(c => c.ShortName == currCol.ShortName).SingleOrDefault();
                        if (col is not null)
                        {
                            col.Id = currCol.Id;
                            context.Entry(currCol).CurrentValues.SetValues(col);
                        }
                        else
                        {
                            context.Remove(currCol);
                        }
                    }
                }

            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedDiaryData(DataContext context)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverterWithAttributeSupport(null, false, false, false, true));
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/descriptions.json";
            var Descriptions = readSeed<DiaryDescription>(path, options);
            var UserDoctors = await context.UserDoctors.ToListAsync();
            foreach (var descr in Descriptions)
            {
                foreach (var userDoctor in UserDoctors)
                {
                    var diaryView = await context.UsersViews.Where(uv => uv.DiaryName == descr.ShortName
                    && uv.UserDoctorId == userDoctor.Id).SingleAsync();
                    if (diaryView is null)
                    {
                        await context.AddAsync(new LastUserView
                        {
                            DiaryName = descr.ShortName,
                            UserDoctorId = userDoctor.Id
                        });
                    }
                }
            }
            await context.SaveChangesAsync();
        }
        private static List<T> readSeed<T>(string JsonDataPath, JsonSerializerOptions options = null) where T : class
        {
            List<T> source = new List<T> { };

            using (StreamReader r = new StreamReader(JsonDataPath))
            {
                string json = r.ReadToEnd();
                source = JsonSerializer.Deserialize<List<T>>(json, options);
            }
            return source;
        }
    }

}