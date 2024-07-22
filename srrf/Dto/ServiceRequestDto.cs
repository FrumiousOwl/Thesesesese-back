namespace srrf.Dto
{
    public class ServiceRequestDto
    {
        public int Id { get; set; } //srrfno??
        public DateTime? DateNeeded { get; set; }
        public string? Name { get; set; }
        public string? Department { get; set; }
        public string? Workstation { get; set; }
        public string? Problem { get; set; }
        public string? MaterialNeeded { get; set; }
        public int Rid { get; set; }
        public int CategoryId { get; set; }

    }
}
