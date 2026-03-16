using Library_System.Data;
using Library_System.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library_System
{
    internal class Program
    {
        static void Main(string[] args)
        {

            using var context = new LibraryDBContextFactory().CreateDbContext(args);

            #region Create and Seed Database
            //// Ensure database is created
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //// ----------------------------
            //// 1️ Add Multiple Authors
            //// ----------------------------
            //var authors = new List<Author>
            //{
            //    new Author("J.K. Rowling", DateOnly.Parse("1965-07-31")),
            //    new Author("George R.R. Martin", DateOnly.Parse("1948-09-20")),
            //    new Author("J.R.R. Tolkien", DateOnly.Parse("1892-01-03"))
            //};
            //context.Authors.AddRange(authors);
            //context.SaveChanges();

            ////----------------------------
            ////2️ Add Multiple Books
            ////----------------------------
            //var books = new List<Book>
            //{
            //    new Book("Harry Potter 1", "HP1", authors[0] ),
            //    new Book("Harry Potter 2", "HP2", authors[0] ),
            //    new Book("A Game of Thrones", "GOT1", authors[1] ),
            //    new Book("The Hobbit", "HOB1", authors[2] )
            //};
            //context.Books.AddRange(books);
            //context.SaveChanges();

            //// ----------------------------
            //// 3️ Add Multiple Borrowers
            //// ----------------------------
            //var borrowers = new List<Borrower>
            //{
            //    new Borrower("Alice", DateOnly.FromDateTime(DateTime.Now.AddDays(-124))),
            //    new Borrower("Bob", DateOnly.FromDateTime(DateTime.Now.AddDays(-50))),
            //    new Borrower("Charlie", DateOnly.FromDateTime(DateTime.Now.AddDays(-30)))
            //};
            //context.Borrowers.AddRange(borrowers);
            //context.SaveChanges();

            //// ----------------------------
            //// 4️ Loan Books to Borrowers
            //// ----------------------------
            //var loans = new List<Loan>
            //{
            //    new Loan { BookId = books[0].Id, BorrowerId = borrowers[0].Id, LoanDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-3)) , ReturnDate = DateOnly.FromDateTime(DateTime.Now).AddDays(14)},
            //    new Loan { BookId = books[1].Id, BorrowerId = borrowers[1].Id, LoanDate = DateOnly.FromDateTime(DateTime.Now) , ReturnDate = DateOnly.FromDateTime(DateTime.Now).AddDays(14)},
            //    new Loan { BookId = books[2].Id, BorrowerId = borrowers[2].Id, LoanDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) , ReturnDate = DateOnly.FromDateTime(DateTime.Now).AddDays(14)},
            //    new Loan { BookId = books[3].Id, BorrowerId = borrowers[0].Id, LoanDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-12)) , ReturnDate = DateOnly.FromDateTime(DateTime.Now).AddDays(14)},
            //};
            //context.Loans.AddRange(loans);
            //context.SaveChanges();
            #endregion



            //// ----------------------------
            //// 5️ Complex CRUD Operations
            //// ----------------------------
            #region Update
            //// a) Update all books by "J.K. Rowling" to add " - Special Edition" in title
            //var rowlingBooks = context.Books
            //    .Where(b => b.Author.Name == "J.K. Rowling")
            //    .ToList();

            //rowlingBooks.ForEach(b => b.Title += " - Special Edition");
            //context.SaveChanges();

            //foreach (var book in context.Books.Where(b => b.Author.Name == "J.K. Rowling").ToList())
            //{

            //        Console.WriteLine(book.Title);
            //}
            #endregion

            #region Read & Query
            //// b) Query: Get all borrowers who have borrowed books by "J.R.R. Tolkien"
            //var tolkienBorrowers = context.Loans
            //    .Include(l => l.Book)
            //    .Include(l => l.Borrower)
            //    .Where(l => l.Book.Author.Name == "J.R.R. Tolkien")
            //    .Select(l => l.Borrower.Name)
            //    .Distinct()
            //    .ToList();

            //Console.WriteLine("Borrowers who borrowed Tolkien books:");
            //tolkienBorrowers.ForEach(Console.WriteLine);
            #endregion


            #region Delete
            //// c) Delete all loans older than today - example cleanup
            //var oldLoans = context.Loans
            //    .Where(l => l.LoanDate < DateOnly.FromDateTime(DateTime.Now))
            //    .ToList();
            //context.Loans.RemoveRange(oldLoans);
            //context.SaveChanges();


            //// d) Delete an author and cascade delete their books
            //var authorToDelete = context.Authors.FirstOrDefault(a => a.Name == "George R.R. Martin");
            //if (authorToDelete != null)
            //{
            //    context.Authors.Remove(authorToDelete);
            //    context.SaveChanges();
            //}

            
            ////show remaining books and their authors and borrowers after deletion
            


            //var booksWithDetails = context.Books
            //    .Include(b => b.Author)
            //    .Include(b => b.Borrowers)
            //    .ToList();

            //foreach (var b in booksWithDetails)
            //{
            //    Console.WriteLine($"{b.Title} by {b.Author.Name}, Borrowers: {string.Join(", ", b.Borrowers.Select(br => br.Name))}");
            //}

            #endregion

        }
    }
    }
