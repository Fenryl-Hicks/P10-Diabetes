using Microsoft.EntityFrameworkCore;
using NoteService.Data;
using NoteService.Models.Entities;
using NoteService.Repositories.Interfaces;

namespace NoteService.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly NoteServiceDbContext _database;

        public NoteRepository(NoteServiceDbContext database)
        {
            _database = database;
        }

        public async Task<List<Note>> GetAllAsync()
        {
            return await _database.Notes.Where(n => !n.IsDeleted).ToListAsync();
        }

        public async Task<Note?> GetByIdAsync(string id)
        {
            return await _database.Notes.FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted);
        }

        public async Task<List<Note>> GetByPatientIdAsync(int patientId)
        {
            return await _database.Notes
                .Where(n => n.PatientId == patientId && !n.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(Note note)
        {
            await _database.Notes.AddAsync(note);
            await _database.SaveChangesAsync();
        }

        public async Task UpdateAsync(Note note)
        {
            _database.Notes.Update(note);
            await _database.SaveChangesAsync();
        }

        public async Task DeleteAsync(Note note)
        {
            note.IsDeleted = true;
            _database.Notes.Update(note);
            await _database.SaveChangesAsync();
        }
    }
}

