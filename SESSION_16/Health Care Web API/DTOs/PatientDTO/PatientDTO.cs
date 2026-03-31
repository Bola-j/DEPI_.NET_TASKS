using Health_Care_Web_API.DTOs.AppointmentDTO;
using System.Text.Json.Serialization;

namespace Health_Care_Web_API.DTOs.PatientDTO
{
    public class PatientDTO : SlimPatientDTO
    {
        [JsonPropertyName("appointments")]
        [JsonPropertyOrder(4)]
        public ICollection<AppointmentWithDrDTO> Appointments { get; set; }
    }
}
