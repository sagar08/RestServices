using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTPolicyBasedAuthorization.Data.Contracts;
using JWTPolicyBasedAuthorization.Data;
using JWTPolicyBasedAuthorization.Infrastructure;
using JWTPolicyBasedAuthorization.Models;
using JWTPolicyBasedAuthorization.Dtos;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace JWTPolicyBasedAuthorization.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await Task.Run(()=>FindById(id));
        }

        public async Task<User> GetUserByNameAsync(string userName)
        {
            return await FindByCondition(x => x.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<User> LoginUserAsync(string userName, string password)
        {
            var user = await FindByCondition(x => x.UserName == userName).FirstOrDefaultAsync();
            if (user == null) return null;

            var validPassword = new CommonUtility().ValidatePassword(password, user.PasswordHash, user.PasswordSalt);

            return user;
        }

        public async Task<User> RegisterUserAsync(User newUser, Role role)
        {            
            using (IUnitOfWork work = new UnitOfWork(_dbContext))
            {
                Add(newUser);

                newUser.UserRoles.Add(new UserRoles
                {
                    UserId = newUser.Id,
                    RoleId = role.Id
                });

               await work.SaveChangesAsync();
            }
            return newUser;
        }
    }
}