using srrf.Dto.Hardware;
using srrf.Dto.User;
using System.ComponentModel.DataAnnotations;

namespace srrf.Dto.HardwareRequest
{
    public class HardwareRequestCUDDto
    {
        [Required]
        public DateTime DateNeeded { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Department { get; set; } = string.Empty;
        [Required]
        public string Workstation { get; set; } = string.Empty;
        [Required]
        public string Problem { get; set; } = string.Empty;
        public bool IsFulfilled { get; set; }
        public int UserId { get; set; }
        public int HardwareId { get; set; }
    }
}
