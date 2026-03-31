using System.Text.Json.Serialization;

namespace Health_Care_Web_API.DTOs.PatientDTO
{
    public class SlimPatientDTO
    {
        [JsonPropertyName("Patient Id")]
        [JsonPropertyOrder(1)]
        public int Id { get; set; }
        [JsonPropertyName("Patient Name")]
        [JsonPropertyOrder(2)]
        public string Name { get; set; }
        [JsonPropertyName("Date Of Birth")]
        [JsonPropertyOrder(3)]
        
        public DateOnly DateOfBirth { get; set; }
    }
}
