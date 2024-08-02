namespace srrf.Dto
{
    public class AvailableAssetDto
    {
        public int AssetId { get; set; }
        public string? Name { get; set; }
        public int? Available { get; set; }
        public int? Deployed { get; set; }
        public DateTime? DatePurchased { get; set; }
    }

}