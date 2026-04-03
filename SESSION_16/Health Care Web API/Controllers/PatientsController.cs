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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Health_Care_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly HEALTH_CARE_SYSTEM_DBContext _context;
        private readonly ILogger<PatientsController> _logger;
        private readonly IMapper _mapper;

        public PatientsController(HEALTH_CARE_SYSTEM_DBContext context, ILogger<PatientsController> logger, IMapper mapper)
        {
            _context = context;
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

            var query = _context.Patients
                .Include(a => a.Appointments)   
                    .ThenInclude(d => d.Doctor)
                .AsNoTracking();
            var searchName = name?.Trim();
            if (!string.IsNullOrWhiteSpace(searchName))
                query = query.Where(p => EF.Functions.Like(p.Name, $"%{searchName}%"));


            var totalCount = await query.CountAsync();

            var patients = await query
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => _mapper.Map<PatientDTO>(p))
                .ToListAsync();
                    

            _logger.LogInformation(
                $"Retrieved {patients.Count} patients (page {page}/{(int)Math.Ceiling((double)totalCount / pageSize)}) with filter: name='{name}'.");

            return Ok(GenericResult<PagedResult<PatientDTO>>.Success(
                new PagedResult<PatientDTO>(patients, page, pageSize, totalCount)));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResult<PatientDTO>>> GetPatient(int id, int page = 1, int pageSize = 10)
        {
            var patient = await _context.Patients
                .AsNoTracking()
                .Include(a => a.Appointments)
                    .ThenInclude(d => d.Doctor) 
                .Where(p => p.Id == id)
                .Select(p => _mapper.Map<PatientDTO>(p))
                .FirstOrDefaultAsync();

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
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            var patientResponse = _mapper.Map<SlimPatientDTO>(patient);
            _logger.LogInformation($"Created patient with Id: {patientResponse.Id}.");
            return CreatedAtAction(nameof(GetPatient), new { id = patientResponse.Id },
                GenericResult<SlimPatientDTO>.Success(patientResponse));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResult<SlimPatientDTO>>> UpdatePatient(int id, UpdatePatientRequest request)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                _logger.LogWarning($"No patient found with Id: {id} to update.");
                return NotFound(GenericResult<SlimPatientDTO>.Failure($"No patient found with Id: {id}."));
            }

            if (!string.IsNullOrWhiteSpace(request.Name) && request.Name != "" && request.Name != "string")
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

            return Ok(GenericResult<SlimPatientDTO>.Success(_mapper.Map<SlimPatientDTO>(patient)));
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
            return Ok(GenericResult<SlimPatientDTO>.Success(_mapper.Map<SlimPatientDTO>(patient)));
        }
    }
}