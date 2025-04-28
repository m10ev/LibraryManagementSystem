using Business;
using Data;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Seeders
{
    public class DbSeeder
    {
        private readonly LibraryDbContext _context;
        public DbSeeder(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task Seed()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();

            // Create Lists for seed data
            var authors = new List<Author>
            {
                new Author { FirstName = "George", LastName = "Orwell", DateOfBirth = new DateTime(1903, 6, 25), Biography = "British novelist and essayist", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7e/George_Orwell_press_photo.jpg/1024px-George_Orwell_press_photo.jpg"},
                new Author { FirstName = "Jane", LastName = "Austen", DateOfBirth = new DateTime(1775, 12, 16), Biography = "Renowned English novelist", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/cc/CassandraAusten-JaneAusten%28c.1810%29_hires.jpg/1024px-CassandraAusten-JaneAusten%28c.1810%29_hires.jpg" },
                new Author { FirstName = "J.K.", LastName = "Rowling", DateOfBirth = new DateTime(1965, 7, 31), Biography = "Author of Harry Potter series", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5d/J._K._Rowling_2010.jpg/250px-J._K._Rowling_2010.jpg"},
                new Author { FirstName = "Stephen", LastName = "King", DateOfBirth = new DateTime(1947, 9, 21), Biography = "King of horror and supernatural fiction", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Stephen_King_at_the_2024_Toronto_International_Film_Festival_2_%28cropped%29.jpg/250px-Stephen_King_at_the_2024_Toronto_International_Film_Festival_2_%28cropped%29.jpg" },
                new Author { FirstName = "Mark", LastName = "Twain", DateOfBirth = new DateTime(1835, 11, 30), Biography = "Famous American humorist and writer", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/0/0c/Mark_Twain_by_AF_Bradley.jpg"}
            };

            var members = new List<Member>
            {
                new Member { FirstName = "Alice", LastName = "Johnson", MembershipExpireDate = new DateTime(2026, 5, 1), PhoneNumber = "555-12345678" },
                new Member { FirstName = "Bob", LastName = "Smith", MembershipExpireDate = new DateTime(2026, 4, 15), PhoneNumber = "555-87654321" },
                new Member { FirstName = "Carol", LastName = "Martinez", MembershipExpireDate = new DateTime(2026, 7, 30), PhoneNumber = "555-56789012" },
                new Member { FirstName = "Dave", LastName = "Brown", MembershipExpireDate = new DateTime(2024, 3, 10), PhoneNumber = "555-43210987" }, // EXPIRED
                new Member { FirstName = "Eve", LastName = "Davis", MembershipExpireDate = new DateTime(2026, 9, 20), PhoneNumber = "555-34567890" }
            };

            // Insert Authors and save changes
            await _context.Authors.AddRangeAsync(authors);
            await _context.SaveChangesAsync(); // Save authors to generate IDs

            // Retrieve authors with their IDs
            var authorList = await _context.Authors.ToListAsync();

            // Now that we have the author IDs, create books and link them to the correct author IDs
            var books = new List<Book>
            {
                new Book { Title = "1984", Genre = Genre.ScienceFiction, ISBN = "9780451524935", PublicationDate = new DateTime(1949, 6, 8), AuthorID = 1},
                new Book { Title = "Animal Farm", Genre = Genre.Fiction, ISBN = "9780451526342", PublicationDate = new DateTime(1945, 8, 17), AuthorID = 1},
                new Book { Title = "Pride and Prejudice", Genre = Genre.Romance, ISBN = "9780679783268", PublicationDate = new DateTime(1813, 1, 28), AuthorID = 2},
                new Book { Title = "Sense and Sensibility", Genre = Genre.Romance, ISBN = "9780141439662", PublicationDate = new DateTime(1811, 10, 30), AuthorID = 2},
                new Book { Title = "Harry Potter and the Sorcerer's Stone", Genre = Genre.Fantasy, ISBN = "9780439554930", PublicationDate = new DateTime(1997, 6, 26), AuthorID = 3},
                new Book { Title = "Harry Potter and the Chamber of Secrets", Genre = Genre.Fantasy, ISBN = "9780439064873", PublicationDate = new DateTime(1998, 7, 2), AuthorID = 3},
                new Book { Title = "The Shining", Genre = Genre.Horror, ISBN = "9780385121675", PublicationDate = new DateTime(1977, 1, 28), AuthorID = 4},
                new Book { Title = "It", Genre = Genre.Horror, ISBN = "9780451169518", PublicationDate = new DateTime(1986, 9, 15), AuthorID = 4},
                new Book { Title = "Adventures of Huckleberry Finn", Genre = Genre.HistoricalFiction, ISBN = "9780486280615", PublicationDate = new DateTime(1884, 12, 10), AuthorID = 4},
                new Book { Title = "The Adventures of Tom Sawyer", Genre = Genre.HistoricalFiction, ISBN = "9780486400778", PublicationDate = new DateTime(1876, 6, 1), AuthorID = 4}
            };

            // Insert Members and save changes
            await _context.Members.AddRangeAsync(members);
            await _context.SaveChangesAsync(); // Save members to generate IDs

            // Insert Books and save changes
            await _context.Books.AddRangeAsync(books);
            await _context.SaveChangesAsync(); // Save books to generate BookIDs

            // Create and insert Borrowed Books
            var borrowedBooks = new List<BorrowedBook>
            {
                new BorrowedBook { BookID = books[0].Id, MemberID = members[0].Id, BorrowDate = new DateTime(2025, 4, 1), DueDate = new DateTime(2025, 4, 15), ReturnDate = new DateTime(2025, 4, 10) },
                new BorrowedBook { BookID = books[1].Id, MemberID = members[0].Id, BorrowDate = new DateTime(2025, 4, 20), DueDate = new DateTime(2025, 5, 4), ReturnDate = null },
                new BorrowedBook { BookID = books[2].Id, MemberID = members[1].Id, BorrowDate = new DateTime(2025, 3, 15), DueDate = new DateTime(2025, 3, 29), ReturnDate = new DateTime(2025, 3, 28) },
                new BorrowedBook { BookID = books[3].Id, MemberID = members[1].Id, BorrowDate = new DateTime(2025, 4, 5), DueDate = new DateTime(2025, 4, 19), ReturnDate = null },
                new BorrowedBook { BookID = books[4].Id, MemberID = members[2].Id, BorrowDate = new DateTime(2025, 2, 10), DueDate = new DateTime(2025, 2, 24), ReturnDate = new DateTime(2025, 2, 25) },
                new BorrowedBook { BookID = books[5].Id, MemberID = members[2].Id, BorrowDate = new DateTime(2025, 4, 10), DueDate = new DateTime(2025, 4, 24), ReturnDate = null },
                new BorrowedBook { BookID = books[6].Id, MemberID = members[3].Id, BorrowDate = new DateTime(2025, 3, 1), DueDate = new DateTime(2025, 3, 15), ReturnDate = new DateTime(2025, 3, 12) },
                new BorrowedBook { BookID = books[7].Id, MemberID = members[4].Id, BorrowDate = new DateTime(2025, 3, 20), DueDate = new DateTime(2025, 4, 3), ReturnDate = new DateTime(2025, 4, 1) },
                new BorrowedBook { BookID = books[8].Id, MemberID = members[4].Id, BorrowDate = new DateTime(2025, 4, 18), DueDate = new DateTime(2025, 5, 2), ReturnDate = null }
            };

            // Insert Borrowed Books and save changes
            await _context.BorrowedBooks.AddRangeAsync(borrowedBooks);
            await _context.SaveChangesAsync(); // Save borrowed books
        }
    }
}
