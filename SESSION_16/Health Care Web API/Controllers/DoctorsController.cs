using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Health_Care_Web_API.Models;
using Microsoft.EntityFrameworkCore;
using Health_Care_Web_API.DTOs.DoctorDTO;
using Health_Care_Web_API.DTOs.AppointmentDTO;
using Health_Care_Web_API.DTOs.PatientDTO;
using Microsoft.Extensions.Logging;
using Health_Care_Web_API.Results;
using AutoMapper;


namespace Health_Care_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly HEALTH_CARE_SYSTEM_DBContext _context;
        private readonly ILogger<DoctorsController> _logger;
        private readonly IMapper _mapper;

        public DoctorsController(HEALTH_CARE_SYSTEM_DBContext context, ILogger<DoctorsController> logger, IMapper mapper)
        {
            _context = context;
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

            var query = _context.Doctors
                .Include(d => d.Appointments)
                .ThenInclude(a => a.Patient)
                .AsNoTracking();

            var searchName = name?.Trim();
            if (!string.IsNullOrWhiteSpace(searchName))
                query = query.Where(p => EF.Functions.Like(p.Name, $"%{searchName}%"));

            var totalCount = await query.CountAsync();



            var doctorResponses = await query
                .AsNoTracking()
                .Select(d => _mapper.Map<DoctorDTO>(d))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            _logger.LogInformation(
                $"Retrieved {doctorResponses.Count} doctors (page {page}/{(int)Math.Ceiling((double)totalCount / pageSize)}) with filter: name='{name}'.");
            return Ok(GenericResult<PagedResult<DoctorDTO>>.Success(
                new PagedResult<DoctorDTO>(doctorResponses, page, pageSize, totalCount)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResult<DoctorDTO>>> GetDoctor(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Appointments)
                .ThenInclude(a => a.Patient)
                .AsNoTracking()
                .Where(d => d.Id == id)
                .Select(d => _mapper.Map<DoctorDTO>(d))
                .FirstOrDefaultAsync();
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
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            var doctorResponse = _mapper.Map<SlimDoctorDTO>(doctor);

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
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Updated doctor with Id: {id}, Name: {doctor.Name}, Specialization: {doctor.Specialization}.");
            return Ok(GenericResult<SlimDoctorDTO>.Success(_mapper.Map<SlimDoctorDTO>(_mapper.Map(doctor,new SlimDoctorDTO()))));
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
            return Ok(GenericResult<SlimDoctorDTO>.Success(_mapper.Map<SlimDoctorDTO>(doctor)));
        }
    }
}