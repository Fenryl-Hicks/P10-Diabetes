using IdentityService.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IdentityService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<IdentityUser?> FindByUsernameAsync(string username)
        {
            return _userManager.FindByNameAsync(username);
        }

        public Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password);
        }
        public Task<IdentityResult> CreateAsync(IdentityUser user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

    }
}
