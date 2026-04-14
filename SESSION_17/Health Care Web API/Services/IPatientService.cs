using Health_Care_Web_API.Models;
using Health_Care_Web_API.Results;

namespace Health_Care_Web_API.Services
{
    public interface IPatientService
    {
        Task<GenericResult<PagedResult<Patient>>> GetAllAsync(string? name, int page, int pageSize);
        Task<GenericResult<Patient>> GetByIdAsync(int id);
        Task<GenericResult<Patient>> CreateAsync(Patient patient);
        Task<GenericResult<Patient>> UpdateAsync(Patient patient);
        Task<GenericResult<Patient>> DeleteAsync(int id);
    }
}
