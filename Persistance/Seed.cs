using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.User;
using System.Reflection;
using Domain;
using Microsoft.Extensions.Logging;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Persistance
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<User> userManager, ILogger<Seed> logger)
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
            await context.SaveChangesAsync();
        }
    }
}