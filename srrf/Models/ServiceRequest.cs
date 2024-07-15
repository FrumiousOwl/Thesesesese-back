namespace srrf.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        public DateTime? DateNeeded { get; set; }
        public string? Name { get; set; }
        public string? Department { get; set; }
        public string? Workstation { get; set; }
        public string? Status { get; set; }

        public int CategoryId { get; set; } // Foreign key property.
        public Category Category { get; set; } // Navigation
                                               // 
        public int AssetId { get; set; } // Foreign key property.
        public Asset Asset { get; set; }
    }
}
