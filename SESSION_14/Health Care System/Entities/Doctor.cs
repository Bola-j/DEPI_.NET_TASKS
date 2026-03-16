using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Health_Care_System.Entities
{
    internal class Doctor
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Specialization { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

        public Doctor()
        {
            Appointments = new List<Appointment>();
        }

        public Doctor(string name, string specialization)
        {
            Name = name;
            Specialization = specialization;
            Appointments = new List<Appointment>();
        }

        public override string ToString()
        {
            return $"ID: {Id}, Doctor: {Name}, Specialization: {Specialization}";
        }
    }
}