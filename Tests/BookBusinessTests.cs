using Business;
using Data;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class BookBusinessTests
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

                context.SaveChanges();

                BookBusiness bookBusiness = new BookBusiness(context);
                List<Book> books = await bookBusiness.GetAllAsync();

                Assert.That(books.Count, Is.EqualTo(2));
            }
        }

        [Test]
        public async Task GetAllWithIncludesTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Authors.Add(new Author { Id = 1, FirstName = "Author", LastName = "1", Biography = "abcdefg", DateOfBirth = DateTime.Now.AddYears(-1), ImageUrl = "randomUrl" });
                context.Authors.Add(new Author { Id = 2, FirstName = "Author", LastName = "2", Biography = "gfedcba", DateOfBirth = DateTime.Now.AddYears(-2), ImageUrl = "randomUrl" });

                context.Books.Add(new Book { Id = 1, Title = "Book 1", AuthorID = 1, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-1), ISBN = "0900000001" });
                context.Books.Add(new Book { Id = 2, Title = "Book 2", AuthorID = 2, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-2), ISBN = "0900000002" });

                context.SaveChanges();
                BookBusiness bookBusiness = new BookBusiness(context);
                List<Book> books = await bookBusiness.GetAllWithIncludesAsync();
                Assert.That(books.Count, Is.EqualTo(2));
                Assert.That(books[0].Author, Is.Not.Null);
                Assert.That(books[0].BorrowedBooks, Is.Not.Null);
            }
        }
        
        [Test]
        public async Task GetAllByGenreTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Books.Add(new Book { Id = 1, Title = "Book 1", AuthorID = 1, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-1), ISBN = "0900000001" });
                context.Books.Add(new Book { Id = 2, Title = "Book 2", AuthorID = 2, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-2), ISBN = "0900000002" });
                context.Books.Add(new Book { Id = 3, Title = "Book 3", AuthorID = 3, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-3), ISBN = "0900000003" });

                context.SaveChanges();

                BookBusiness bookBusiness = new BookBusiness(context);
                List<Book> booksFiction = await bookBusiness.GetAllByGenreAsync("Fiction");
                List<Book> booksNonFiction = await bookBusiness.GetAllByGenreAsync("NonFiction");

                Assert.That(booksFiction.Count, Is.EqualTo(1));
                Assert.That(booksFiction[0].Genre, Is.EqualTo(Genre.Fiction));

                Assert.That(booksNonFiction.Count, Is.EqualTo(2));
                Assert.That(booksNonFiction[0].Genre, Is.EqualTo(Genre.NonFiction));
            }
        }

        [Test]
        public async Task GetAllByGenreWithIncludesTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Authors.Add(new Author { Id = 1, FirstName = "Author", LastName = "1", Biography = "abcdefg", DateOfBirth = DateTime.Now.AddYears(-1), ImageUrl = "randomUrl" });
                context.Authors.Add(new Author { Id = 2, FirstName = "Author", LastName = "2", Biography = "gfedcba", DateOfBirth = DateTime.Now.AddYears(-2), ImageUrl = "randomUrl" });
                context.Authors.Add(new Author { Id = 3, FirstName = "Author", LastName = "3", Biography = "gfedcba", DateOfBirth = DateTime.Now.AddYears(-3), ImageUrl = "randomUrl" });

                context.Books.Add(new Book { Id = 1, Title = "Book 1", AuthorID = 1, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-1), ISBN = "0900000001" });
                context.Books.Add(new Book { Id = 2, Title = "Book 2", AuthorID = 2, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-2), ISBN = "0900000002" });
                context.Books.Add(new Book { Id = 3, Title = "Book 3", AuthorID = 3, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-3), ISBN = "0900000003" });

                context.SaveChanges();

                BookBusiness bookBusiness = new BookBusiness(context);
                List<Book> booksFiction = await bookBusiness.GetAllByGenreWithIncludesAsync("Fiction");
                List<Book> booksNonFiction = await bookBusiness.GetAllByGenreWithIncludesAsync("NonFiction");

                Assert.That(booksFiction.Count, Is.EqualTo(1));
                Assert.That(booksFiction[0].Genre, Is.EqualTo(Genre.Fiction));

                Assert.That(booksNonFiction.Count, Is.EqualTo(2));
                Assert.That(booksNonFiction[0].Genre, Is.EqualTo(Genre.NonFiction));

                Assert.That(booksFiction[0].Author, Is.Not.Null);
                Assert.That(booksNonFiction[0].Author, Is.Not.Null);
                Assert.That(booksNonFiction[1].Author, Is.Not.Null);
            }
        }        

        [Test]
        public async Task GetTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Books.Add(new Book { Id = 1, Title = "Book 1", AuthorID = 1, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-1), ISBN = "0900000001" });
                context.Books.Add(new Book { Id = 2, Title = "Book 2", AuthorID = 2, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-2), ISBN = "0900000002" });

                context.SaveChanges();

                BookBusiness bookBusiness = new BookBusiness(context);
                Book book = await bookBusiness.GetAsync(1);

                Assert.That(book, Is.Not.Null);
                Assert.That(book.Id, Is.EqualTo(1));
                Assert.That(book, Is.EqualTo(context.Books.Find(1)));
            }
        }

        [Test]
        public async Task GetWithIncludesTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Authors.Add(new Author { Id = 1, FirstName = "Author", LastName = "1", Biography = "abcdefg", DateOfBirth = DateTime.Now.AddYears(-1), ImageUrl = "randomUrl" });
                context.Authors.Add(new Author { Id = 2, FirstName = "Author", LastName = "2", Biography = "gfedcba", DateOfBirth = DateTime.Now.AddYears(-2), ImageUrl = "randomUrl" });

                context.Books.Add(new Book { Id = 1, Title = "Book 1", AuthorID = 1, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-1), ISBN = "0900000001" });
                context.Books.Add(new Book { Id = 2, Title = "Book 2", AuthorID = 2, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-2), ISBN = "0900000002" });

                context.SaveChanges();

                BookBusiness bookBusiness = new BookBusiness(context);
                Book book = await bookBusiness.GetWithIncludesAsync(1);

                Assert.That(book, Is.Not.Null);
                Assert.That(book.Id, Is.EqualTo(1));
                Assert.That(book, Is.EqualTo(context.Books.Find(1)));

                Assert.That(book.Author, Is.Not.Null);
                Assert.That(book.BorrowedBooks, Is.Not.Null);

                Assert.That(book.Author, Is.EqualTo(context.Books.Include(b => b.Author).Include(b => b.BorrowedBooks).First(x => x.Id == 1).Author));
            }
        }

        [Test]
        public async Task GetByISBNTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Books.Add(new Book { Id = 1, Title = "Book 1", AuthorID = 1, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-1), ISBN = "0900000001" });
                context.Books.Add(new Book { Id = 2, Title = "Book 2", AuthorID = 2, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-2), ISBN = "0900000002" });

                context.SaveChanges();

                BookBusiness bookBusiness = new BookBusiness(context);
                Book book = await bookBusiness.GetByISBNAsync("0900000001");

                Assert.That(book, Is.Not.Null);
                Assert.That(book.Id, Is.EqualTo(1));
                Assert.That(book.ISBN, Is.EqualTo("0900000001"));
                Assert.That(book, Is.EqualTo(context.Books.First(x => x.ISBN == "0900000001")));
            }
        }

        [Test]
        public async Task GetByISBNWithIncludesTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Authors.Add(new Author { Id = 1, FirstName = "Author", LastName = "1", Biography = "abcdefg", DateOfBirth = DateTime.Now.AddYears(-1), ImageUrl = "randomUrl" });
                context.Authors.Add(new Author { Id = 2, FirstName = "Author", LastName = "2", Biography = "gfedcba", DateOfBirth = DateTime.Now.AddYears(-2), ImageUrl = "randomUrl" });

                context.Books.Add(new Book { Id = 1, Title = "Book 1", AuthorID = 1, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-1), ISBN = "0900000001" });
                context.Books.Add(new Book { Id = 2, Title = "Book 2", AuthorID = 2, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-2), ISBN = "0900000002" });

                context.SaveChanges();

                BookBusiness bookBusiness = new BookBusiness(context);
                Book book = await bookBusiness.GetByISBNWithIncludesAsync("0900000001");

                Assert.That(book, Is.Not.Null);
                Assert.That(book.Id, Is.EqualTo(1));
                Assert.That(book.ISBN, Is.EqualTo("0900000001"));
                Assert.That(book, Is.EqualTo(context.Books.First(x => x.ISBN == "0900000001")));

                Assert.That(book.Author, Is.Not.Null);
                Assert.That(book.BorrowedBooks, Is.Not.Null);

                Assert.That(book.Author, Is.EqualTo(context.Books.Include(b => b.Author).Include(b => b.BorrowedBooks).First(x => x.ISBN == "0900000001").Author));
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

                context.SaveChanges();

                BookBusiness bookBusiness = new BookBusiness(context);
                Book book = new Book { Id = 3, Title = "Book 3", AuthorID = 3, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-3), ISBN = "0900000003" };
                await bookBusiness.AddAsync(book);

                Assert.That(context.Books, Has.Exactly(1).EqualTo(book));
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

                context.SaveChanges();

                BookBusiness bookBusiness = new BookBusiness(context);
                Book oldBook = new Book { Id = 2, Title = "Book 2", AuthorID = 2, Genre = Genre.NonFiction, PublicationDate = DateTime.Now.AddYears(-2), ISBN = "0900000002" };
                Book book = new Book { Id = 2, Title = "Book 2.1", AuthorID = 1, Genre = Genre.Fiction, PublicationDate = DateTime.Now.AddYears(-3), ISBN = "0900000011" };
                await bookBusiness.UpdateAsync(book);

                Assert.That(context.Books, Has.Exactly(0).EqualTo(oldBook));

                Assert.That(context.Books.Count(), Is.EqualTo(2));

                Assert.That(context.Books.First(x => x.Id == 2).Title, Is.EqualTo("Book 2.1"));
                Assert.That(context.Books.First(x => x.Id == 2).PublicationDate, Is.EqualTo(context.Books.Find(2).PublicationDate));
                Assert.That(context.Books.First(x => x.Id == 2).ISBN, Is.EqualTo("0900000011"));
                Assert.That(context.Books.First(x => x.Id == 2).Genre, Is.EqualTo(Genre.Fiction));
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

                context.SaveChanges();

                BookBusiness bookBusiness = new BookBusiness(context);
                await bookBusiness.DeleteAsync(2);

                Assert.That(context.Books, Has.Exactly(0).EqualTo(context.Books.Find(2)));

                Assert.That(context.Books.Count(), Is.EqualTo(1));
            }
        }
    }
}
