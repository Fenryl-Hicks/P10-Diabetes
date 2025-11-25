using System;

namespace P10.Frontend.Models
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int GenderId { get; set; }
        public string? GenderName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}
