using System.ComponentModel.DataAnnotations;

namespace srrf.Queries
{
    public class QueryUser
    {
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? Email { get; set; } = null;
    }
}
