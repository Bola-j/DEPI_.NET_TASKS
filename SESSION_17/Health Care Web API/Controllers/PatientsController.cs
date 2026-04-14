using Azure;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Health_Care_Web_API.DTOs.AppointmentDTO;
using Health_Care_Web_API.DTOs.DoctorDTO;
using Health_Care_Web_API.DTOs.PatientDTO;
using Health_Care_Web_API.Models;
using Health_Care_Web_API.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Health_Care_Web_API.Services;

namespace Health_Care_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<PatientsController> _logger;
        private readonly IMapper _mapper;

        public PatientsController(IPatientService patientService, ILogger<PatientsController> logger, IMapper mapper)
        {
            _patientService = patientService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResult<PagedResult<PatientDTO>>>> GetPatients(string? name, int page = 1, int pageSize = 10)
        {

            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}.");
                return BadRequest(Result.Failure("Page and PageSize must be greater than 0."));
            }

            var result = await _patientService.GetAllAsync(name, page, pageSize);
            if (result.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    GenericResult<PagedResult<PatientDTO>>.Failure(result.Error));
            }

            var patients = result.Value.Data.Select(_mapper.Map<PatientDTO>).ToList();
                    

            _logger.LogInformation(
                $"Retrieved {patients.Count} patients (page {page}/{(int)Math.Ceiling((double)result.Value.TotalCount / pageSize)}) with filter: name='{name}'.");

            return Ok(GenericResult<PagedResult<PatientDTO>>.Success(
                new PagedResult<PatientDTO>(patients, page, pageSize, result.Value.TotalCount)));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResult<PatientDTO>>> GetPatient(int id)
        {
            var result = await _patientService.GetByIdAsync(id);

            if (result.IsFailure)
            {
                _logger.LogWarning($"No patient found with Id: {id}.");
                return NotFound(GenericResult<PatientDTO>.Failure($"No patient found with Id: {id}."));
            }

            _logger.LogInformation($"Retrieved patient with Id: {id}.");
            return Ok(GenericResult<PatientDTO>.Success(_mapper.Map<PatientDTO>(result.Value)));
        }

        [HttpPost]
        public async Task<ActionResult<GenericResult<SlimPatientDTO>>> CreatePatient(CreatePatientRequest request)
        {

            
            if (string.IsNullOrWhiteSpace(request.Name) || request.Name == "" || request.Name == "string")
            {

                _logger.LogWarning("Attempted to create a patient with an empty or whitespace name.");
                return BadRequest(Result.Failure("Patient name cannot be empty or whitespace."));
            }


            if (!request.DateOfBirth.HasValue || request.DateOfBirth.Value >= DateOnly.FromDateTime(DateTime.Now))
            { 
                _logger.LogWarning("Attempted to create a patient with an invalid Date of Birth.");
                return BadRequest(Result.Failure("Date of Birth must be a valid date and cannot be in the future."));
            }
            var patient =  _mapper.Map<Patient>(request);
            var createResult = await _patientService.CreateAsync(patient);
            if (createResult.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    GenericResult<SlimPatientDTO>.Failure(createResult.Error));
            }

            var patientResponse = _mapper.Map<SlimPatientDTO>(patient);
            _logger.LogInformation($"Created patient with Id: {patientResponse.Id}.");
            return CreatedAtAction(nameof(GetPatient), new { id = patientResponse.Id },
                GenericResult<SlimPatientDTO>.Success(patientResponse));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResult<SlimPatientDTO>>> UpdatePatient(int id, UpdatePatientRequest request)
        {
            var getResult = await _patientService.GetByIdAsync(id);
            if (getResult.IsFailure)
            {
                _logger.LogWarning($"No patient found with Id: {id} to update.");
                return NotFound(GenericResult<SlimPatientDTO>.Failure($"No patient found with Id: {id}."));
            }

            var patient = getResult.Value;

            if (!string.IsNullOrWhiteSpace(request.Name) && request.Name != "" && request.Name != "string")
            {

                patient.Name = request.Name;
            }
            else
            {
                _logger.LogWarning("Attempted to update a patient with an empty or whitespace name.\n The name Stays as it is");
                request.Name = patient.Name; // Keep the existing name if the new one is invalid
            }
            if (request.DateOfBirth.HasValue && request.DateOfBirth.Value <= DateOnly.FromDateTime(DateTime.Now))
            {

                patient.DateOfBirth = request.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue);
            }
            // means the Date of Birth is entered earlier than today, surely the patient is not born in the future, so we can set the year to 0001, which is the minimum value for DateTime
            else
            {
                _logger.LogWarning("Attempted to update a patient with an invalid Date of Birth.\n The Date of Birth stays as it is.");
                request.DateOfBirth = DateOnly.FromDateTime(patient.DateOfBirth); // Keep the existing Date of Birth if the new one is invalid
            }


            _mapper.Map(request, patient);
            var updateResult = await _patientService.UpdateAsync(patient);
            if (updateResult.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    GenericResult<SlimPatientDTO>.Failure(updateResult.Error));
            }

            _logger.LogInformation($"Updated patient with Id: {id}.");

            return Ok(GenericResult<SlimPatientDTO>.Success(_mapper.Map<SlimPatientDTO>(_mapper.Map(patient,new SlimPatientDTO()))));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResult<SlimPatientDTO>>> DeletePatient(int id)
        {
            var deleteResult = await _patientService.DeleteAsync(id);
            if (deleteResult.IsFailure)
            {
                _logger.LogWarning($"No patient found with Id: {id} to delete.");
                return NotFound(GenericResult<SlimPatientDTO>.Failure($"No patient found with Id: {id}."));
            }

            _logger.LogInformation($"Deleted patient with Id: {id}.");
            return Ok(GenericResult<SlimPatientDTO>.Success(_mapper.Map<SlimPatientDTO>(deleteResult.Value)));
        }
    }
}