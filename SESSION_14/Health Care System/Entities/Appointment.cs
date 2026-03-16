using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace Health_Care_System.Entities
{
    internal class Appointment
    {

        public int PatientId { get; set; }
        public Patient Patient { get; set; }


        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public DateTime AppointmentDate { get; set; }

        protected Appointment() { }
        public Appointment(Patient patient, Doctor doctor, DateTime appointmentDate)
        {
            Patient = patient;
            PatientId = patient.Id;
            Doctor = doctor;
            DoctorId = doctor.Id;
            AppointmentDate = appointmentDate;
        }

        public override string ToString()
        {
            return $"Appointment: {PatientId} with Dr. {DoctorId} on {AppointmentDate}";
        }
    }
}
