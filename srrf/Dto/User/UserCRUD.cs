namespace srrf.Dto.User
{
    public class UserCRUD
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int PhoneNumber { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
