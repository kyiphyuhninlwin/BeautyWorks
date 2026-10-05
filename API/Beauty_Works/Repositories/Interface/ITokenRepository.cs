using Microsoft.AspNetCore.Identity;

namespace Beauty_Works.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
