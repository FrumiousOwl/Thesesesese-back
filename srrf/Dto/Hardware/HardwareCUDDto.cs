using System.ComponentModel.DataAnnotations;

namespace srrf.Dto.Hardware
{
    public class HardwareCUDDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Hardware name must be atleast 3 characters")]
        [MaxLength(50, ErrorMessage = "Hardware name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "Description must be atleast 5 characters")]
        [MaxLength(250, ErrorMessage = "Hardware name cannot exceed 250 characters")]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime DatePurchased { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "Supplier must be atleast 1 characters")]
        [MaxLength(250, ErrorMessage = "Supplier name cannot exceed 250 characters")]
        public string Supplier { get; set; } = string.Empty;
        public int? Defective { get; set; }
        public int? Available { get; set; }
        public int? Deployed { get; set; }
        [Required]
        public string TotalPrice { get; set; } = string.Empty;
    }
}
