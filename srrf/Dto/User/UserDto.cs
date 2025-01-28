using srrf.Dto.HardwareRequest;
using System.ComponentModel.DataAnnotations;

namespace srrf.Dto.User
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int PhoneNumber { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
