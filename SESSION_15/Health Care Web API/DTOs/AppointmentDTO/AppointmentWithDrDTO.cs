using Health_Care_Web_API.DTOs.DoctorDTO;
using System.Text.Json.Serialization;

namespace Health_Care_Web_API.DTOs.AppointmentDTO
{
    public class AppointmentWithDrDTO
    {
        
        [JsonPropertyName("Appointment Date")]
        [JsonPropertyOrder(1)]
        public DateTime AppointmentDate { get; set; }
        [JsonPropertyName("Doctor Details")]
        [JsonPropertyOrder(2)]
        public SlimDoctorDTO? Doctor { get; set; } = null;
    }
}