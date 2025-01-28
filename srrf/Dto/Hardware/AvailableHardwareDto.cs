namespace srrf.Dto.Hardware
{
    public class AvailableHardwareDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Available { get; set; }
        public int? Deployed { get; set; }
        public DateTime? DatePurchased { get; set; }
        public string Supplier { get; set; } = string.Empty;
    }


}