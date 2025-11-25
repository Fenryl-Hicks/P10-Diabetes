using PatientService.Models;

namespace PatientService.DTOs
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int GenderId { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}
