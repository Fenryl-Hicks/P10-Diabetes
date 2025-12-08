using System.Net.Http.Headers;
using System.Text.Json;
using AssessmentService.Models.DTOs;
using AssessmentService.Services.Interfaces;

namespace AssessmentService.Services
{
    public class PatientApiService : IPatientApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PatientApiService> _logger;

        public PatientApiService(HttpClient httpClient, ILogger<PatientApiService> logger, IConfiguration config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri(config["PatientApi:BaseUrl"]!);
        }

        public async Task<PatientDto?> GetPatientByIdAsync(int patientId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            try
            {
                var response = await _httpClient.GetAsync($"/api/patients/{patientId}");
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Échec GetPatientByIdAsync - Status: {StatusCode}", response.StatusCode);
                    return null;
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var patient = await JsonSerializer.DeserializeAsync<PatientDto>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return patient;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception dans GetPatientByIdAsync");
                return null;
            }
        }
    }
}
