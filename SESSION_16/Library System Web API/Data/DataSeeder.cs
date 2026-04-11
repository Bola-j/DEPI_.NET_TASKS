using Library_System_Web_API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Library_System_Web_API.Data
{
    public class DataSeeder
    {
        private readonly LibraryDBContext _context;
        private readonly ILogger<DataSeeder> _logger;

        // Base path where the SeedData JSON files are located (resolved at runtime)
        private static readonly string SeedDataPath = Path.Combine(
            AppContext.BaseDirectory, "Data", "SeedData");

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public DataSeeder(LibraryDBContext context, ILogger<DataSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Entry point: run all seeders in dependency order
        public async Task SeedAsync()
        {
            await SeedAuthorsAsync();
            await SeedBorrowersAsync();
            await SeedBooksAsync();
            await SeedLoansAsync();
        }

        // ─── Authors ─────────────────────────────────────────────────────────────

        private async Task SeedAuthorsAsync()
        {
            if (await _context.Authors.AnyAsync())
            {
                _logger.LogInformation("Authors table already has data — skipping seed.");
                return;
            }

            var json = await ReadJsonFileAsync("authors.json");
            var authors = JsonSerializer.Deserialize<List<Author>>(json, JsonOptions);

            if (authors is null || authors.Count == 0)
            {
                _logger.LogWarning("authors.json is empty or could not be deserialized.");
                return;
            }

            foreach (var author in authors)
            {
                author.Id = 0; // Let the DB generate the PK
            }

            await _context.Authors.AddRangeAsync(authors);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} authors.", authors.Count);
        }

        // ─── Borrowers ───────────────────────────────────────────────────────────

        private async Task SeedBorrowersAsync()
        {
            if (await _context.Borrowers.AnyAsync())
            {
                _logger.LogInformation("Borrowers table already has data — skipping seed.");
                return;
            }

            var json = await ReadJsonFileAsync("borrowers.json");
            var borrowers = JsonSerializer.Deserialize<List<Borrower>>(json, JsonOptions);

            if (borrowers is null || borrowers.Count == 0)
            {
                _logger.LogWarning("borrowers.json is empty or could not be deserialized.");
                return;
            }

            foreach (var borrower in borrowers)
            {
                borrower.Id = 0; // Let the DB generate the PK
            }

            await _context.Borrowers.AddRangeAsync(borrowers);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} borrowers.", borrowers.Count);
        }

        // ─── Books ───────────────────────────────────────────────────────────────

        private async Task SeedBooksAsync()
        {
            if (await _context.Books.AnyAsync())
            {
                _logger.LogInformation("Books table already has data — skipping seed.");
                return;
            }

            var json = await ReadJsonFileAsync("books.json");

            // Deserialize into a plain DTO first so we can read the positional AuthorId
            var bookDtos = JsonSerializer.Deserialize<List<BookSeedDto>>(json, JsonOptions);

            if (bookDtos is null || bookDtos.Count == 0)
            {
                _logger.LogWarning("books.json is empty or could not be deserialized.");
                return;
            }

            // Load real Author IDs from the DB (ordered so index 1 = first seeded author)
            var authorIds = await _context.Authors
                .OrderBy(a => a.Id)
                .Select(a => a.Id)
                .ToListAsync();

            if (!authorIds.Any())
                throw new Exception("Authors not seeded properly.");

            var books = bookDtos.Select(dto =>
            {
                // dto.AuthorId is 1-based position in authors.json
                var resolvedAuthorId = authorIds[Math.Clamp(dto.AuthorId - 1, 0, authorIds.Count - 1)];

                return new Book
                {
                    Id = 0, // Let the DB generate the PK
                    Title = dto.Title,
                    ISBN = dto.ISBN,
                    AuthorId = resolvedAuthorId,
                    Author = null
                };
            }).ToList();

            await _context.Books.AddRangeAsync(books);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} books.", books.Count);
        }

        // ─── Loans ───────────────────────────────────────────────────────────────

        private async Task SeedLoansAsync()
        {
            if (await _context.Loans.AnyAsync())
            {
                _logger.LogInformation("Loans table already has data — skipping seed.");
                return;
            }

            var json = await ReadJsonFileAsync("loans.json");
            var loanDtos = JsonSerializer.Deserialize<List<LoanSeedDto>>(json, JsonOptions);

            if (loanDtos is null || loanDtos.Count == 0)
            {
                _logger.LogWarning("loans.json is empty or could not be deserialized.");
                return;
            }

            // Load real IDs from the DB (ordered so index 1 = first seeded record)
            var bookIds = await _context.Books
                .OrderBy(b => b.Id)
                .Select(b => b.Id)
                .ToListAsync();

            var borrowerIds = await _context.Borrowers
                .OrderBy(b => b.Id)
                .Select(b => b.Id)
                .ToListAsync();

            if (!bookIds.Any() || !borrowerIds.Any())
                throw new Exception("Books or Borrowers not seeded properly.");

            var loans = loanDtos.Select(dto => new Loan
            {
                Id = 0, // Let the DB generate the PK
                BookId = bookIds[Math.Clamp(dto.BookId - 1, 0, bookIds.Count - 1)],
                BorrowerId = borrowerIds[Math.Clamp(dto.BorrowerId - 1, 0, borrowerIds.Count - 1)],
                LoanDate = dto.LoanDate,
                ReturnDate = dto.ReturnDate,
                Book = null,
                Borrower = null
            }).ToList();

            _context.ChangeTracker.Clear();

            await _context.Loans.AddRangeAsync(loans);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} loans.", loans.Count);
        }

        // ─── Seed DTOs ───────────────────────────────────────────────────────────

        // Lightweight DTOs used only during seeding to carry positional FK values
        // from the JSON files before they are resolved to real DB-generated IDs.

        private sealed class BookSeedDto
        {
            public string Title { get; set; } = string.Empty;
            public string ISBN { get; set; } = string.Empty;
            public int AuthorId { get; set; }   // 1-based position in authors.json
        }

        private sealed class LoanSeedDto
        {
            public int BookId { get; set; }   // 1-based position in books.json
            public int BorrowerId { get; set; }   // 1-based position in borrowers.json
            public DateOnly LoanDate { get; set; }
            public DateOnly? ReturnDate { get; set; }
        }

        // ─── Helper ──────────────────────────────────────────────────────────────

        private static async Task<string> ReadJsonFileAsync(string fileName)
        {
            var filePath = Path.Combine(SeedDataPath, fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Seed data file not found: {filePath}");

            return await File.ReadAllTextAsync(filePath);
        }
    }
}