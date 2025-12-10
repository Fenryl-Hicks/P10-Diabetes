using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace P10.Frontend.Services
{
    public interface IAuthApiClient
    {
        Task<string?> LoginAsync(string username, string password);
    }

    public class AuthApiClient : IAuthApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthApiClient> _logger;

        public AuthApiClient(HttpClient httpClient, ILogger<AuthApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var payload = new
            {
                Username = username,
                Password = password
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/auth/login", payload);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("[LOGIN FAIL] Status: {StatusCode}, Body: {Body}", response.StatusCode, error);
                    return null;
                }

                using var stream = await response.Content.ReadAsStreamAsync();
                using var doc = await JsonDocument.ParseAsync(stream);

                if (doc.RootElement.TryGetProperty("token", out var tokenProperty))
                    return tokenProperty.GetString();

                _logger.LogWarning("[LOGIN FAIL] Réponse JSON ne contient pas de champ 'token'");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EXCEPTION] Une erreur s’est produite lors de l’appel à IdentityService");
                return null;
            }
        }
    }
}


