using Health_Care_Web_API.Models;
using Health_Care_Web_API.Repositories;
using Health_Care_Web_API.Results;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_Web_API.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IGenericRepository<Appointment> _appointmentRepository;
        private readonly IGenericRepository<Doctor> _doctorRepository;
        private readonly IGenericRepository<Patient> _patientRepository;

        public AppointmentService(
            IGenericRepository<Appointment> appointmentRepository,
            IGenericRepository<Doctor> doctorRepository,
            IGenericRepository<Patient> patientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        public async Task<GenericResult<PagedResult<Appointment>>> GetAllAsync(int page, int pageSize)
        {
            var appointmentsData = await _appointmentRepository.Query()
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .ToListAsync();

            IEnumerable<Appointment> query = appointmentsData;

            var totalCount = query.Count();
            var appointments = query
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.PatientId)
                .ThenBy(a => a.DoctorId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return GenericResult<PagedResult<Appointment>>.Success(new PagedResult<Appointment>(appointments, page, pageSize, totalCount));
        }

        public async Task<GenericResult<Appointment>> GetByPatientAndDoctorAsync(int patientId, int doctorId)
        {
            var appointment = await _appointmentRepository.Query(false)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.PatientId == patientId && a.DoctorId == doctorId);

            return appointment is null
                ? GenericResult<Appointment>.Failure($"No appointment found for DoctorId: {doctorId} and PatientId: {patientId}.")
                : GenericResult<Appointment>.Success(appointment);
        }

        public Task<GenericResult<Appointment>> CreateAsync(Appointment appointment)
            => _appointmentRepository.CreateAsync(appointment);

        public Task<GenericResult<Appointment>> UpdateAsync(Appointment appointment)
            => _appointmentRepository.UpdateAsync(appointment);

        public async Task<GenericResult<Appointment>> DeleteByPatientAndDoctorAsync(int patientId, int doctorId)
        {
            var appointment = await _appointmentRepository.Query(false)
                .FirstOrDefaultAsync(a => a.PatientId == patientId && a.DoctorId == doctorId);

            if (appointment is null)
            {
                return GenericResult<Appointment>.Failure("Appointment not found.");
            }

            return await _appointmentRepository.DeleteAsync(appointment.Id);
        }

        public async Task<bool> DoctorExistsAsync(int doctorId)
        {
            var doctors = await _doctorRepository.Query().ToListAsync();
            return doctors.Any(d => d.Id == doctorId);
        }

        public async Task<bool> PatientExistsAsync(int patientId)
        {
            var patients = await _patientRepository.Query().ToListAsync();
            return patients.Any(p => p.Id == patientId);
        }
    }
}
