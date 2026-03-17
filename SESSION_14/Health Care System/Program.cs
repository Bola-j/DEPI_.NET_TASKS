using Health_Care_System.Data;
using Health_Care_System.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Health_Care_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new HealthCareDBContextFactory().CreateDbContext(args);

            #region Create & Seed Database
            //// Ensure database is created
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //// Add Multiple Doctors
            var doctors = new List<Doctor>
            {
                new Doctor("Dr. Ahmed", "Cardiology"),
                new Doctor("Dr. Sara", "Neurology"),
                new Doctor("Dr. Khalid", "Orthopedics")
            };
            context.Doctors.AddRange(doctors);
            context.SaveChanges();

            //// Add Multiple Patients
            var patients = new List<Patient>
            {
                new Patient("Mohamed Ali", new DateTime(1995, 5, 10)),
                new Patient("Fatima Hassan", new DateTime(2000, 3, 15)),
                new Patient("Omar Youssef", new DateTime(1998, 12, 20))
            };
            context.Patients.AddRange(patients);
            context.SaveChanges();

            //// Add Multiple Appointments
            var appointments = new List<Appointment>
            {
                new Appointment(patients[0], doctors[0], DateTime.Now.AddDays(1)),
                new Appointment(patients[1], doctors[1], DateTime.Now.AddDays(2)),
                new Appointment(patients[2], doctors[2], DateTime.Now.AddDays(3)),
                new Appointment(patients[0], doctors[1], DateTime.Now.AddDays(4)),
                new Appointment(patients[1], doctors[0], DateTime.Now.AddDays(5)),
                new Appointment(patients[2], doctors[1], DateTime.Now.AddDays(6))
            };
            //context.Appointments.AddRange(appointments);
            //context.SaveChanges();
            #endregion

            #region Update
            //// Update all doctors in Cardiology to Internal Medicine
            //var cardioDoctors = context.Doctors
            //    .Where(d => d.Specialization == "Cardiology")
            //    .ToList();

            //cardioDoctors.ForEach(d => d.Specialization = "Internal Medicine");

            //context.Doctors.UpdateRange(cardioDoctors);
            //context.SaveChanges();
            //Console.WriteLine("Updated Cardiology doctors to Internal Medicine ");

            //// Update patient names older than 25 to append "(Updated)"
            //var oldPatients = context.Patients
            //    .Where(p => (DateTime.Now.Year - p.DateOfBirth.Year) > 25)
            //    .ToList();

            //oldPatients.ForEach(p => { if (p.Name.Count(c => c == '(') < 1) p.Name += " (Updated)"; });


            //context.Patients.UpdateRange(oldPatients);
            //context.SaveChanges();
            //Console.WriteLine("Updated patient names older than 25 ");

            // //Reschedule appointments by 7 days as dr of Id 2 is on leave
            //var appointmentsToReschedule = context.Appointments
            //    .Where(a => a.DoctorId == 2)
            //    .ToList();

            //appointmentsToReschedule.ForEach(a => a.AppointmentDate = a.AppointmentDate.AddDays(7));

            //context.Appointments.UpdateRange(appointmentsToReschedule);
            //context.SaveChanges();
            //Console.WriteLine("Rescheduled appointments for Dr. with Id 2 ");
            #endregion

            #region Read & Query
            ////// Read all doctors and their appointments
            //var doctorDetails = context.Doctors.Include(d => d.Appointments).ThenInclude(a => a.Patient).ToList();
            //Console.WriteLine("Doctors and their Appointments:");
            //foreach (var d in doctorDetails)
            //{
            //    Console.WriteLine($"{d.Name} ({d.Specialization})");
            //    foreach (var a in d.Appointments)
            //    {
            //        Console.WriteLine($"  Appointment with {a.Patient.Name} on {a.AppointmentDate}");
            //    }
            //}

            ////// Query: Patients who have appointments with Dr. Sara
            //var patientsWithSara = context.Appointments
            //    .Include(a => a.Patient)
            //    .Include(a => a.Doctor)
            //    .Where(a => a.Doctor.Name == "Dr. Sara")
            //    .Select(a => a.Patient.Name)
            //    .Distinct()
            //    .ToList();

            //Console.WriteLine("\nPatients with appointments with Dr. Sara:");
            //patientsWithSara.ForEach(Console.WriteLine);
            #endregion

            #region Delete
            //// Delete appointments older than today (example cleanup)
            var oldAppointments = context.Appointments.Where(a => a.AppointmentDate < DateTime.Now).ToList();
            context.Appointments.RemoveRange(oldAppointments);
            context.SaveChanges();

            //// Delete a doctor and cascade delete their appointments
            var doctorToDelete = context.Doctors.FirstOrDefault(d => d.Name == "Dr. Khalid");
            if (doctorToDelete != null)
            {
                context.Doctors.Remove(doctorToDelete);
                context.SaveChanges();
            }

            //// Show remaining appointments and their doctors/patients
            var remainingAppointments = context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToList();

            Console.WriteLine("\nRemaining Appointments after deletion:");
            foreach (var a in remainingAppointments)
            {
                Console.WriteLine($"{a.Patient.Name} with {a.Doctor.Name} on {a.AppointmentDate}");
            }
            #endregion
        }
    }
}