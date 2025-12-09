namespace PatientService.Models
{
    public class Gender
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    }
}
