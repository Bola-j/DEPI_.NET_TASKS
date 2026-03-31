using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Health_Care_Web_API.Models;
using Microsoft.EntityFrameworkCore;
using Health_Care_Web_API.DTOs.AppointmentDTO;
using Health_Care_Web_API.DTOs.DoctorDTO;
using Health_Care_Web_API.DTOs.PatientDTO;


namespace Health_Care_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly HEALTH_CARE_SYSTEM_DBContext _context;

        public AppointmentsController(HEALTH_CARE_SYSTEM_DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointments()
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

            return Ok(appointments);
        }

        [HttpGet("ByPatientAndDoctor")]
        public async Task<ActionResult<AppointmentDTO>> GetAppointment(int DocotrId, int PatientId)
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
                return NotFound();
            }

            return Ok(appointment);
        }
        [HttpPost]
        public async Task<ActionResult<AppointmentDTO>> CreateAppointment(CreateAppointmentRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request Date");
            var doctor = await _context.Doctors.FindAsync(request.DoctorId);

            var patient = await _context.Patients.FindAsync(request.PatientId);

            if (doctor == null || patient == null)
            {
                return BadRequest("Invalid DoctorId or PatientId.");
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
            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, response);
        }

        [HttpPut("ByPatientAndDoctor")]
        public async Task<IActionResult> Update(UpdateAppointmentRequest request)
        {
            if(request == null)
            {
                return BadRequest("Invalid request data");
            }

            var appointment = await _context.Appointments.Where(a => a.PatientId == request.PatientId && a.DoctorId == request.DoctorId).FirstOrDefaultAsync();

            if (appointment == null) 
            {
                return NotFound();
            }
            if (request.AppointmentDate.HasValue && request.AppointmentDate.Value >= DateTime.Now)
            {
                appointment.AppointmentDate = request.AppointmentDate.Value;
            }

            _context.Update(appointment);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int PatientId, int DoctorId) 
        {
            var appointment = await _context.Appointments.Where(a => a.PatientId == PatientId && a.DoctorId == DoctorId).FirstOrDefaultAsync();
            if (appointment == null) 
            {
                return NotFound();
            }

            _context.Remove(appointment);
            _context.SaveChanges();

            return Ok();
        }

    }
}