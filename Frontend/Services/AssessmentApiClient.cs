using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;

namespace P10.Frontend.Services
{
    public class AssessmentApiClient : IAssessmentApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AssessmentApiClient> _logger;

        public AssessmentApiClient(HttpClient httpClient, ILogger<AssessmentApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string?> GetPatientRiskAsync(int patientId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            try
            {
                var response = await _httpClient.GetAsync($"/api/assessment/{patientId}");
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Échec GetPatientRiskAsync - Status: {StatusCode}", response.StatusCode);
                    return null;
                }

                // L'API retourne directement le RiskLevel sous forme de texte (ex: "None", "InDanger")
                var riskLevel = await response.Content.ReadAsStringAsync();
                
                // Nettoyer les guillemets si présents
                riskLevel = riskLevel?.Trim('"');
                
                _logger.LogInformation("Risque récupéré pour le patient {PatientId}: {RiskLevel}", patientId, riskLevel);
                
                return riskLevel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception dans GetPatientRiskAsync");
                return null;
            }
        }
    }
}
