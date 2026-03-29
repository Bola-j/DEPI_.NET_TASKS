using Library_System_Web_API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_System_Web_API.Data
{
    public class LibraryDBContext : DbContext
    {
        public DbSet<Entities.Author> Authors { get; set; }
        public DbSet<Entities.Book> Books { get; set; }
        public DbSet<Entities.Borrower> Borrowers { get; set; }
        public DbSet<Entities.Loan> Loans { get; set; }

        public LibraryDBContext(DbContextOptions<LibraryDBContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Fallback connection string (for development/testing)
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LibraryDB;Trusted_Connection=True;");
            }
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
            modelBuilder.Entity<Loan>()
                    .HasOne(l => l.Book)
                    .WithMany(b => b.Loans)
                    .HasForeignKey(l => l.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Loan>()
                    .HasOne(l => l.Borrower)
                    .WithMany(b => b.Loans)
                    .HasForeignKey(l => l.BorrowerId)
                    .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
