using Library_System.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_System.Data
{
    internal class LibraryDBContext : DbContext
    {
        public DbSet<Entities.Author> Authors { get; set; }
        public DbSet<Entities.Book> Books { get; set; }
        public DbSet<Entities.Borrower> Borrowers { get; set; }
        public DbSet<Entities.Loan> Loans { get; set; }

        public LibraryDBContext(DbContextOptions<LibraryDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -----------------------------
            // Author -> Book (One-to-Many)
            // -----------------------------
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);


            // -------------------------------------------------
            // Book <-> Borrower (Many-to-Many using Loan)
            // -------------------------------------------------
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Borrowers)
                .WithMany(br => br.BorrowedBooks)
                .UsingEntity<Loan>(
                    j => j
                        .HasOne(l => l.Borrower)
                        .WithMany()
                        .HasForeignKey(l => l.BorrowerId),

                    j => j
                        .HasOne(l => l.Book)
                        .WithMany()
                        .HasForeignKey(l => l.BookId),

                    j =>
                    {
                        j.HasKey(l => new { l.BookId, l.BorrowerId });
                    });
        }
    }
}
