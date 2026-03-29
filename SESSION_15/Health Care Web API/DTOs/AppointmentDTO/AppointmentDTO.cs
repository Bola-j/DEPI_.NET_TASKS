using Health_Care_Web_API.DTOs.DoctorDTO;
using Health_Care_Web_API.DTOs.PatientDTO;
namespace Health_Care_Web_API.DTOs.AppointmentDTO
{
    public class AppointmentDTO : SlimAppointmentDTO
    {
        public SlimDoctorDTO? Doctor { get; set; } = null;
        public SlimPatientDTO? Patient { get; set; } = null;
    }
}
