using System.Collections.Generic;
using System.Threading.Tasks;
using JWTPolicyBasedAuthorization.Models;

namespace JWTPolicyBasedAuthorization.Data.Contracts
{
    public interface IRoleRepository : IRepositoryBase<Role>
    {
        Task<Role> GetRoleByIdAsync(int id);
        Task<Role> GetRoleByNameAsync(string roleName);
        Task<List<Role>> GetUserRolesAsync(int userId);

        void CreateRoles(params Role[] roles);
    }
}