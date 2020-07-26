using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTPolicyBasedAuthorization.Data.Contracts;
using JWTPolicyBasedAuthorization.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTPolicyBasedAuthorization.Data.Repositories
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        private ApplicationDbContext _dbContext;

        public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            return await Task.Run(() => FindById(id));
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await FindByCondition(x => x.RoleName == roleName).FirstOrDefaultAsync();

        }

        public async Task<List<Role>> GetUserRolesAsync(int userId)
        {            
            return await FindByCondition(x => x.UserRoles.Any(ur => ur.UserId == userId)).ToListAsync();
        }

        public void CreateRoles(params Role[] roles)
        {
            AddRange(roles);
        }
    }
}