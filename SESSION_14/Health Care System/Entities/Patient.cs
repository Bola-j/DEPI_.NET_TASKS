using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Health_Care_System.Entities
{
    internal class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

        public Patient()
        {
            Appointments = new List<Appointment>();
        }

        public Patient(string name, DateTime dateOfBirth)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DateOfBirth = dateOfBirth;
            Appointments = new List<Appointment>();
        }

        public override string ToString()
        {
            return $"ID: {Id}, Patient: {Name}, Date of Birth: {DateOfBirth.ToShortDateString()}";
        }
    }
}