using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Dto.User;
using srrf.Interfaces;
using srrf.Models;

namespace srrf.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;
        public UserRepository(Context context)
        {
            _context = context;
        }
        public async Task<User> CreateAsync(User userModel)
        {
            await _context.Users.AddAsync(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }

        public async Task<User?> DeleteAsync(int userId)
        {
            var userToDelete = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);

            if (userToDelete == null)
            {
                return null;
            }

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return userToDelete;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<User?> UpdateAsync(int id, UserCRUD userdto)
        {
            var userExisted = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);

            if (userExisted == null)
            {
                return null;
            }

            userExisted.FirstName = userdto.FirstName;
            userExisted.LastName = userdto.LastName;
            userExisted.Email = userdto.Email;
            userExisted.Password = userdto.Password;
            userExisted.PhoneNumber = userdto.PhoneNumber;
            userExisted.Role = userdto.Role;

            await _context.SaveChangesAsync();

            return userExisted;
        }
    }
}
