using System.ComponentModel.DataAnnotations;

namespace NoteService.Models.DTOs
{
    public class CreateNoteDto
    {
        public required int PatientId { get; set; }

        [Required(ErrorMessage = "Le contenu de la note est obligatoire")]
        public required string Content { get; set; }
    }
}
