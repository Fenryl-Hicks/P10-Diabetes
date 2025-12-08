using AssessmentService.Models.DTOs;

namespace AssessmentService.Services.Interfaces
{
    public interface INoteApiService
    {
        Task<List<NoteDto>> GetNotesByPatientIdAsync(int patientId, string token);
    }
}
