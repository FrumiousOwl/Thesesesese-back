using Microsoft.AspNetCore.Identity;
using BCrypt.Net;

namespace srrf.Service
{

    public class BcryptPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
    {
        private const int WorkFactor = 12;
        private readonly PasswordHasher<TUser> _legacyHasher = new PasswordHasher<TUser>();

        public string HashPassword(TUser user, string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: WorkFactor);
        }

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            if (hashedPassword.StartsWith("$2a$") || hashedPassword.StartsWith("$2b$") || hashedPassword.StartsWith("$2y$"))
            {
                if (BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword))
                {
                    return PasswordVerificationResult.Success;
                }
            }
            else
            {
                var legacyResult = _legacyHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
                if (legacyResult == PasswordVerificationResult.Success)
                {
                    string newHash = HashPassword(user, providedPassword);
                    return PasswordVerificationResult.Success;
                }
            }

            return PasswordVerificationResult.Failed;
        }

    }

}
