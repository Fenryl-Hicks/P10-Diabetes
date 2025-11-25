using System.ComponentModel.DataAnnotations;

namespace PatientService.DTOs
{
    public class CreatePatientDto
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid gender")]
        public int GenderId { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}
