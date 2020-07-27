using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTPolicyBasedAuthorization.Data.Contracts;
using JWTPolicyBasedAuthorization.Data.Repositories;
using JWTPolicyBasedAuthorization.Dtos;
using JWTPolicyBasedAuthorization.Infrastructure;
using JWTPolicyBasedAuthorization.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JWTPolicyBasedAuthorization.Data.SeedData
{
    public class Seed
    {        
        #region Public Methods
        public static void SeedRoles(ApplicationDbContext context)
        {
            if (context.Roles.Any()) return;

            var rolesJsonData = System.IO.File.ReadAllText(@"Data\SeedData\RolesMasterSeedData.json");
            var roles = JsonConvert.DeserializeObject<List<Role>>(rolesJsonData);

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

        public static void SeedConfigKeyValues(ApplicationDbContext context)
        {
            if (context.ConfigKeys.Any()) return;

            var configJsonData = System.IO.File.ReadAllText(@"Data\SeedData\ConfigKeyValuesMasterSeedData.json");
            var configKeys = JsonConvert.DeserializeObject<List<ConfigKey>>(configJsonData);

            foreach (var key in configKeys)
            {
                context.ConfigKeys.Add(key);
                // foreach (var value in key.ConfigValues)
                //     key.ConfigValues.Add(value);                
            }
            context.SaveChanges();
        }

        public static void SeedProducts(ApplicationDbContext context)
        {
            if (context.Products.Any()) return;

            var jsonData = System.IO.File.ReadAllText(@"Data\SeedData\ProductSeedData.json");
            var products = JsonConvert.DeserializeObject<Product[]>(jsonData);

            context.Products.AddRange(products);
            context.SaveChanges();            
        }

        #endregion

        #region Private Methods
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
        #endregion
    }
}