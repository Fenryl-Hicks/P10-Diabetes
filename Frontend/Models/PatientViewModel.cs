using System;
using System.ComponentModel.DataAnnotations;

namespace P10.Frontend.Models
{
    public class PatientViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "La date de naissance est obligatoire")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Le genre est obligatoire")]
        public int GenderId { get; set; }

        public string? GenderName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        
        public string? DiabetesRisk { get; set; }
    }
}
