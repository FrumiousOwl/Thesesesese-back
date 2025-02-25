using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using srrf.Dto.Account;
using srrf.Interfaces;
using srrf.Models;
using srrf.Queries;

namespace srrf.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager) 
        {
            _userManager = userManager; 
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<IEnumerable<UsersDto>> GetAllUsersAsync(QueryUser queryUser)
        {
            var user = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryUser.UserName))
            {
                user = user.Where(u => u.UserName.Contains(queryUser.UserName));
            }

            if (!string.IsNullOrWhiteSpace(queryUser.Email))
            {
                user = user.Where(u => u.Email.Contains(queryUser.Email));
            }

            var users = await user.Select(u => new UsersDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber
            }).ToListAsync();

            return users;
        }

        public async Task<UsersDto?> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user == null ? null : new UsersDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<bool> UpdateUserAsync(string id, UpdateUsersDto updatedUser)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            user.Email = updatedUser.Email;
            user.UserName = updatedUser.UserName;
            user.PhoneNumber = updatedUser.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
