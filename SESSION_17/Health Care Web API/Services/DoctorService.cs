using Health_Care_Web_API.Models;
using Health_Care_Web_API.Repositories;
using Health_Care_Web_API.Results;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_Web_API.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IGenericRepository<Doctor> _doctorRepository;

        public DoctorService(IGenericRepository<Doctor> doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<GenericResult<PagedResult<Doctor>>> GetAllAsync(string? name, int page, int pageSize)
        {
            var doctorsData = await _doctorRepository.Query()
                .Include(d => d.Appointments)
                .ThenInclude(a => a.Patient)
                .ToListAsync();

            IEnumerable<Doctor> query = doctorsData;

            var searchName = name?.Trim();
            if (!string.IsNullOrWhiteSpace(searchName))
            {
                query = query.Where(d => d.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = query.Count();
            var doctors = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return GenericResult<PagedResult<Doctor>>.Success(new PagedResult<Doctor>(doctors, page, pageSize, totalCount));
        }

        public async Task<GenericResult<Doctor>> GetByIdAsync(int id)
        {
            var doctor = await _doctorRepository.Query(false)
                .Include(d => d.Appointments)
                .ThenInclude(a => a.Patient)
                .FirstOrDefaultAsync(d => d.Id == id);

            return doctor is null
                ? GenericResult<Doctor>.Failure($"No doctor found with Id: {id}.")
                : GenericResult<Doctor>.Success(doctor);
        }

        public Task<GenericResult<Doctor>> CreateAsync(Doctor doctor)
            => _doctorRepository.CreateAsync(doctor);

        public Task<GenericResult<Doctor>> UpdateAsync(Doctor doctor)
            => _doctorRepository.UpdateAsync(doctor);

        public Task<GenericResult<Doctor>> DeleteAsync(int id)
            => _doctorRepository.DeleteAsync(id);
    }
}
