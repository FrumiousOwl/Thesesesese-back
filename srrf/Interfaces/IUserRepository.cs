using Microsoft.AspNetCore.Identity;
using srrf.Dto.Account;
using srrf.Models;
using srrf.Queries;

namespace srrf.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UsersDto>> GetAllUsersAsync(QueryUser queryUser);
        Task<UsersDto?> GetUserByIdAsync(string id);
        Task<bool> UpdateUserAsync(string id, UpdateUsersDto updatedUser);
        Task<bool> DeleteUserAsync(string id);
    }
}
