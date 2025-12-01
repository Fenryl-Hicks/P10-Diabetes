using NoteService.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteService.Services.Interfaces
{
    public interface INoteServices
    {
        Task<List<NoteDto>> GetAllAsync();
        Task<NoteDto?> GetByIdAsync(string id);
        Task<List<NoteDto>> GetByPatientIdAsync(int patientId);
        Task<NoteDto> CreateAsync(CreateNoteDto dto);
        Task<NoteDto?> UpdateAsync(string id, UpdateNoteDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
