using PatientService.Models;

namespace PatientService.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetAllAsync();
        Task<Patient?> GetByIdAsync(int id);
        Task AddAsync(Patient patient);
        Task SaveChangesAsync();
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(Patient patient);
    }
}
