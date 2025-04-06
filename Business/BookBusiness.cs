using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Business
{
    public class BookBusiness
    {
        private LibraryDbContext context;

        public List<Book> GetAll()
        {
            using (context = new LibraryDbContext())
            {
                return context.Books.ToList();
            }
        }

        public List<Book> GetAllWithIncludes()
        {
            using (context = new LibraryDbContext())
            {
                return context.Books
                    .Include(b => b.Author)
                    .Include(b => b.BorrowedBooks)
                    .ToList();
            }
        }

        public List<Book> GetAllByGenre(string genre)
        {
            using (context = new LibraryDbContext())
            {
                return context.Books
                    .Where(b => b.Genre.ToString().ToLower() == genre.ToLower())
                    .ToList();
            }
        }

        public List<Book> GetAllByGenreWithIncludes(string genre)
        {
            using (context = new LibraryDbContext())
            {
                return context.Books
                    .Include(b => b.Author)
                    .Include(b => b.BorrowedBooks)
                    .Where(b => b.Genre.ToString().ToLower() == genre.ToLower())
                    .ToList();
            }
        }

        public Book Get(int id)
        {
            using (context = new LibraryDbContext())
            {
                return context.Books.Find(id);
            }
        }
        public Book GetWithIncludes(int id)
        {
            using (context = new LibraryDbContext())
            {
                return context.Books
                    .Include(b => b.Author)
                    .Include(b => b.BorrowedBooks)
                    .FirstOrDefault(b => b.Id == id);
            }
        }

        public Book GetByISBN(string ISBN)
        {
            using (context = new LibraryDbContext())
            {
                return context.Books.First(b => b.ISBN == ISBN);
            }
        }

        public Book GetByISBNWithIncludes(string ISBN)
        {
            using (context = new LibraryDbContext())
            {
                return context.Books
                    .Include(b => b.Author)
                    .Include(b => b.BorrowedBooks)
                    .FirstOrDefault(b => b.ISBN == ISBN);
            }
        }

        public void Add(Book book)
        {
            using (context = new LibraryDbContext())
            {
                context.Books.Add(book);
                context.SaveChanges();
            }
        }

        public void Update(Book book)
        {
            using (context = new LibraryDbContext())
            {
                var item = context.Books.Find(book.Id);
                if (item != null)
                {
                    context.Entry(item).CurrentValues.SetValues(book);
                    context.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (context = new LibraryDbContext())
            {
                var book = context.Books.Find(id);
                if (book != null)
                {
                    context.Books.Remove(book);
                    context.SaveChanges();
                }
            }
        }
    }
}
