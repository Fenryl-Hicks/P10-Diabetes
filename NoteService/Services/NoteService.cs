using NoteService.Models.DTOs;
using NoteService.Repositories.Interfaces;
using NoteService.Services.Interfaces;
using NoteService.Utilities;

namespace NoteService.Services
{
    public class NoteServices : INoteServices
    {
        private readonly INoteRepository _repository;

        public NoteServices(INoteRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<NoteDto>> GetAllAsync()
        {
            var notes = await _repository.GetAllAsync();
            return notes.Select(Mapping.ToNoteDto).ToList();
        }

        public async Task<NoteDto?> GetByIdAsync(string id)
        {
            var note = await _repository.GetByIdAsync(id);
            return note is null ? null : Mapping.ToNoteDto(note);
        }

        public async Task<List<NoteDto>> GetByPatientIdAsync(int patientId)
        {
            var notes = await _repository.GetByPatientIdAsync(patientId);
            return notes.Select(Mapping.ToNoteDto).ToList();
        }

        public async Task<NoteDto> CreateAsync(CreateNoteDto dto)
        {
            var note = Mapping.ToNote(dto);
            await _repository.AddAsync(note);
            return Mapping.ToNoteDto(note);
        }

        public async Task<NoteDto?> UpdateAsync(string id, UpdateNoteDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null) return null;

            Mapping.UpdateNoteFromDto(existing, dto);
            await _repository.UpdateAsync(existing);
            return Mapping.ToNoteDto(existing);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null) return false;

            await _repository.DeleteAsync(existing);
            return true;
        }
    }
}

