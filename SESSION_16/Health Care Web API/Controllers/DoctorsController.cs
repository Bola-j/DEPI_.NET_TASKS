using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Health_Care_Web_API.Models;
using Microsoft.EntityFrameworkCore;
using Health_Care_Web_API.DTOs.DoctorDTO;
using Health_Care_Web_API.DTOs.AppointmentDTO;
using Health_Care_Web_API.DTOs.PatientDTO;
using Microsoft.Extensions.Logging;
using Health_Care_Web_API.Results;


namespace Health_Care_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly HEALTH_CARE_SYSTEM_DBContext _context;
        private readonly ILogger<DoctorsController> _logger;

        public DoctorsController(HEALTH_CARE_SYSTEM_DBContext context, ILogger<DoctorsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResult<IEnumerable<DoctorDTO>>>> GetDoctors()
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

            _logger.LogInformation($"Retrieved {doctorResponses.Count} doctors from the database.");
            return Ok(GenericResult<IEnumerable<DoctorDTO>>.Success(doctorResponses));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResult<DoctorDTO>>> GetDoctor(int id)
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
                _logger.LogWarning($"No doctor found with Id: {id}.");
                return NotFound(GenericResult<DoctorDTO>.Failure($"No doctor found with Id: {id}."));
            }

            _logger.LogInformation($"Retrieved doctor with Id: {id}.");
            return Ok(GenericResult<DoctorDTO>.Success(doctor));
        }

        [HttpPost]
        public async Task<ActionResult<GenericResult<SlimDoctorDTO>>> CreateDoctor(CreateDoctorRequest request)
        {
            var doctor = new Doctor();

            if (!string.IsNullOrWhiteSpace(request.Name) || request.Name != "")
            {
                doctor.Name = request.Name;
            }
            else
            {
                _logger.LogWarning("Attempted to create a doctor with an empty or whitespace name.");
                return BadRequest(Result.Failure("Doctor name cannot be empty or whitespace."));
            }

            if (!string.IsNullOrWhiteSpace(request.Specialization) || request.Specialization != "")
            {
                doctor.Specialization = request.Specialization;
            }
            else
            {
                _logger.LogWarning("Attempted to create a doctor with an empty or whitespace specialization.");
                return BadRequest(Result.Failure("Doctor specialization cannot be empty or whitespace."));
            }

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            var doctorResponse = new SlimDoctorDTO
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Specialization = doctor.Specialization
            };

            _logger.LogInformation($"Created doctor with Id: {doctor.Id}, Name: {doctor.Name}, Specialization: {doctor.Specialization}.");

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id },
                GenericResult<SlimDoctorDTO>.Success(doctorResponse));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, UpdateDoctorRequest request)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                _logger.LogWarning($"No doctor found with Id: {id} to update.");
                return NotFound(GenericResult<SlimDoctorDTO>.Failure($"No doctor found with Id: {id}."));
            }

            if (!string.IsNullOrWhiteSpace(request.Name) || request.Name != "")
            {

                doctor.Name = request.Name;
            }
            else
            {
                _logger.LogWarning("Attempted to update a doctor with an empty or whitespace name.");
                return BadRequest(Result.Failure("Doctor name cannot be empty or whitespace."));

            }
            if (!string.IsNullOrWhiteSpace(request.Specialization) || request.Specialization != "")
            {
                doctor.Specialization = request.Specialization;
            }
            else
            {
                _logger.LogWarning("Attempted to update a doctor with an empty or whitespace specialization.");
                return BadRequest(Result.Failure("Doctor specialization cannot be empty or whitespace."));
            }

            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Updated doctor with Id: {id}, Name: {doctor.Name}, Specialization: {doctor.Specialization}.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResult<SlimDoctorDTO>>> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                _logger.LogWarning($"No doctor found with Id: {id} to delete.");
                return NotFound(GenericResult<SlimDoctorDTO>.Failure($"No doctor found with Id: {id}."));
            }
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Deleted doctor with Id: {id}.");
            return Ok(GenericResult<SlimDoctorDTO>.Success(new SlimDoctorDTO
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Specialization = doctor.Specialization
            }));
        }
    }
}