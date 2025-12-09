using NoteService.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteService.Repositories.Interfaces
{
    public interface INoteRepository
    {
        Task<List<Note>> GetAllAsync();
        Task<Note?> GetByIdAsync(string id);
        Task<List<Note>> GetByPatientIdAsync(int patientId);
        Task AddAsync(Note note);
        Task UpdateAsync(Note note);
        Task DeleteAsync(Note note);
    }
}

