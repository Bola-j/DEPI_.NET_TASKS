using Health_Care_Web_API.DTOs.PatientDTO;
using System.Text.Json.Serialization;

namespace Health_Care_Web_API.DTOs.AppointmentDTO
{
    public class AppointmentWithPatientDTO
    {
        

        [JsonPropertyName("Appointment Date")]
        [JsonPropertyOrder(1)]
        public DateTime AppointmentDate { get; set; }
        [JsonPropertyName("Patient Details")]
        [JsonPropertyOrder(2)]
        public SlimPatientDTO? Patient { get; set; } = null;

    }
}
