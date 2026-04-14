using Health_Care_Web_API.Models;
using Health_Care_Web_API.Results;

namespace Health_Care_Web_API.Services
{
    public interface IAppointmentService
    {
        Task<GenericResult<PagedResult<Appointment>>> GetAllAsync(int page, int pageSize);
        Task<GenericResult<Appointment>> GetByPatientAndDoctorAsync(int patientId, int doctorId);
        Task<GenericResult<Appointment>> CreateAsync(Appointment appointment);
        Task<GenericResult<Appointment>> UpdateAsync(Appointment appointment);
        Task<GenericResult<Appointment>> DeleteByPatientAndDoctorAsync(int patientId, int doctorId);
        Task<bool> DoctorExistsAsync(int doctorId);
        Task<bool> PatientExistsAsync(int patientId);
    }
}
