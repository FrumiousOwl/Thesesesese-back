using System.ComponentModel.DataAnnotations;

namespace srrf.Dto.Account
{
    public class UpdateUsersDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

    }
}
