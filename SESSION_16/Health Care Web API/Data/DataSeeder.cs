using Health_Care_Web_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Health_Care_Web_API.Data
{
    public class DataSeeder
    {
        private readonly HEALTH_CARE_SYSTEM_DBContext _context;
        private readonly ILogger<DataSeeder> _logger;

        // Base path where the SeedData JSON files are located (resolved at runtime)
        private static readonly string SeedDataPath = Path.Combine(
            AppContext.BaseDirectory, "Data", "SeedData");

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public DataSeeder(HEALTH_CARE_SYSTEM_DBContext context, ILogger<DataSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Entry point: run all seeders in dependency order
        public async Task SeedAsync()
        {
            await SeedDoctorsAsync();
            await SeedPatientsAsync();
            await SeedAppointmentsAsync();
        }

        // ─── Doctors ────────────────────────────────────────────────────────────

        private async Task SeedDoctorsAsync()
        {
            if (await _context.Doctors.AnyAsync())
            {
                _logger.LogInformation("Doctors table already has data — skipping seed.");
                return;
            }

            var json = await ReadJsonFileAsync("doctors.json");
            var doctors = JsonSerializer.Deserialize<List<Doctor>>(json, JsonOptions);

            if (doctors is null || doctors.Count == 0)
            {
                _logger.LogWarning("doctors.json is empty or could not be deserialized.");
                return;
            }

            foreach (var doctor in doctors)
            {
                doctor.Id = 0; 
            }

            await _context.Doctors.AddRangeAsync(doctors);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} doctors.", doctors.Count);
        }

        // ─── Patients ───────────────────────────────────────────────────────────

        private async Task SeedPatientsAsync()
        {
            if (await _context.Patients.AnyAsync())
            {
                _logger.LogInformation("Patients table already has data — skipping seed.");
                return;
            }

            var json = await ReadJsonFileAsync("patients.json");
            var patients = JsonSerializer.Deserialize<List<Patient>>(json, JsonOptions);

            if (patients is null || patients.Count == 0)
            {
                _logger.LogWarning("patients.json is empty or could not be deserialized.");
                return;
            }

            foreach (var patient in patients)
            {
                patient.Id = 0; 
            }

            await _context.Patients.AddRangeAsync(patients);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} patients.", patients.Count);
        }

        // ─── Appointments ────────────────────────────────────────────────────────

        private async Task SeedAppointmentsAsync()
        {
            if (await _context.Appointments.AnyAsync())
            {
                _logger.LogInformation("Appointments table already has data — skipping seed.");
                return;
            }

            var json = await ReadJsonFileAsync("appointments.json");
            var appointments = JsonSerializer.Deserialize<List<Appointment>>(json, JsonOptions);

            if (appointments is null || appointments.Count == 0)
            {
                _logger.LogWarning("appointments.json is empty or could not be deserialized.");
                return;
            }

            //Load real IDs from DB
            var doctorIds = await _context.Doctors.Select(d => d.Id).ToListAsync();
            var patientIds = await _context.Patients.Select(p => p.Id).ToListAsync();

            if (!doctorIds.Any() || !patientIds.Any())
                throw new Exception("Doctors or Patients not seeded properly.");

            var random = new Random();

            foreach (var appointment in appointments)
            {
                appointment.Id = 0;

                //Assign VALID IDs dynamically
                appointment.DoctorId = doctorIds[random.Next(doctorIds.Count)];
                appointment.PatientId = patientIds[random.Next(patientIds.Count)];

                appointment.Doctor = null;
                appointment.Patient = null;
            }

            _context.ChangeTracker.Clear();

            await _context.Appointments.AddRangeAsync(appointments);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} appointments.", appointments.Count);
        }

        // ─── Helper ─────────────────────────────────────────────────────────────

        private static async Task<string> ReadJsonFileAsync(string fileName)
        {
            var filePath = Path.Combine(SeedDataPath, fileName);


            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Seed data file not found: {filePath}");

            return await File.ReadAllTextAsync(filePath);
        }
    }
}
