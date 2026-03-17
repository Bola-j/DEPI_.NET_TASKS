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

        [Required]
        public Patient Patient { get; set; }

        [Required]
        [ForeignKey(nameof(Doctor))]
        public int DoctorId { get; set; }

        [Required]
        public Doctor Doctor { get; set; }

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
            return $"Appointment: {PatientId} with Dr. {DoctorId} on {AppointmentDate}";
        }
    }
}