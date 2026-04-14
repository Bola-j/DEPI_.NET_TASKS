using Health_Care_Web_API.Models;
using Health_Care_Web_API.Results;

namespace Health_Care_Web_API.Services
{
    public interface IDoctorService
    {
        Task<GenericResult<PagedResult<Doctor>>> GetAllAsync(string? name, int page, int pageSize);
        Task<GenericResult<Doctor>> GetByIdAsync(int id);
        Task<GenericResult<Doctor>> CreateAsync(Doctor doctor);
        Task<GenericResult<Doctor>> UpdateAsync(Doctor doctor);
        Task<GenericResult<Doctor>> DeleteAsync(int id);
    }
}
