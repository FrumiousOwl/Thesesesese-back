using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Dto.Account;
using srrf.Interfaces;
using srrf.Models;

namespace srrf.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Context _context;
        public UserRoleRepository(Context context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<UserRoleDto>> GetAllUserRoleAsync()
        {
            var userRoles = await(from ur in _context.UserRoles
                                  join u in _context.Users on ur.UserId equals u.Id
                                  join r in _context.Roles on ur.RoleId equals r.Id
                                  select new UserRoleDto
                                  {
                                      UserId = ur.UserId,
                                      UserName = u.UserName,  
                                      RoleId = ur.RoleId,
                                      RoleName = r.Name
                                  }).ToListAsync();

            return userRoles;
        }

        public async Task<bool> UpdateUserRoleAsync(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!await _roleManager.RoleExistsAsync(newRole))
                return false;

            var result = await _userManager.AddToRoleAsync(user, newRole);
            return result.Succeeded;
        }
    }
}
