using AutoMapper;
using Health_Care_Web_API.DTOs.AppointmentDTO;
using Health_Care_Web_API.DTOs.DoctorDTO;
using Health_Care_Web_API.DTOs.PatientDTO;
using Health_Care_Web_API.Models;

namespace Health_Care_Web_API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ── Doctor ────────────────────────────────────────────────────────

            // Doctor → SlimDoctorDTO
            // Id, Name, Specialization all match by name — convention handles it.
            CreateMap<Doctor, SlimDoctorDTO>();

            // Doctor → DoctorDTO (inherits SlimDoctorDTO, adds Appointments)
            // Model has ICollection<Appointment>; DTO has ICollection<AppointmentWithPatientDTO>.
            // AutoMapper resolves the element type using the Appointment → AppointmentWithPatientDTO map below.
            CreateMap<Doctor, DoctorDTO>()
                .ForMember(dest => dest.Appointments,
                    opt => opt.MapFrom(src => src.Appointments));

            // CreateDoctorRequest → Doctor
            // Name and Specialization match by name.
            // Doctor.Id is DB-generated — ignore it so AutoMapper doesn't try to set it.
            CreateMap<CreateDoctorRequest, Doctor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());

            // UpdateDoctorRequest → Doctor
            // UpdateDoctorRequest.Id is string; Doctor.Id is int — ignore it (id comes from route).
            CreateMap<UpdateDoctorRequest, Doctor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());

            // ── Patient ───────────────────────────────────────────────────────

            // Patient → SlimPatientDTO
            // DateOfBirth: model is DateTime, DTO is DateOnly — explicit conversion required.
            CreateMap<Patient, SlimPatientDTO>()
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DateOfBirth)));

            // Patient → PatientDTO (inherits SlimPatientDTO, adds Appointments)
            // Model has ICollection<Appointment>; DTO has ICollection<AppointmentWithDrDTO>.
            // DateOfBirth conversion still required since SlimPatientDTO members are re-mapped.
            CreateMap<Patient, PatientDTO>()
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DateOfBirth)))
                .ForMember(dest => dest.Appointments,
                    opt => opt.MapFrom(src => src.Appointments));

            // CreatePatientRequest → Patient
            // DateOfBirth: request is DateOnly?, model is DateTime — convert with fallback.
            CreateMap<CreatePatientRequest, Patient>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore())
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src =>
                        src.DateOfBirth.HasValue
                            ? src.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue)
                            : DateTime.MinValue));

            // UpdatePatientRequest → Patient
            // UpdatePatientRequest.Id is int and matches Patient.Id — ignore it (id comes from route).
            CreateMap<UpdatePatientRequest, Patient>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore())
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src =>
                        src.DateOfBirth.HasValue
                            ? src.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue)
                            : DateTime.MinValue));

            // ── Appointment ───────────────────────────────────────────────────

            // Appointment → SlimAppointmentDTO
            // PatientId, DoctorId, AppointmentDate all match by name — convention handles it.
            CreateMap<Appointment, SlimAppointmentDTO>();

            // Appointment → AppointmentDTO (inherits SlimAppointmentDTO, adds Doctor + Patient)
            // Nested objects: AutoMapper resolves them using the
            // Doctor → SlimDoctorDTO and Patient → SlimPatientDTO maps defined above.
            CreateMap<Appointment, AppointmentDTO>()
                .ForMember(dest => dest.Doctor,
                    opt => opt.MapFrom(src => src.Doctor))
                .ForMember(dest => dest.Patient,
                    opt => opt.MapFrom(src => src.Patient));

            // Appointment → AppointmentWithPatientDTO
            // AppointmentDate matches by name.
            // Patient: resolved via Patient → SlimPatientDTO map.
            CreateMap<Appointment, AppointmentWithPatientDTO>()
                .ForMember(dest => dest.AppointmentDate,
                    opt => opt.MapFrom(src => src.AppointmentDate))
                .ForMember(dest => dest.Patient,
                    opt => opt.MapFrom(src => src.Patient));

            // Appointment → AppointmentWithDrDTO
            // AppointmentDate matches by name.
            // Doctor: resolved via Doctor → SlimDoctorDTO map.
            CreateMap<Appointment, AppointmentWithDrDTO>()
                .ForMember(dest => dest.AppointmentDate,
                    opt => opt.MapFrom(src => src.AppointmentDate))
                .ForMember(dest => dest.Doctor,
                    opt => opt.MapFrom(src => src.Doctor));

            // CreateAppointmentRequest → Appointment
            // Id is DB-generated and navigation properties are loaded by EF.
            CreateMap<CreateAppointmentRequest, Appointment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore())
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.AppointmentDate,
                    opt => opt.MapFrom(src =>
                        src.AppointmentDate.HasValue
                            ? src.AppointmentDate.Value
                            : DateTime.Now));
        }
    }
}