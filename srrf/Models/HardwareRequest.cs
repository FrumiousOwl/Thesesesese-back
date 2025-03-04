using System.ComponentModel.DataAnnotations;

namespace srrf.Models
{
    public class HardwareRequest
    {
        [Key]
        public int RequestId { get; set; } 
        public DateTime DateNeeded { get; set; } = DateTime.Now;
        public string Name { get; set; } = string.Empty; 
        public string Department { get; set; } = string.Empty; 
        public string Workstation { get; set; } = string.Empty;
        public string Problem { get; set; } = string.Empty;
        public bool IsFulfilled { get; set; }
        public string SerialNo { get; set; } = string.Empty;
        public Hardware Hardware { get; set; } 
        public int HardwareId { get; set; }

    }
}
