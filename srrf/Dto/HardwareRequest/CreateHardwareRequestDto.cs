namespace srrf.Dto.HardwareRequest
{
    public class CreateHardwareRequestDto
    {

        public string Department { get; set; } = string.Empty;
        public string Workstation { get; set; } = string.Empty;
        public string Problem { get; set; } = string.Empty;
        public DateTime DateNeeded { get; set; }
        public bool IsFulfilled { get; set; }
        public string SerialNo { get; set; } = string.Empty;
        public int HardwareId { get; set; }
    }
}
