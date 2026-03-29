using System.Text.Json.Serialization;


namespace Health_Care_Web_API.DTOs.PatientDTO
{
    public class CreatePatientRequest
    {

        public string? Name { get; set; } = null;

        public DateOnly? DateOfBirth { get; set; }

    }
}
