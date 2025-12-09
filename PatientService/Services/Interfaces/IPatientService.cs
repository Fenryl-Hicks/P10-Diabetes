using PatientService.DTOs;

namespace PatientService.Services.Interfaces
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();
        Task<PatientDto?> GetByIdAsync(int id);
        Task<PatientDto> CreateAsync(CreatePatientDto dto);
        Task<PatientDto?> UpdateAsync(int id, UpdatePatientDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
