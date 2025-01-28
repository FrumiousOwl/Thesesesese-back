using srrf.Dto.Hardware;
using srrf.Dto.User;

namespace srrf.Dto.HardwareRequest
{
    public class HardwareRequestDto
    {
        public int RequestId { get; set; }
        public DateTime DateNeeded { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Workstation { get; set; } = string.Empty;
        public string Problem { get; set; } = string.Empty;
        public bool IsFulfilled { get; set; }
        public int UserId { get; set; }
        public int HardwareId { get; set; }
    }
}
