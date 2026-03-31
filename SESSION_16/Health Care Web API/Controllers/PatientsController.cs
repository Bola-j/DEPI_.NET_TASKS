using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Health_Care_Web_API.Models;
using Microsoft.EntityFrameworkCore;
using Health_Care_Web_API.DTOs.PatientDTO;
using Health_Care_Web_API.DTOs.DoctorDTO;
using Health_Care_Web_API.DTOs.AppointmentDTO;

namespace Health_Care_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly HEALTH_CARE_SYSTEM_DBContext _context;

        public PatientsController(HEALTH_CARE_SYSTEM_DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetPatients()
        {
            var patientResponses = await _context.Patients
                .AsNoTracking()
                .Select(p => new PatientDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    DateOfBirth = DateOnly.FromDateTime(p.DateOfBirth),
                    Appointments = p.Appointments.Where(a => a.PatientId == p.Id)
                        .Select(a => new AppointmentWithDrDTO
                        {
                            AppointmentDate = a.AppointmentDate,
                            Doctor = new SlimDoctorDTO
                            {
                                Id = a.Doctor.Id,
                                Name = a.Doctor.Name,
                                Specialization = a.Doctor.Specialization
                            }
                        }).ToList()


                }).ToListAsync();


            return Ok(patientResponses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDTO>> GetPatient(int id)
        {
            var patient = await _context.Patients
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new PatientDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    DateOfBirth = DateOnly.FromDateTime(p.DateOfBirth),
                    Appointments = p.Appointments.Where(a => a.PatientId == p.Id)
                        .Select(a => new AppointmentWithDrDTO
                        {

                            AppointmentDate = a.AppointmentDate,
                            Doctor = new SlimDoctorDTO
                            {
                                Id = a.Doctor.Id,
                                Name = a.Doctor.Name,
                                Specialization = a.Doctor.Specialization
                            }
                        }).ToList()
                }).FirstOrDefaultAsync();

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult<SlimPatientDTO>> CreatePatient(CreatePatientRequest request)
        {

            var patient = new Patient();
            if (!string.IsNullOrWhiteSpace(request.Name) || request.Name != "")
                patient.Name = request.Name;

            if (request.DateOfBirth.HasValue && request.DateOfBirth.Value <= DateOnly.FromDateTime(DateTime.Now))
                patient.DateOfBirth = request.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue);
            // means the Date of Birth is entered earlier than today, surely the patient is not born in the future, so we can set the year to 0001, which is the minimum value for DateTime



            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            var patientResponse = new SlimPatientDTO
            {
                Id = patient.Id,
                Name = patient.Name,
                DateOfBirth = request.DateOfBirth ?? DateOnly.FromDateTime(DateTime.MinValue)
            };
            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patientResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, UpdatePatientRequest request)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(request.Name) || request.Name != "" )
                patient.Name = request.Name;

            if (request.DateOfBirth.HasValue && request.DateOfBirth.Value <= DateOnly.FromDateTime(DateTime.Now))
                patient.DateOfBirth = request.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue);
            // means the Date of Birth is entered earlier than today, surely the patient is not born in the future, so we can set the year to 0001, which is the minimum value for DateTime


            _context.Update(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}