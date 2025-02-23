using Microsoft.AspNetCore.Identity;
using srrf.Dto.Account;

namespace srrf.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<List<UserRoleDto>> GetAllUserRoleAsync();
        Task<bool> UpdateUserRoleAsync(string userId, string newRole);
    }
}
