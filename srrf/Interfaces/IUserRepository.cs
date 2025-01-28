using srrf.Dto.User;
using srrf.Models;

namespace srrf.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int  userId);
        Task<User> CreateAsync(User userModel);
        Task<User?> DeleteAsync(int userId);
        Task<User?> UpdateAsync(int userId, UserCRUD updateDto);
    }
}
