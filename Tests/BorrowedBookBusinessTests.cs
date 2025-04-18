using Business;
using Data.Models;
using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Enums;
using System.Net;

namespace Tests
{
    internal class BorrowedBookBusinessTests
    {
        [Test]
        public async Task GetAllTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Books.Add(new Book { Id = 1, Title = "Book 1", AuthorID = 1, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-1), ISBN = "0900000001" });
                context.Books.Add(new Book { Id = 2, Title = "Book 2", AuthorID = 2, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-2), ISBN = "0900000002" });

                context.Members.Add(new Member { Id = 1, FirstName = "Member", LastName = "1", PhoneNumber = "1234567890", MembershipExpireDate = DateTime.Now.AddYears(1) });
                context.Members.Add(new Member { Id = 2, FirstName = "Member", LastName = "2", PhoneNumber = "0987654321", MembershipExpireDate = DateTime.Now.AddYears(1 / 2) });

                context.BorrowedBooks.Add(new BorrowedBook { BookID = 1, MemberID = 1, BorrowDate = DateTime.Now.AddDays(-10), ReturnDate = null, DueDate = DateTime.Now.AddMonths(-3) });
                context.BorrowedBooks.Add(new BorrowedBook { BookID = 2, MemberID = 1, BorrowDate = DateTime.Now.AddDays(-10), ReturnDate = DateTime.Now });

                context.SaveChanges();

                BorrowedBookBusiness borrowedBookBusiness = new BorrowedBookBusiness(context);
                List<BorrowedBook> borrowedBooks = await borrowedBookBusiness.GetAllAsync();

                Assert.That(borrowedBooks.Count, Is.EqualTo(2));
            }
        }

        [Test]
        public async Task GetByBookIdTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Books.Add(new Book { Id = 1, Title = "Book 1", AuthorID = 1, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-1), ISBN = "0900000001" });
                context.Books.Add(new Book { Id = 2, Title = "Book 2", AuthorID = 2, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-2), ISBN = "0900000002" });

                context.Members.Add(new Member { Id = 1, FirstName = "Member", LastName = "1", PhoneNumber = "1234567890", MembershipExpireDate = DateTime.Now.AddYears(1) });
                context.Members.Add(new Member { Id = 2, FirstName = "Member", LastName = "2", PhoneNumber = "0987654321", MembershipExpireDate = DateTime.Now.AddYears(1 / 2) });

                context.BorrowedBooks.Add(new BorrowedBook { BookID = 1, MemberID = 1, BorrowDate = DateTime.Now.AddDays(-10), ReturnDate = null, DueDate = DateTime.Now.AddMonths(-3) });
                context.BorrowedBooks.Add(new BorrowedBook { BookID = 2, MemberID = 1, BorrowDate = DateTime.Now.AddDays(-10), ReturnDate = DateTime.Now });

                context.SaveChanges();

                BorrowedBookBusiness borrowedBookBusiness = new BorrowedBookBusiness(context);
                var borrowedBook = await borrowedBookBusiness.GetByBookIdAsync(1);

                Assert.That(borrowedBook.BookID, Is.EqualTo(1));
                Assert.That(borrowedBook, Is.EqualTo(context.BorrowedBooks.Include(bb => bb.Book).Include(bb => bb.Member).OrderBy(bb => bb.BorrowDate).Reverse().First(bb => bb.BookID == 1)));

                Assert.That(borrowedBook.Member, Is.EqualTo(context.BorrowedBooks.Include(bb => bb.Book).Include(bb => bb.Member).OrderBy(bb => bb.BorrowDate).Reverse().First(bb => bb.BookID == 1).Member));
                Assert.That(borrowedBook.Book, Is.EqualTo(context.BorrowedBooks.Include(bb => bb.Book).Include(bb => bb.Member).OrderBy(bb => bb.BorrowDate).Reverse().First(bb => bb.BookID == 1).Book));
            }
        }

        [Test]
        public async Task GetAllBetweenDatesTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Books.Add(new Book { Id = 1, Title = "Book 1", AuthorID = 1, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-1), ISBN = "0900000001" });
                context.Books.Add(new Book { Id = 2, Title = "Book 2", AuthorID = 2, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-2), ISBN = "0900000002" });

                context.Members.Add(new Member { Id = 1, FirstName = "Member", LastName = "1", PhoneNumber = "1234567890", MembershipExpireDate = DateTime.Now.AddYears(1) });
                context.Members.Add(new Member { Id = 2, FirstName = "Member", LastName = "2", PhoneNumber = "0987654321", MembershipExpireDate = DateTime.Now.AddYears(1 / 2) });

                context.BorrowedBooks.Add(new BorrowedBook { BookID = 1, MemberID = 1, BorrowDate = DateTime.Now.AddDays(-10), ReturnDate = null, DueDate = DateTime.Now.AddMonths(-3) });
                context.BorrowedBooks.Add(new BorrowedBook { BookID = 2, MemberID = 1, BorrowDate = DateTime.Now.AddYears(-2), ReturnDate = DateTime.Now });

                context.SaveChanges();

                BorrowedBookBusiness borrowedBookBusiness = new BorrowedBookBusiness(context);
                List<BorrowedBook> borrowedBooks = await borrowedBookBusiness.GetAllBetweenDatesAsync(DateTime.Now.AddYears(-1), DateTime.Now);

                Assert.That(borrowedBooks.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task GetByMemberIdTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Books.Add(new Book { Id = 1, Title = "Book 1", AuthorID = 1, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-1), ISBN = "0900000001" });
                context.Books.Add(new Book { Id = 2, Title = "Book 2", AuthorID = 2, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-2), ISBN = "0900000002" });

                context.Members.Add(new Member { Id = 1, FirstName = "Member", LastName = "1", PhoneNumber = "1234567890", MembershipExpireDate = DateTime.Now.AddYears(1) });
                context.Members.Add(new Member { Id = 2, FirstName = "Member", LastName = "2", PhoneNumber = "0987654321", MembershipExpireDate = DateTime.Now.AddYears(1 / 2) });

                context.BorrowedBooks.Add(new BorrowedBook { BookID = 1, MemberID = 1, BorrowDate = DateTime.Now.AddDays(-10), ReturnDate = null, DueDate = DateTime.Now.AddMonths(-3) });
                context.BorrowedBooks.Add(new BorrowedBook { BookID = 2, MemberID = 1, BorrowDate = DateTime.Now.AddYears(-2), ReturnDate = DateTime.Now });

                context.SaveChanges();

                BorrowedBookBusiness borrowedBookBusiness = new BorrowedBookBusiness(context);
                var borrowedBook = await borrowedBookBusiness.GetByMemberIdAsync(1);

                Assert.That(borrowedBook.MemberID, Is.EqualTo(1));
                Assert.That(borrowedBook, Is.EqualTo(context.BorrowedBooks.Include(bb => bb.Book).Include(bb => bb.Member).OrderBy(bb => bb.BorrowDate).Reverse().First(bb => bb.MemberID == 1)));
            }
        }

        [Test]
        public async Task GetByBookIdAndDateTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Books.Add(new Book { Id = 1, Title = "Book 1", AuthorID = 1, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-1), ISBN = "0900000001" });
                context.Books.Add(new Book { Id = 2, Title = "Book 2", AuthorID = 2, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-2), ISBN = "0900000002" });

                context.Members.Add(new Member { Id = 1, FirstName = "Member", LastName = "1", PhoneNumber = "1234567890", MembershipExpireDate = DateTime.Now.AddYears(1) });
                context.Members.Add(new Member { Id = 2, FirstName = "Member", LastName = "2", PhoneNumber = "0987654321", MembershipExpireDate = DateTime.Now.AddYears(1 / 2) });

                DateTime borrowDate = DateTime.Now;

                context.BorrowedBooks.Add(new BorrowedBook { BookID = 1, MemberID = 1, BorrowDate = borrowDate, ReturnDate = null, DueDate = DateTime.Now.AddMonths(-3) });
                context.BorrowedBooks.Add(new BorrowedBook { BookID = 2, MemberID = 1, BorrowDate = borrowDate, ReturnDate = DateTime.Now });

                context.SaveChanges();

                BorrowedBookBusiness borrowedBookBusiness = new BorrowedBookBusiness(context);
                BorrowedBook borrowedBook = await borrowedBookBusiness.GetByBookIdAndDateAsync(1, borrowDate);

                Assert.That(borrowedBook.BookID, Is.EqualTo(1));
                Assert.That(borrowedBook, Is.EqualTo(context.BorrowedBooks.Include(bb => bb.Book).Include(bb => bb.Member).OrderBy(bb => bb.BorrowDate).Reverse().First(bb => bb.BookID == 1 && bb.BorrowDate == borrowDate)));
            }
        }

        [Test]
        public async Task AddTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Books.Add(new Book { Id = 1, Title = "Book 1", AuthorID = 1, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-1), ISBN = "0900000001" });
                context.Books.Add(new Book { Id = 2, Title = "Book 2", AuthorID = 2, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-2), ISBN = "0900000002" });

                context.Members.Add(new Member { Id = 1, FirstName = "Member", LastName = "1", PhoneNumber = "1234567890", MembershipExpireDate = DateTime.Now.AddYears(1) });
                context.Members.Add(new Member { Id = 2, FirstName = "Member", LastName = "2", PhoneNumber = "0987654321", MembershipExpireDate = DateTime.Now.AddYears(1 / 2) });

                context.BorrowedBooks.Add(new BorrowedBook { BookID = 1, MemberID = 1, BorrowDate = DateTime.Now.AddDays(-10), ReturnDate = null, DueDate = DateTime.Now.AddMonths(-3) });
                context.BorrowedBooks.Add(new BorrowedBook { BookID = 2, MemberID = 1, BorrowDate = DateTime.Now.AddYears(-2), ReturnDate = DateTime.Now });

                context.SaveChanges();

                BorrowedBookBusiness borrowedBookBusiness = new BorrowedBookBusiness(context);
                BorrowedBook borrowedBook = new BorrowedBook
                {
                    BookID = 2,
                    MemberID = 2,
                    BorrowDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14),
                    ReturnDate = null
                };
                await borrowedBookBusiness.AddAsync(borrowedBook);

                Assert.That(context.BorrowedBooks.Count, Is.EqualTo(3));
            }
        }

        [Test]
        public async Task UpdateTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Books.Add(new Book { Id = 1, Title = "Book 1", AuthorID = 1, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-1), ISBN = "0900000001" });
                context.Books.Add(new Book { Id = 2, Title = "Book 2", AuthorID = 2, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-2), ISBN = "0900000002" });

                context.Members.Add(new Member { Id = 1, FirstName = "Member", LastName = "1", PhoneNumber = "1234567890", MembershipExpireDate = DateTime.Now.AddYears(1) });
                context.Members.Add(new Member { Id = 2, FirstName = "Member", LastName = "2", PhoneNumber = "0987654321", MembershipExpireDate = DateTime.Now.AddYears(1 / 2) });

                context.BorrowedBooks.Add(new BorrowedBook { BookID = 1, MemberID = 1, BorrowDate = DateTime.Now.AddDays(-10), ReturnDate = null, DueDate = DateTime.Now.AddMonths(-3) });
                context.BorrowedBooks.Add(new BorrowedBook { BookID = 2, MemberID = 1, BorrowDate = DateTime.Now.AddYears(-2), ReturnDate = DateTime.Now });

                context.SaveChanges();

                BorrowedBookBusiness borrowedBookBusiness = new BorrowedBookBusiness(context);
                BorrowedBook borrowedBook = new BorrowedBook
                {
                    BookID = 1,
                    MemberID = 1,
                    BorrowDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14),
                    ReturnDate = null
                };
                await borrowedBookBusiness.UpdateAsync(borrowedBook);

                Assert.That(borrowedBook.BorrowDate, Is.EqualTo(context.BorrowedBooks.First(x => x.MemberID == 1 && x.BookID == 1).BorrowDate));
                Assert.That(borrowedBook.DueDate, Is.EqualTo(context.BorrowedBooks.First(x => x.MemberID == 1 && x.BookID == 1).DueDate));
                Assert.That(context.BorrowedBooks.Count, Is.EqualTo(2));
            }
        }

        [Test]
        public async Task DeleteTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Books.Add(new Book { Id = 1, Title = "Book 1", AuthorID = 1, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-1), ISBN = "0900000001" });
                context.Books.Add(new Book { Id = 2, Title = "Book 2", AuthorID = 2, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-2), ISBN = "0900000002" });

                context.Members.Add(new Member { Id = 1, FirstName = "Member", LastName = "1", PhoneNumber = "1234567890", MembershipExpireDate = DateTime.Now.AddYears(1) });
                context.Members.Add(new Member { Id = 2, FirstName = "Member", LastName = "2", PhoneNumber = "0987654321", MembershipExpireDate = DateTime.Now.AddYears(1 / 2) });

                context.BorrowedBooks.Add(new BorrowedBook { BookID = 1, MemberID = 1, BorrowDate = DateTime.Now, ReturnDate = null, DueDate = DateTime.Now.AddMonths(-3) });
                context.BorrowedBooks.Add(new BorrowedBook { BookID = 2, MemberID = 1, BorrowDate = DateTime.Now.AddYears(-2), ReturnDate = DateTime.Now });

                context.SaveChanges();

                BorrowedBookBusiness borrowedBookBusiness = new BorrowedBookBusiness(context);
                await borrowedBookBusiness.DeleteAsync(1);

                Assert.That(context.BorrowedBooks.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task DeleteByIdAndDateTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Books.Add(new Book { Id = 1, Title = "Book 1", AuthorID = 1, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-1), ISBN = "0900000001" });
                context.Books.Add(new Book { Id = 2, Title = "Book 2", AuthorID = 2, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-2), ISBN = "0900000002" });

                context.Members.Add(new Member { Id = 1, FirstName = "Member", LastName = "1", PhoneNumber = "1234567890", MembershipExpireDate = DateTime.Now.AddYears(1) });
                context.Members.Add(new Member { Id = 2, FirstName = "Member", LastName = "2", PhoneNumber = "0987654321", MembershipExpireDate = DateTime.Now.AddYears(1 / 2) });

                DateTime borrowDate = DateTime.Now;

                context.BorrowedBooks.Add(new BorrowedBook { BookID = 1, MemberID = 1, BorrowDate = borrowDate, ReturnDate = null, DueDate = DateTime.Now.AddMonths(-3) });
                context.BorrowedBooks.Add(new BorrowedBook { BookID = 2, MemberID = 1, BorrowDate = DateTime.Now.AddYears(-2), ReturnDate = DateTime.Now });

                context.SaveChanges();

                BorrowedBookBusiness borrowedBookBusiness = new BorrowedBookBusiness(context);
                await borrowedBookBusiness.DeleteByIdAndDateAsync(1, borrowDate);

                Assert.That(context.BorrowedBooks.Count, Is.EqualTo(1));
            }
        }
    }
}
