using System.ComponentModel.DataAnnotations;

namespace srrf.Dto.Account
{
    public class UpdateUserRoleDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string NewRole { get; set; }
    }

}
