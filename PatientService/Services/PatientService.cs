using PatientService.DTOs;
using PatientService.Models;
using PatientService.Repositories.Interfaces;
using PatientService.Services.Interfaces;
using PatientService.Utilities;

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
            return patients.Select(Mapping.ToPatientDto).ToList();
        }

        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            return patient is null ? null : Mapping.ToPatientDto(patient);
        }

        public async Task<PatientDto> CreateAsync(CreatePatientDto dto)
        {
            var patient = Mapping.ToPatient(dto);
            await _repository.AddAsync(patient);
            await _repository.SaveChangesAsync();
            return Mapping.ToPatientDto(patient);
        }

        public async Task<PatientDto?> UpdateAsync(int id, UpdatePatientDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null) return null;
            Mapping.UpdatePatientFromDto(existing, dto);
            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();
            return Mapping.ToPatientDto(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null) return false;
            await _repository.DeleteAsync(existing);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
