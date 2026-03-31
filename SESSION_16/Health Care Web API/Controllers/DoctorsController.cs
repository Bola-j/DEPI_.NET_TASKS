using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Health_Care_Web_API.Models;
using Microsoft.EntityFrameworkCore;
using Health_Care_Web_API.DTOs.DoctorDTO;
using Health_Care_Web_API.DTOs.AppointmentDTO;
using Health_Care_Web_API.DTOs.PatientDTO;


namespace Health_Care_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly HEALTH_CARE_SYSTEM_DBContext _context;

        public DoctorsController(HEALTH_CARE_SYSTEM_DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<DoctorDTO>> GetDoctors()
        {
            var doctorResponses = await _context.Doctors
                .AsNoTracking()
                .Select(d => new DoctorDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Specialization = d.Specialization,
                    Appointments = d.Appointments.Where(a => a.DoctorId == d.Id)
                        .Select(a => new AppointmentWithPatientDTO
                        {
                            AppointmentDate = a.AppointmentDate,
                            Patient = new SlimPatientDTO
                            {
                                Id = a.Patient.Id,
                                Name = a.Patient.Name,
                                DateOfBirth = DateOnly.FromDateTime(a.Patient.DateOfBirth)
                            }
                        }).ToList()
                }).ToListAsync();
            return doctorResponses;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDTO>> GetDoctor(int id)
        {
            var doctor = await _context.Doctors
                .AsNoTracking()
                .Where(d => d.Id == id)
                .Select(d => new DoctorDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Specialization = d.Specialization,
                    Appointments = d.Appointments.Where(a => a.DoctorId == d.Id)
                        .Select(a => new AppointmentWithPatientDTO
                        {
                            AppointmentDate = a.AppointmentDate,
                            Patient = new SlimPatientDTO
                            {
                                Id = a.Patient.Id,
                                Name = a.Patient.Name,
                                DateOfBirth = DateOnly.FromDateTime(a.Patient.DateOfBirth)
                            }
                        }).ToList()
                }).FirstOrDefaultAsync();
            if (doctor == null)
            {
                return NotFound();
            }
            return Ok(doctor);
        }

        [HttpPost]
        public async Task<ActionResult<SlimDoctorDTO>> CreateDoctor(CreateDoctorRequest request)
        {
            var doctor = new Doctor();

            if (!string.IsNullOrWhiteSpace(request.Name) || request.Name != "")
                doctor.Name = request.Name;
            if (!string.IsNullOrWhiteSpace(request.Specialization) || request.Specialization != "")
                doctor.Specialization = request.Specialization;

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            var doctorResponse = new SlimDoctorDTO
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Specialization = doctor.Specialization
            };

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, doctorResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, UpdateDoctorRequest request)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(request.Name) || request.Name != "")
                doctor.Name = request.Name;

            if (!string.IsNullOrWhiteSpace(request.Specialization) || request.Specialization != "")
                doctor.Specialization = request.Specialization;

            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
