using srrf.Models;

namespace srrf.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
