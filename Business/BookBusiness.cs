using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business 
{
    public class BookBusiness : IDisposable
    {
        private readonly LibraryDbContext _context;
        private readonly bool _contextOwned;

        // Constructor for ASP.NET Core (DI provides the context)
        public BookBusiness(LibraryDbContext context)
        {
            _context = context;
            _contextOwned = false;
        }

        // Constructor for Console App (creates its own context)
        public BookBusiness()
        {
            _context = new LibraryDbContext();
            _contextOwned = true;
        }

        public async Task<List<Book>> GetAllAsync()
        {
                return await _context.Books.ToListAsync();
        }

        public async Task<List<Book>> GetAllWithIncludesAsync()
        {
                return await _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.BorrowedBooks)
                    .ToListAsync();
        }

        public async Task<List<Book>> GetAllByGenreAsync(string genre)
        {
                return await _context.Books
                    .Where(b => b.Genre.ToString().ToLower() == genre.ToLower())
                    .ToListAsync();
        }

        public async Task<List<Book>> GetAllByGenreWithIncludesAsync(string genre)
        {
                return await _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.BorrowedBooks)
                    .Where(b => b.Genre.ToString().ToLower() == genre.ToLower())
                    .ToListAsync();
        }

        public async Task<Book> GetAsync(int id)
        {
                return await _context.Books.FindAsync(id);
        }
        public async Task<Book> GetWithIncludesAsync(int id)
        {
                return await _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.BorrowedBooks)
                    .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> GetByISBNAsync(string ISBN)
        {
                return await _context.Books.FirstAsync(b => b.ISBN == ISBN);
        }

        public async Task<Book> GetByISBNWithIncludesAsync(string ISBN)
        {        
                return await _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.BorrowedBooks)
                    .FirstOrDefaultAsync(b => b.ISBN == ISBN);
        }

        public async Task AddAsync(Book book)
        {
                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
                var item = await _context.Books.FindAsync(book.Id);
                if (item != null)
                {
                    _context.Entry(item).CurrentValues.SetValues(book);
                    await _context.SaveChangesAsync();
                }
        }

        public async Task DeleteAsync(int id)
        {
                var book = await _context.Books.FindAsync(id);
                if (book != null)
                {
                    _context.Books.Remove(book);
                    await _context.SaveChangesAsync();
                }
        }

        // Make sure you clean up if we created the context ourselves
        public void Dispose()
        {
            if (_contextOwned)
            {
                _context.Dispose();
            }
        }
    }
}
