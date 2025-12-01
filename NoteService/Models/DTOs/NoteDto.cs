namespace NoteService.Models.DTOs
{
    public class NoteDto
    {
        public required string Id { get; set; }
        public int PatientId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
