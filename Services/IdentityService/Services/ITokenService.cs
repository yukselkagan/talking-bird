using IdentityService.Models;

namespace IdentityService.Services
{
    public interface ITokenService
    {
        public string CreateAccessToken(User user);
    }
}
