using Microsoft.EntityFrameworkCore;
using PatientService.Data;
using PatientService.Models;
using PatientService.Repositories.Interfaces;

namespace PatientService.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientDbContext _context;

        public PatientRepository(PatientDbContext context)
        {
            _context = context;
        }

        public async Task<List<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .Include(p => p.Gender)
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Gender)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            return patient;
        }

        public async Task AddAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Patient patient)
        {
            patient.IsDeleted = true;
            _context.Patients.Update(patient);
            return Task.CompletedTask;
        }
    }
}
