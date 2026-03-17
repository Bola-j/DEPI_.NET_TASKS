using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Care_System.Entities
{
    internal class Doctor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Specialization { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

        public Doctor()
        {
            Appointments = new List<Appointment>();
        }

        public Doctor(string name, string specialization)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Specialization = specialization ?? throw new ArgumentNullException(nameof(specialization));
            Appointments = new List<Appointment>();
        }

        public override string ToString()
        {
            return $"ID: {Id}, Doctor: {Name}, Specialization: {Specialization}";
        }
    }
}