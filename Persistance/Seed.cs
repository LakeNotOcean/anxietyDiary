using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.User;
using System.Reflection;
using Domain;
using Microsoft.Extensions.Logging;
using Domain.Enums;

namespace Persistance
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, ILogger<Seed> logger)
        {
            logger.LogInformation("SeedData started");
            if (context.Roles.Any())
            {
                return;
            }
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
            await context.SaveChangesAsync();

        }
    }
}