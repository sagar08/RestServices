using System.Collections.Generic;
using System.Threading.Tasks;
using JWTPolicyBasedAuthorization.Dtos;
using JWTPolicyBasedAuthorization.Models;

namespace JWTPolicyBasedAuthorization.Data.Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetUserByIdAsync(int id);

        Task<User> GetUserByNameAsync(string userName);

        Task<List<User>> GetUsersAsync();

        Task<User> LoginUserAsync(string userName, string password);

        Task<User> RegisterUserAsync(User newUser, Role role);

    }
}