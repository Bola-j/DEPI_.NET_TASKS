
using System.Text.Json.Serialization;

namespace Health_Care_Web_API.DTOs.AppointmentDTO
{
    public class SlimAppointmentDTO 
    {

        [JsonPropertyName("Patient Id")]
        [JsonPropertyOrder(1)]
        public int PatientId { get; set; }
        
        [JsonPropertyName("Doctor Id")]
        [JsonPropertyOrder(2)]
        public int DoctorId { get; set; }

        [JsonPropertyName("Appointment Date")]
        [JsonPropertyOrder(3)]
        public DateTime AppointmentDate { get; set; }
    }
}
