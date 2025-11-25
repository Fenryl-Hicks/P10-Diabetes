namespace PatientService.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int GenderId { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Gender Gender { get; set; } = null!;
    }
}
