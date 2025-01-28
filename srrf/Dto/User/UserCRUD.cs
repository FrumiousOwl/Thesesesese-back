using System.ComponentModel.DataAnnotations;

namespace srrf.Dto.User
{
    public class UserCRUD
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public int PhoneNumber { get; set; }
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
