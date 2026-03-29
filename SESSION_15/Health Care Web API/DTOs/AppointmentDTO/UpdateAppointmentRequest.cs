namespace Health_Care_Web_API.DTOs.AppointmentDTO
{
    public class UpdateAppointmentRequest
    {
        public int PatientId { get; set; }   
        public int DoctorId { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }
}
