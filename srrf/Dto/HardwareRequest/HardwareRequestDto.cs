
using srrf.Models;

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
        public string SerialNo { get; set; } = string.Empty;
        public int HardwareId { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
    }
}
