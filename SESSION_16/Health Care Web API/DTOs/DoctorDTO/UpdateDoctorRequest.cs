namespace Health_Care_Web_API.DTOs.DoctorDTO
{
    public class UpdateDoctorRequest
    {   
        public string Id { get; set; }
        public string? Name { get; set; } = null;
        public string? Specialization { get; set; } = null;
    }
}
