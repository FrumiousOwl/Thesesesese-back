namespace srrf.Models
{
    public class AnomalyLog
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Entity { get; set; }
        public string Action { get; set; }
        public bool IsAnomaly { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

}
