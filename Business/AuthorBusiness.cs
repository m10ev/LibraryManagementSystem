using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Business
{
    public class AuthorBusiness : IDisposable
    {
        private readonly LibraryDbContext _context;
        private readonly bool _contextOwned;

        // Constructor for ASP.NET Core (DI provides the context)
        public AuthorBusiness(LibraryDbContext context)
        {
            _context = context;
            _contextOwned = false;
        }

        // Constructor for Console App (creates its own context)
        public AuthorBusiness()
        {
            _context = new LibraryDbContext();
            _contextOwned = true;
        }

        public async Task<List<Author>> GetAllAsync()
        {
               return await _context.Authors.ToListAsync();
        }


        public async Task<Author> GetAsync(int id)
        {
                return await _context.Authors.FindAsync(id);
        }

        public async Task<Author> GetWithIncludesAsync(int id)
        {
                return await _context.Authors
                    .Include(a => a.Books)
                    .ThenInclude(b => b.BorrowedBooks)
                    .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Author author)
        {
                await _context.Authors.AddAsync(author);
                await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Author author)
        {
                var item = await _context.Authors.FindAsync(author.Id);
                if (item != null)
                {
                    _context.Entry(item).CurrentValues.SetValues(author);
                    await _context.SaveChangesAsync();
                }
        }

        public async Task DeleteAsync(int id)
        {
                var author = await _context.Authors.FindAsync(id);
                if (author != null)
                {
                    _context.Authors.Remove(author);
                    await _context.SaveChangesAsync();
                }
        }

        // Make sure you clean up if we created the _context ourselves
        public void Dispose()
        {
            if (_contextOwned)
            {
                _context.Dispose();
            }
        }
    }
}
