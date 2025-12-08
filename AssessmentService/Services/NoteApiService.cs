using System.Net.Http.Headers;
using System.Text.Json;
using AssessmentService.Models.DTOs;
using AssessmentService.Services.Interfaces;

namespace AssessmentService.Services
{
    public class NoteApiService : INoteApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NoteApiService> _logger;

        public NoteApiService(HttpClient httpClient, ILogger<NoteApiService> logger, IConfiguration config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri(config["NoteApi:BaseUrl"]!);
        }

        public async Task<List<NoteDto>> GetNotesByPatientIdAsync(int patientId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            try
            {
                var response = await _httpClient.GetAsync($"/api/notes/patient/{patientId}");
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Échec GetNotesByPatientIdAsync - Status: {StatusCode}", response.StatusCode);
                    return new List<NoteDto>();
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var notes = await JsonSerializer.DeserializeAsync<List<NoteDto>>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return notes ?? new List<NoteDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception dans GetNotesByPatientIdAsync");
                return new List<NoteDto>();
            }
        }
    }
}
