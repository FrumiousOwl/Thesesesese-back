namespace srrf.Dto.Account
{
    public class ResetPasswordDto
    {
        public string UserEmail { get; set; } 
        public string TemporaryPassword { get; set; }
    }
}
