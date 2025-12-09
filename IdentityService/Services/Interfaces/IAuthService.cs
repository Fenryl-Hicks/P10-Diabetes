using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string?> GenerateTokenAsync(string username, string password);
        Task<IdentityResult> RegisterUserAsync(string username, string email, string password);
    }
}
