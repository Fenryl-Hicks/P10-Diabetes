using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using P10.Frontend.Models;

namespace P10.Frontend.Services
{
    public class PatientApiClient : IPatientApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PatientApiClient> _logger;

        public PatientApiClient(HttpClient httpClient, ILogger<PatientApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri("https://localhost:7050"); // Gateway
        }

        public async Task<List<PatientViewModel>> GetPatientsAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.GetAsync("/api/patients");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Échec GetPatientsAsync - Status: {StatusCode}", response.StatusCode);
                    return new List<PatientViewModel>();
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var patients = await JsonSerializer.DeserializeAsync<List<PatientViewModel>>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return patients ?? new List<PatientViewModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception dans GetPatientsAsync");
                return new List<PatientViewModel>();
            }
        }

        public async Task<PatientViewModel?> GetPatientByIdAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.GetAsync($"/api/patients/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Échec GetPatientByIdAsync - Status: {StatusCode}", response.StatusCode);
                    return null;
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var patient = await JsonSerializer.DeserializeAsync<PatientViewModel>(stream, new JsonSerializerOptions
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

        public async Task<bool> CreatePatientAsync(PatientViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/patients", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception dans CreatePatientAsync");
                return false;
            }
        }

        public async Task<bool> UpdatePatientAsync(PatientViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/patients/{model.Id}", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception dans UpdatePatientAsync");
                return false;
            }
        }

        public async Task<bool> DeletePatientAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.DeleteAsync($"/api/patients/{id}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception dans DeletePatientAsync");
                return false;
            }
        }
    }
}
