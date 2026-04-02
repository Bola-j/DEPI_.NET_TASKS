using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Health_Care_Web_API.Models;
using Microsoft.EntityFrameworkCore;
using Health_Care_Web_API.DTOs.PatientDTO;
using Health_Care_Web_API.DTOs.DoctorDTO;
using Health_Care_Web_API.DTOs.AppointmentDTO;
using Microsoft.Extensions.Logging;
using Health_Care_Web_API.Results;

namespace Health_Care_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly HEALTH_CARE_SYSTEM_DBContext _context;
        private readonly ILogger<PatientsController> _logger;

        public PatientsController(HEALTH_CARE_SYSTEM_DBContext context, ILogger<PatientsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResult<IEnumerable<PatientDTO>>>> GetPatients()
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

            _logger.LogInformation($"Retrieved {patientResponses.Count} patients from the database.");

            return Ok(GenericResult<IEnumerable<PatientDTO>>.Success(patientResponses));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResult<PatientDTO>>> GetPatient(int id)
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
                _logger.LogWarning($"No patient found with Id: {id}.");
                return NotFound(GenericResult<PatientDTO>.Failure($"No patient found with Id: {id}."));
            }

            _logger.LogInformation($"Retrieved patient with Id: {id}.");
            return Ok(GenericResult<PatientDTO>.Success(patient));
        }

        [HttpPost]
        public async Task<ActionResult<GenericResult<SlimPatientDTO>>> CreatePatient(CreatePatientRequest request)
        {

            var patient = new Patient();
            if (!string.IsNullOrWhiteSpace(request.Name) || request.Name != "")
            {
                patient.Name = request.Name;

            }
            else
            {
                _logger.LogWarning("Attempted to create a patient with an empty or whitespace name.");
                return BadRequest(Result.Failure("Patient name cannot be empty or whitespace."));
            }


            if (request.DateOfBirth.HasValue && request.DateOfBirth.Value <= DateOnly.FromDateTime(DateTime.Now))
            {

                patient.DateOfBirth = request.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue);
            }
            // means the Date of Birth is entered earlier than today, surely the patient is not born in the future, so we can set the year to 0001, which is the minimum value for DateTime
            else
            {
                _logger.LogWarning("Attempted to create a patient with an invalid Date of Birth.");
                return BadRequest(Result.Failure("Date of Birth must be a valid date and cannot be in the future."));
            }


            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            var patientResponse = new SlimPatientDTO
            {
                Id = patient.Id,
                Name = patient.Name,
                DateOfBirth = request.DateOfBirth ?? DateOnly.FromDateTime(DateTime.MinValue)
            };

            _logger.LogInformation($"Created patient with Id: {patient.Id}.");
            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id },
                GenericResult<SlimPatientDTO>.Success(patientResponse));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, UpdatePatientRequest request)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                _logger.LogWarning($"No patient found with Id: {id} to update.");
                return NotFound(GenericResult<SlimPatientDTO>.Failure($"No patient found with Id: {id}."));
            }

            if (!string.IsNullOrWhiteSpace(request.Name) || request.Name != "")
            {

                patient.Name = request.Name;
            }
            else
            {
                _logger.LogWarning("Attempted to update a patient with an empty or whitespace name.");
                return BadRequest(Result.Failure("Patient name cannot be empty or whitespace."));
            }
            if (request.DateOfBirth.HasValue && request.DateOfBirth.Value <= DateOnly.FromDateTime(DateTime.Now))
            {

                patient.DateOfBirth = request.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue);
            }
            // means the Date of Birth is entered earlier than today, surely the patient is not born in the future, so we can set the year to 0001, which is the minimum value for DateTime
            else
            {
                _logger.LogWarning("Attempted to update a patient with an invalid Date of Birth.");
                return BadRequest(Result.Failure("Date of Birth must be a valid date and cannot be in the future."));
            }

            _context.Update(patient);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Updated patient with Id: {id}.");

            return Ok(GenericResult<SlimPatientDTO>.Success(new SlimPatientDTO
            {
                Id = patient.Id,
                Name = patient.Name,
                DateOfBirth = DateOnly.FromDateTime(patient.DateOfBirth)
            }));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResult<SlimPatientDTO>>> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                _logger.LogWarning($"No patient found with Id: {id} to delete.");
                return NotFound(GenericResult<SlimPatientDTO>.Failure($"No patient found with Id: {id}."));
            }
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Deleted patient with Id: {id}.");
            return Ok(GenericResult<SlimPatientDTO>.Success(new SlimPatientDTO
            {
                Id = patient.Id,
                Name = patient.Name,
                DateOfBirth = DateOnly.FromDateTime(patient.DateOfBirth)
            }));
        }
    }
}