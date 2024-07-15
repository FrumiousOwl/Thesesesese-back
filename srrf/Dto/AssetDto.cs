namespace srrf.Dto
{
    public class AssetDto
    {
        public int AssetId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DatePurchased { get; set; }
        public int? Defective { get; set; }
        public int? Available { get; set; }
        public int? Deployed { get; set; }
    }
}
