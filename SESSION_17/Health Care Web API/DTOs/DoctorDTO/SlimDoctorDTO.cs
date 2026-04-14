using System.Text.Json.Serialization;


namespace Health_Care_Web_API.DTOs.DoctorDTO
{
    public class SlimDoctorDTO
    {
        [JsonPropertyName("Doctor Id")]
        [JsonPropertyOrder(1)]
        public int Id { get; set; }
        [JsonPropertyName("Doctor Name")]
        [JsonPropertyOrder(2)]
        public string? Name { get; set; } = null;
        [JsonPropertyName("Specialization")]
        [JsonPropertyOrder(3)]
        public string? Specialization { get; set; } = null;
    }
}
