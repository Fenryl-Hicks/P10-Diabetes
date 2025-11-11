using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IdentityService.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityUser?> FindByUsernameAsync(string username);
        Task<bool> CheckPasswordAsync(IdentityUser user, string password);
        Task<IdentityResult> CreateAsync(IdentityUser user, string password);

    }
}
