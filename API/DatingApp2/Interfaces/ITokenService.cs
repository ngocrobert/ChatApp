using DatingApp2.Entities;

namespace DatingApp2.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
