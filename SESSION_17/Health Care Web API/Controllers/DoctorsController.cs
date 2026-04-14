using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Health_Care_Web_API.Models;
using Health_Care_Web_API.DTOs.DoctorDTO;
using Health_Care_Web_API.DTOs.AppointmentDTO;
using Health_Care_Web_API.DTOs.PatientDTO;
using Microsoft.Extensions.Logging;
using Health_Care_Web_API.Results;
using AutoMapper;
using Health_Care_Web_API.Services;


namespace Health_Care_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly ILogger<DoctorsController> _logger;
        private readonly IMapper _mapper;

        public DoctorsController(IDoctorService doctorService, ILogger<DoctorsController> logger, IMapper mapper)
        {
            _doctorService = doctorService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResult<PagedResult<DoctorDTO>>>> GetDoctors(string? name, int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}.");
                return BadRequest(Result.Failure("Page and PageSize must be greater than 0."));
            }

            var result = await _doctorService.GetAllAsync(name, page, pageSize);
            if (result.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    GenericResult<PagedResult<DoctorDTO>>.Failure(result.Error));
            }

            var doctorResponses = result.Value.Data.Select(_mapper.Map<DoctorDTO>).ToList();

            _logger.LogInformation(
                $"Retrieved {doctorResponses.Count} doctors (page {page}/{(int)Math.Ceiling((double)result.Value.TotalCount / pageSize)}) with filter: name='{name}'.");
            return Ok(GenericResult<PagedResult<DoctorDTO>>.Success(
                new PagedResult<DoctorDTO>(doctorResponses, page, pageSize, result.Value.TotalCount)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResult<DoctorDTO>>> GetDoctor(int id)
        {
            var result = await _doctorService.GetByIdAsync(id);
            if (result.IsFailure)
            {
                _logger.LogWarning($"No doctor found with Id: {id}.");
                return NotFound(GenericResult<DoctorDTO>.Failure($"No doctor found with Id: {id}."));
            }

            _logger.LogInformation($"Retrieved doctor with Id: {id}.");
            return Ok(GenericResult<DoctorDTO>.Success(_mapper.Map<DoctorDTO>(result.Value)));
        }

        [HttpPost]
        public async Task<ActionResult<GenericResult<SlimDoctorDTO>>> CreateDoctor(CreateDoctorRequest request)
        {

            if (string.IsNullOrWhiteSpace(request.Name) || request.Name == "" || request.Name == "string")
            {
            
                _logger.LogWarning("Attempted to create a doctor with an empty or whitespace name.");
                return BadRequest(Result.Failure("Doctor name cannot be empty or whitespace."));
            }

            if (string.IsNullOrWhiteSpace(request.Specialization) || request.Specialization == "" || request.Specialization == "string")
            {
                _logger.LogWarning("Attempted to create a doctor with an empty or whitespace specialization.");
                return BadRequest(Result.Failure("Doctor specialization cannot be empty or whitespace."));
            }
            
            var doctor = _mapper.Map<Doctor>(request);
            var createResult = await _doctorService.CreateAsync(doctor);
            if (createResult.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    GenericResult<SlimDoctorDTO>.Failure(createResult.Error));
            }

            var doctorResponse = _mapper.Map<SlimDoctorDTO>(doctor);

            _logger.LogInformation($"Created doctor with Id: {doctor.Id}, Name: {doctor.Name}, Specialization: {doctor.Specialization}.");

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id },
                GenericResult<SlimDoctorDTO>.Success(doctorResponse));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResult<SlimDoctorDTO>>> UpdateDoctor(int id, UpdateDoctorRequest request)
        {
            var getResult = await _doctorService.GetByIdAsync(id);

            if (getResult.IsFailure)
            {
                _logger.LogWarning($"No doctor found with Id: {id} to update.");
                return NotFound(GenericResult<SlimDoctorDTO>.Failure($"No doctor found with Id: {id}."));
            }

            var doctor = getResult.Value;

            if (!string.IsNullOrWhiteSpace(request.Name) || request.Name != "" || request.Name != "string")
            {

                doctor.Name = request.Name;
            }
            else
            {
                _logger.LogWarning("Attempted to update a doctor with an empty or whitespace name.\n The name stays as it is.");
                request.Name = doctor.Name; // Keep the existing name if the new one is invalid
            }
            if (!string.IsNullOrWhiteSpace(request.Specialization) || request.Specialization != "" || request.Specialization != "string")
            {
                doctor.Specialization = request.Specialization;
            }
            else
            {
                _logger.LogWarning("Attempted to update a doctor with an empty or whitespace specialization.\n The specialization stays as it is.");
                request.Specialization = doctor.Specialization; // Keep the existing specialization if the new one is invalid
            }

            _mapper.Map(request, doctor);
            var updateResult = await _doctorService.UpdateAsync(doctor);
            if (updateResult.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    GenericResult<SlimDoctorDTO>.Failure(updateResult.Error));
            }

            _logger.LogInformation($"Updated doctor with Id: {id}, Name: {doctor.Name}, Specialization: {doctor.Specialization}.");
            return Ok(GenericResult<SlimDoctorDTO>.Success(_mapper.Map<SlimDoctorDTO>(_mapper.Map(doctor,new SlimDoctorDTO()))));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResult<SlimDoctorDTO>>> DeleteDoctor(int id)
        {
            var deleteResult = await _doctorService.DeleteAsync(id);
            if (deleteResult.IsFailure)
            {
                _logger.LogWarning($"No doctor found with Id: {id} to delete.");
                return NotFound(GenericResult<SlimDoctorDTO>.Failure($"No doctor found with Id: {id}."));
            }

            _logger.LogInformation($"Deleted doctor with Id: {id}.");
            return Ok(GenericResult<SlimDoctorDTO>.Success(_mapper.Map<SlimDoctorDTO>(deleteResult.Value)));
        }
    }
}