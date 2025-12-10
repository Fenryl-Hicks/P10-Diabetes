using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using P10.Frontend.Models;

namespace P10.Frontend.Services
{
    public class NoteApiClient : INoteApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NoteApiClient> _logger;

        public NoteApiClient(HttpClient httpClient, ILogger<NoteApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<NoteViewModel>> GetNotesByPatientAsync(int patientId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var response = await _httpClient.GetAsync($"/api/notes/patient/{patientId}");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Échec GetNotesByPatientAsync - Status: {StatusCode}", response.StatusCode);
                    return new List<NoteViewModel>();
                }
                var stream = await response.Content.ReadAsStreamAsync();
                var notes = await JsonSerializer.DeserializeAsync<List<NoteViewModel>>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return notes ?? new List<NoteViewModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception dans GetNotesByPatientAsync");
                return new List<NoteViewModel>();
            }
        }

        public async Task<NoteViewModel?> GetNoteByIdAsync(string id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var response = await _httpClient.GetAsync($"/api/notes/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Échec GetNoteByIdAsync - Status: {StatusCode}", response.StatusCode);
                    return null;
                }
                var stream = await response.Content.ReadAsStreamAsync();
                var note = await JsonSerializer.DeserializeAsync<NoteViewModel>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return note;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception dans GetNoteByIdAsync");
                return null;
            }
        }

        public async Task<bool> CreateNoteAsync(NoteViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/notes", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception dans CreateNoteAsync");
                return false;
            }
        }

        public async Task<bool> UpdateNoteAsync(NoteViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/notes/{model.Id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception dans UpdateNoteAsync");
                return false;
            }
        }

        public async Task<bool> DeleteNoteAsync(string id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/notes/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception dans DeleteNoteAsync");
                return false;
            }
        }
    }
}
