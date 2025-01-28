namespace srrf.Dto.Hardware
{
    public class HardwareCUDDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DatePurchased { get; set; }
        public string Supplier { get; set; } = string.Empty;
        public int? Defective { get; set; }
        public int? Available { get; set; }
        public int? Deployed { get; set; }
    }
}
