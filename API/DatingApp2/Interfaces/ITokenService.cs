using DatingApp2.Entities;

namespace DatingApp2.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
