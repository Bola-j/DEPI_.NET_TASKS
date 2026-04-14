using Health_Care_Web_API.DTOs.AppointmentDTO;
using System.Text.Json.Serialization;
namespace Health_Care_Web_API.DTOs.DoctorDTO
{
    public class DoctorDTO : SlimDoctorDTO
    {
        [JsonPropertyName("appointments")]
        [JsonPropertyOrder(4)]
        public ICollection<AppointmentWithPatientDTO> Appointments { get; set; }
    }
}
