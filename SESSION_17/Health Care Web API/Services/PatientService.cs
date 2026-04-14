using Health_Care_Web_API.Models;
using Health_Care_Web_API.Repositories;
using Health_Care_Web_API.Results;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_Web_API.Services
{
    public class PatientService : IPatientService
    {
        private readonly IGenericRepository<Patient> _patientRepository;

        public PatientService(IGenericRepository<Patient> patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<GenericResult<PagedResult<Patient>>> GetAllAsync(string? name, int page, int pageSize)
        {
            var patientsData = await _patientRepository.Query()
                .Include(p => p.Appointments)
                .ThenInclude(a => a.Doctor)
                .ToListAsync();

            IEnumerable<Patient> query = patientsData;

            var searchName = name?.Trim();
            if (!string.IsNullOrWhiteSpace(searchName))
            {
                query = query.Where(p => p.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = query.Count();
            var patients = query
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return GenericResult<PagedResult<Patient>>.Success(new PagedResult<Patient>(patients, page, pageSize, totalCount));
        }

        public async Task<GenericResult<Patient>> GetByIdAsync(int id)
        {
            var patient = await _patientRepository.Query(false)
                .Include(p => p.Appointments)
                .ThenInclude(a => a.Doctor)
                .FirstOrDefaultAsync(p => p.Id == id);

            return patient is null
                ? GenericResult<Patient>.Failure($"No patient found with Id: {id}.")
                : GenericResult<Patient>.Success(patient);
        }

        public Task<GenericResult<Patient>> CreateAsync(Patient patient)
            => _patientRepository.CreateAsync(patient);

        public Task<GenericResult<Patient>> UpdateAsync(Patient patient)
            => _patientRepository.UpdateAsync(patient);

        public Task<GenericResult<Patient>> DeleteAsync(int id)
            => _patientRepository.DeleteAsync(id);
    }
}
