using P10.Frontend.Models;

namespace P10.Frontend.Services
{
    public interface INoteApiClient
    {
        Task<List<NoteViewModel>> GetNotesByPatientAsync(int patientId, string token);
        Task<NoteViewModel?> GetNoteByIdAsync(string id, string token);
        Task<bool> CreateNoteAsync(NoteViewModel model, string token);
        Task<bool> UpdateNoteAsync(NoteViewModel model, string token);
        Task<bool> DeleteNoteAsync(string id, string token);
    }
}
