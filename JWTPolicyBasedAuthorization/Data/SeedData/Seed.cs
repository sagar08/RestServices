using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTPolicyBasedAuthorization.Data.Contracts;
using JWTPolicyBasedAuthorization.Data.Repositories;
using JWTPolicyBasedAuthorization.Dtos;
using JWTPolicyBasedAuthorization.Infrastructure;
using JWTPolicyBasedAuthorization.Models;
using Newtonsoft.Json;

namespace JWTPolicyBasedAuthorization.Data.SeedData
{
    public class Seed
    {
        public static void SeedRoles(ApplicationDbContext context)
        {
            if (context.Roles.Any()) return;

            var rolesJsonData = System.IO.File.ReadAllText(@"Data\SeedData\RolesMasterSeedData.json");
            var roles = JsonConvert.DeserializeObject<List<Role>>(rolesJsonData);

            // using (IUnitOfWork work = new UnitOfWork(context))
            // {
            //     new RoleRepository(context).CreateRoles(roles.ToArray());
            //     work.SaveChangesAsync();
            // }
            foreach (var role in roles)
            {
                context.Roles.Add(role);
            }
            context.SaveChanges();
        }

        public static void SeedSuperUser(ApplicationDbContext context)
        {
            if (context.Users.Any()) return;

            var user = new RegisterUserDto
            {
                UserName = "SuperUser",
                Password = "P@ssw0rd"
            };

            var newUser = GetUser(user);
            var role = context.Roles.FirstOrDefault(x => x.RoleName == "Super Admin");

            var task = new UserRepository(context).RegisterUserAsync(newUser, role);
            task.Wait();
        }


        private static User GetUser(RegisterUserDto userDto)
        {
            User newUser = new User
            {
                UserName = userDto.UserName
            };

            (byte[] passwordHash, byte[] passwordSalt) = new CommonUtility().CreatePasswordHash(userDto.Password);

            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;

            return newUser;
        }
    }
}