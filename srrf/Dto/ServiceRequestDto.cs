namespace srrf.Dto
{
    public class ServiceRequestDto
    {
        public int Id { get; set; }
        public DateTime? DateNeeded { get; set; }
        public string? Name { get; set; }
        public string? Department { get; set; }
        public string? Workstation { get; set; }
        public string? Status { get; set; }
        public int CategoryId { get; set; }
    }
}
