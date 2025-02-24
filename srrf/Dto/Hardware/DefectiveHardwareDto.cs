namespace srrf.Dto.Hardware
{
    public class DefectiveHardwareDto
    {
        public int HardwareId { get; set; }
        public string? Name { get; set; }
        public int? Defective { get; set; }
        public DateTime? DatePurchased { get; set; }
        public string Supplier { get; set; } = string.Empty;
    }
}
