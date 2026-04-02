using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Health_Care_Web_API.Models;
using Microsoft.EntityFrameworkCore;
using Health_Care_Web_API.DTOs.AppointmentDTO;
using Health_Care_Web_API.DTOs.DoctorDTO;
using Health_Care_Web_API.DTOs.PatientDTO;
using Microsoft.Extensions.Logging.Console;
using Health_Care_Web_API.Results;

namespace Health_Care_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly HEALTH_CARE_SYSTEM_DBContext _context;
        private readonly ILogger<HEALTH_CARE_SYSTEM_DBContext> _logger;

        public AppointmentsController(HEALTH_CARE_SYSTEM_DBContext context, ILogger<HEALTH_CARE_SYSTEM_DBContext> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResult<IEnumerable<AppointmentDTO>>>> GetAppointments()
        {
            var appointments = await _context.Appointments
                .Select(a => new AppointmentDTO
                {
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    AppointmentDate = a.AppointmentDate,
                    Doctor = new SlimDoctorDTO
                    {
                        Id = a.Doctor.Id,
                        Name = a.Doctor.Name,
                        Specialization = a.Doctor.Specialization
                    },
                    Patient = new SlimPatientDTO
                    {
                        Id = a.Patient.Id,
                        Name = a.Patient.Name,
                        DateOfBirth = DateOnly.FromDateTime(a.Patient.DateOfBirth)
                    }

                })
                .ToListAsync();

            _logger.LogInformation($"Retrieved {appointments.Count} appointments from the database.");

            return Ok(GenericResult<IEnumerable<AppointmentDTO>>.Success(appointments));
        }

        [HttpGet("ByPatientAndDoctor")]
        public async Task<ActionResult<GenericResult<AppointmentDTO>>> GetAppointment(int DocotrId, int PatientId)
        {
            var appointment = await _context.Appointments
                .Where(a => a.PatientId == PatientId && a.DoctorId == DocotrId)
                .Select(a => new AppointmentDTO
                {
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    AppointmentDate = a.AppointmentDate,
                    Doctor = new SlimDoctorDTO
                    {
                        Id = a.Doctor.Id,
                        Name = a.Doctor.Name,
                        Specialization = a.Doctor.Specialization
                    },
                    Patient = new SlimPatientDTO
                    {
                        Id = a.Patient.Id,
                        Name = a.Patient.Name,
                        DateOfBirth = DateOnly.FromDateTime(a.Patient.DateOfBirth)
                    }
                })
                .FirstOrDefaultAsync();

            if (appointment == null)
            {
                _logger.LogWarning($"No appointment found for DoctorId: {DocotrId} and PatientId: {PatientId}.");
                return NotFound(GenericResult<AppointmentDTO>.Failure($"No appointment found for DoctorId: {DocotrId} and PatientId: {PatientId}."));
            }

            _logger.LogInformation($"Retrieved appointment for DoctorId: {DocotrId} and PatientId: {PatientId}.");
            return Ok(GenericResult<AppointmentDTO>.Success(appointment));
        }
        [HttpPost]
        public async Task<ActionResult<GenericResult<AppointmentDTO>>> CreateAppointment(CreateAppointmentRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("CreateAppointment request is null.");
                return BadRequest(Result.Failure("Invalid request data."));
            }
            var doctor = await _context.Doctors.FindAsync(request.DoctorId);

            var patient = await _context.Patients.FindAsync(request.PatientId);

            if (doctor == null || patient == null)
            {
                _logger.LogWarning($"Invalid DoctorId: {request.DoctorId} or PatientId: {request.PatientId}.");
                return BadRequest(Result.Failure("Invalid DoctorId or PatientId."));
            }

            if (request.AppointmentDate.HasValue && request.AppointmentDate.Value < DateTime.Now)
            {
                _logger.LogWarning($"Invalid AppointmentDate: {request.AppointmentDate.Value}. Date must be in the future.");
                return BadRequest(Result.Failure("Appointment date must be in the future."));
            }


            var appointment = new Appointment
            {
                DoctorId = request.DoctorId,
                PatientId = request.PatientId,
                AppointmentDate = (request.AppointmentDate.HasValue && request.AppointmentDate.Value >= DateTime.Now) ? request.AppointmentDate.Value : DateTime.Now,
            };
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            var response = new AppointmentDTO
            {
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate,
                Doctor = new SlimDoctorDTO
                {
                    Id = doctor.Id,
                    Name = doctor.Name,
                    Specialization = doctor.Specialization
                },
                Patient = new SlimPatientDTO
                {
                    Id = patient.Id,
                    Name = patient.Name,
                    DateOfBirth = DateOnly.FromDateTime(patient.DateOfBirth)
                }
            };
            _logger.LogInformation($"Created new appointment for DoctorId: {request.DoctorId} and PatientId: {request.PatientId}.");

            return CreatedAtAction(nameof(GetAppointment), new { DocotrId = appointment.DoctorId, PatientId = appointment.PatientId },
                GenericResult<SlimAppointmentDTO>.Success(new SlimAppointmentDTO
                {
                    PatientId = appointment.PatientId,
                    DoctorId = appointment.DoctorId,
                    AppointmentDate = appointment.AppointmentDate
                }));
        }

        [HttpPut("ByPatientAndDoctor")]
        public async Task<ActionResult<GenericResult<SlimAppointmentDTO>>> Update(UpdateAppointmentRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("UpdateAppointment request is null.");
                return BadRequest(Result.Failure("Invalid request data."));
            }

            var appointment = await _context.Appointments.Where(a => a.PatientId == request.PatientId && a.DoctorId == request.DoctorId).FirstOrDefaultAsync();

            if (appointment == null)
            {
                _logger.LogWarning($"No appointment found for DoctorId: {request.DoctorId} and PatientId: {request.PatientId} to update.");
                return NotFound(GenericResult<SlimAppointmentDTO>.Failure($"No appointment found for DoctorId: {request.DoctorId} and PatientId: {request.PatientId}."));
            }
            if (request.AppointmentDate.HasValue && request.AppointmentDate.Value >= DateTime.Now)
            {
                appointment.AppointmentDate = request.AppointmentDate.Value;
            }
            else
            {
                _logger.LogWarning($"Invalid AppointmentDate: {request.AppointmentDate.Value}. Date must be in the future.");
                return BadRequest(Result.Failure("Appointment date must be in the future."));
            }

            _context.Update(appointment);
            _context.SaveChanges();

            _logger.LogInformation($"Updated appointment for DoctorId: {request.DoctorId} and PatientId: {request.PatientId} with new date: {appointment.AppointmentDate}.");
            return Ok(GenericResult<SlimAppointmentDTO>.Success(new SlimAppointmentDTO
            {
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate
            }));
        }

        [HttpDelete]
        public async Task<ActionResult<GenericResult<SlimAppointmentDTO>>> Delete(int PatientId, int DoctorId)
        {
            var appointment = await _context.Appointments.Where(a => a.PatientId == PatientId && a.DoctorId == DoctorId).FirstOrDefaultAsync();
            if (appointment == null)
            {
                return NotFound(GenericResult<SlimAppointmentDTO>.Failure("Appointment not found."));
            }

            _context.Remove(appointment);
            _context.SaveChanges();
            _logger.LogInformation($"Deleted appointment for DoctorId: {DoctorId} and PatientId: {PatientId}.");

            return Ok(GenericResult<SlimAppointmentDTO>.Success(new SlimAppointmentDTO
            {
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate
            }));
        }

    }
}