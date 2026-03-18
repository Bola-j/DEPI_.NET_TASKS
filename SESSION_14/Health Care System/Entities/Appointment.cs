using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Care_System.Entities
{
    internal class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }

        // Navigation made nullable to allow EF to materialize without eager loading
        public Patient? Patient { get; set; }

        [Required]
        [ForeignKey(nameof(Doctor))]
        public int DoctorId { get; set; }

        // Navigation made nullable to allow EF to materialize without eager loading
        public Doctor? Doctor { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDate { get; set; }

        protected Appointment() { }

        public Appointment(Patient patient, Doctor doctor, DateTime appointmentDate)
        {
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            PatientId = patient.Id;
            Doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
            DoctorId = doctor.Id;
            AppointmentDate = appointmentDate;
        }

        public override string ToString()
        {
            var patientStr = Patient?.ToString() ?? $"PatientId {PatientId}";
            var doctorStr = Doctor?.ToString() ?? $"DoctorId {DoctorId}";
            return $"Appointment: {patientStr} with Dr. {doctorStr} on {AppointmentDate}";
        }
    }
}