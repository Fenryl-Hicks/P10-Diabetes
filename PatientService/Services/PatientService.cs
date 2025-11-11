using PatientService.DTOs;
using PatientService.Models;
using PatientService.Repositories.Interfaces;
using PatientService.Services.Interfaces;

namespace PatientService.Services
{
    public class PatientManager : IPatientService
    {
        private readonly IPatientRepository _repository;

        public PatientManager(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PatientDto>> GetAllAsync()
        {
            var patients = await _repository.GetAllAsync();
            return patients.Select(p => new PatientDto
            {
                Id = p.Id,
                FullName = $"{p.FirstName} {p.LastName}",
                BirthDate = p.BirthDate
            }).ToList();
        }

        public async Task<PatientDto?> GetByIdAsync(Guid id)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient is null) return null;

            return new PatientDto
            {
                Id = patient.Id,
                FullName = $"{patient.FirstName} {patient.LastName}",
                BirthDate = patient.BirthDate
            };
        }

        public async Task<PatientDto> CreateAsync(PatientDto dto)
        {
            var names = dto.FullName.Split(' ', 2); // Séparer prénom/nom
            var patient = new Patient
            {
                FirstName = names.ElementAtOrDefault(0) ?? "",
                LastName = names.ElementAtOrDefault(1) ?? "",
                BirthDate = dto.BirthDate
            };

            await _repository.AddAsync(patient);
            await _repository.SaveChangesAsync();

            return new PatientDto
            {
                Id = patient.Id,
                FullName = $"{patient.FirstName} {patient.LastName}",
                BirthDate = patient.BirthDate
            };
        }
        public async Task<PatientDto?> UpdateAsync(Guid id, PatientDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null) return null;

            var names = dto.FullName.Split(' ', 2);
            existing.FirstName = names.ElementAtOrDefault(0) ?? "";
            existing.LastName = names.ElementAtOrDefault(1) ?? "";
            existing.BirthDate = dto.BirthDate;

            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();

            return new PatientDto
            {
                Id = existing.Id,
                FullName = $"{existing.FirstName} {existing.LastName}",
                BirthDate = existing.BirthDate
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null) return false;

            await _repository.DeleteAsync(existing);
            await _repository.SaveChangesAsync();
            return true;
        }

    }
}
