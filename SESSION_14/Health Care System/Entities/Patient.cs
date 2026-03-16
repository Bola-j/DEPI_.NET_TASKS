using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Health_Care_System.Entities
{
    internal class Patient
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

        public Patient()
        {
            Appointments = new List<Appointment>();
        }
        public Patient(string name, DateTime dateOfBirth)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            Appointments = new List<Appointment>();
        }

        public override string ToString()
        {
            return $"ID: {Id}, Patient: {Name}, Date of Birth: {DateOfBirth.ToShortDateString()}";
        }
    }
}
