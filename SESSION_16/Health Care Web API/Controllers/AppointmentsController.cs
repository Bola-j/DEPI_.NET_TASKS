using AutoMapper;
using AutoMapper.QueryableExtensions;
using Health_Care_Web_API.DTOs.AppointmentDTO;
using Health_Care_Web_API.Models;
using Health_Care_Web_API.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Health_Care_Web_API.DTOs.DoctorDTO;
using Health_Care_Web_API.DTOs.PatientDTO;

namespace Health_Care_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly HEALTH_CARE_SYSTEM_DBContext _context;
        private readonly ILogger<AppointmentsController> _logger;
        private readonly IMapper _mapper;

        public AppointmentsController(HEALTH_CARE_SYSTEM_DBContext context, ILogger<AppointmentsController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResult<PagedResult<AppointmentDTO>>>> GetAppointments(int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}.");
                return BadRequest(Result.Failure("Page and PageSize must be greater than 0."));
            }
            var query = _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .AsNoTracking();

            var totalCount = await query.CountAsync();

            var appointments = await query
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.PatientId)
                .ThenBy(a => a.DoctorId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<AppointmentDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            _logger.LogInformation(
                $"Retrieved {appointments.Count} appointments (page {page}/{(int)Math.Ceiling((double)totalCount / pageSize)}).");

            return Ok(GenericResult<PagedResult<AppointmentDTO>>.Success(
                new PagedResult<AppointmentDTO>(appointments, page, pageSize, totalCount)));
        }

        [HttpGet("ByPatientAndDoctor")]
        public async Task<ActionResult<GenericResult<AppointmentDTO>>> GetAppointment(int DocotrId, int PatientId)
        {
            var appointment = await _context.Appointments
                .Where(a => a.PatientId == PatientId && a.DoctorId == DocotrId)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Select(a => _mapper.Map<AppointmentDTO>(a))
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


            var appointment = _mapper.Map<Appointment>(request);
            //    new Appointment
            //{
            //    DoctorId = request.DoctorId,
            //    PatientId = request.PatientId,
            //    AppointmentDate = (request.AppointmentDate.HasValue && request.AppointmentDate.Value >= DateTime.Now) ? request.AppointmentDate.Value : DateTime.Now,
            //};
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<AppointmentDTO>(appointment);
            //var response = new AppointmentDTO
            //{
            //    PatientId = appointment.PatientId,
            //    DoctorId = appointment.DoctorId,
            //    AppointmentDate = appointment.AppointmentDate,
            //    Doctor = _mapper.Map<SlimDoctorDTO>(doctor),
            //    Patient = _mapper.Map<SlimPatientDTO>(patient)          
                
            //};
            _logger.LogInformation($"Created new appointment for DoctorId: {request.DoctorId} and PatientId: {request.PatientId}.");

            return CreatedAtAction(nameof(GetAppointment), new { DocotrId = appointment.DoctorId, PatientId = appointment.PatientId },
                GenericResult<SlimAppointmentDTO>.Success(_mapper.Map<SlimAppointmentDTO>(appointment)));
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
            return Ok(GenericResult<SlimAppointmentDTO>.Success(_mapper.Map<SlimAppointmentDTO>(appointment)));
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

            return Ok(GenericResult<SlimAppointmentDTO>.Success(_mapper.Map<SlimAppointmentDTO>(appointment)));
        }

    }
}