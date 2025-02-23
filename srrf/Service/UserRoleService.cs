using Microsoft.AspNetCore.Identity;
using srrf.Dto.Account;
using srrf.Interfaces;
using srrf.Repository;

namespace srrf.Service
{

    public class UserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<List<UserRoleDto>> GetAllUserRolesAsync()
        {
            return await _userRoleRepository.GetAllUserRoleAsync();
        }

        public async Task<bool> ChangeUserRoleAsync(string userId, string newRole)
        {
            return await _userRoleRepository.UpdateUserRoleAsync(userId, newRole);
        }

    }
}
