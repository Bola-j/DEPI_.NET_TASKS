using AutoMapper;
using Health_Care_Web_API.DTOs.AppointmentDTO;
using Health_Care_Web_API.Models;
using Health_Care_Web_API.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Health_Care_Web_API.DTOs.DoctorDTO;
using Health_Care_Web_API.DTOs.PatientDTO;
using Health_Care_Web_API.Services;

namespace Health_Care_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger<AppointmentsController> _logger;
        private readonly IMapper _mapper;

        public AppointmentsController(IAppointmentService appointmentService, ILogger<AppointmentsController> logger, IMapper mapper)
        {
            _appointmentService = appointmentService;
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
            var result = await _appointmentService.GetAllAsync(page, pageSize);
            if (result.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    GenericResult<PagedResult<AppointmentDTO>>.Failure(result.Error));
            }

            var appointments = result.Value.Data.Select(_mapper.Map<AppointmentDTO>).ToList();

            _logger.LogInformation(
                $"Retrieved {appointments.Count} appointments (page {page}/{(int)Math.Ceiling((double)result.Value.TotalCount / pageSize)}).");

            return Ok(GenericResult<PagedResult<AppointmentDTO>>.Success(
                new PagedResult<AppointmentDTO>(appointments, page, pageSize, result.Value.TotalCount)));
        }

        [HttpGet("ByPatientAndDoctor")]
        public async Task<ActionResult<GenericResult<AppointmentDTO>>> GetAppointment(int DocotrId, int PatientId)
        {
            var result = await _appointmentService.GetByPatientAndDoctorAsync(PatientId, DocotrId);

            if (result.IsFailure)
            {
                _logger.LogWarning($"No appointment found for DoctorId: {DocotrId} and PatientId: {PatientId}.");
                return NotFound(GenericResult<AppointmentDTO>.Failure($"No appointment found for DoctorId: {DocotrId} and PatientId: {PatientId}."));
            }

            _logger.LogInformation($"Retrieved appointment for DoctorId: {DocotrId} and PatientId: {PatientId}.");
            return Ok(GenericResult<AppointmentDTO>.Success(_mapper.Map<AppointmentDTO>(result.Value)));
        }
        [HttpPost]
        public async Task<ActionResult<GenericResult<AppointmentDTO>>> CreateAppointment(CreateAppointmentRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("CreateAppointment request is null.");
                return BadRequest(Result.Failure("Invalid request data."));
            }
            var doctorExists = await _appointmentService.DoctorExistsAsync(request.DoctorId);

            var patientExists = await _appointmentService.PatientExistsAsync(request.PatientId);

            if (!doctorExists || !patientExists)
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
            var createResult = await _appointmentService.CreateAsync(appointment);
            if (createResult.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    GenericResult<SlimAppointmentDTO>.Failure(createResult.Error));
            }

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

            var getResult = await _appointmentService.GetByPatientAndDoctorAsync(request.PatientId, request.DoctorId);

            if (getResult.IsFailure)
            {
                _logger.LogWarning($"No appointment found for DoctorId: {request.DoctorId} and PatientId: {request.PatientId} to update.");
                return NotFound(GenericResult<SlimAppointmentDTO>.Failure($"No appointment found for DoctorId: {request.DoctorId} and PatientId: {request.PatientId}."));
            }
            var appointment = getResult.Value;
            if (request.AppointmentDate.HasValue && request.AppointmentDate.Value >= DateTime.Now)
            {
                appointment.AppointmentDate = request.AppointmentDate.Value;
            }
            else
            {
                _logger.LogWarning($"Invalid AppointmentDate: {request.AppointmentDate.Value}. Date must be in the future.\n The date stays as it is.");
                request.AppointmentDate = appointment.AppointmentDate; // Keep the existing date if the new one is invalid
                
            }

            _mapper.Map(request, appointment);
            var updateResult = await _appointmentService.UpdateAsync(appointment);
            if (updateResult.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    GenericResult<SlimAppointmentDTO>.Failure(updateResult.Error));
            }

            _logger.LogInformation($"Updated appointment for DoctorId: {request.DoctorId} and PatientId: {request.PatientId} with new date: {appointment.AppointmentDate}.");
            return Ok(GenericResult<SlimAppointmentDTO>.Success(_mapper.Map<SlimAppointmentDTO>(_mapper.Map(appointment,new SlimAppointmentDTO()))));
        }

        [HttpDelete]
        public async Task<ActionResult<GenericResult<SlimAppointmentDTO>>> Delete(int PatientId, int DoctorId)
        {
            var deleteResult = await _appointmentService.DeleteByPatientAndDoctorAsync(PatientId, DoctorId);
            if (deleteResult.IsFailure)
            {
                return NotFound(GenericResult<SlimAppointmentDTO>.Failure("Appointment not found."));
            }

            _logger.LogInformation($"Deleted appointment for DoctorId: {DoctorId} and PatientId: {PatientId}.");

            return Ok(GenericResult<SlimAppointmentDTO>.Success(_mapper.Map<SlimAppointmentDTO>(deleteResult.Value)));
        }

    }
}