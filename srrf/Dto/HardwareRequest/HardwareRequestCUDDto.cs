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
        public string SerialNo { get; set; } = string.Empty;
        public int HardwareId { get; set; }

    }
}
