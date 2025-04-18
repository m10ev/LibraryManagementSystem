using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{
    public class BorrowedBookBusiness : IDisposable
    {
        private readonly LibraryDbContext _context;
        private readonly bool _contextOwned;

        // Constructor for ASP.NET Core (DI provides the _context)
        public BorrowedBookBusiness(LibraryDbContext context)
        {
            _context = context;
            _contextOwned = false;
        }

        // Constructor for Console App (creates its own _context)
        public BorrowedBookBusiness()
        {
            _context = new LibraryDbContext();
            _contextOwned = true;
        }

        public async Task<List<BorrowedBook>> GetAllAsync()
        {
                return await _context.BorrowedBooks
                    .Include(bb => bb.Book)
                    .Include(bb => bb.Member)
                    .OrderBy(bb => bb.BorrowDate)
                    .Reverse()
                    .ToListAsync();
        }

        public async Task<BorrowedBook> GetByBookIdAsync(int bookId)
        {
                return await _context.BorrowedBooks
                    .Include(bb => bb.Book)
                    .Include(bb => bb.Member)
                    .OrderBy(bb => bb.BorrowDate)
                    .Reverse()
                    .FirstOrDefaultAsync(bb => bb.BookID == bookId);
        }

        public async Task<List<BorrowedBook>> GetAllBetweenDatesAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.BorrowedBooks
                .Where(bb => bb.BorrowDate >= startDate && bb.BorrowDate <= endDate)
                .ToListAsync();
        }

        public async Task<BorrowedBook> GetByMemberIdAsync(int memberId)
        {
                return await _context.BorrowedBooks
                    .Include(bb => bb.Book)
                    .Include(bb => bb.Member)
                    .OrderBy(bb => bb.BorrowDate)
                    .Reverse()
                    .FirstOrDefaultAsync(bb => bb.MemberID == memberId);
        }

        public async Task<BorrowedBook> GetByBookIdAndDateAsync(int bookId, DateTime date)
        {
                return await _context.BorrowedBooks
                    .Include(bb => bb.Book)
                    .Include(bb => bb.Member)
                    .OrderBy(bb => bb.BorrowDate)
                    .Reverse()
                    .FirstOrDefaultAsync(bb => bb.BookID == bookId && bb.BorrowDate == date);
        }

        public async Task AddAsync(BorrowedBook borrowedBook)
        {
                await _context.BorrowedBooks.AddAsync(borrowedBook);
                await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BorrowedBook borrowedBook)
        {
                var item = await _context.BorrowedBooks
                    .FirstOrDefaultAsync(bb => bb.BookID == borrowedBook.BookID && bb.MemberID == borrowedBook.MemberID);
                if (item != null)
                {
                    _context.Entry(item).CurrentValues.SetValues(borrowedBook);
                    await _context.SaveChangesAsync();
                }
        }

        public async Task DeleteAsync(int bookId)
        {
                var borrowedBook = await _context.BorrowedBooks
                    .OrderBy(bb => bb.BorrowDate)
                    .Reverse()
                    .FirstOrDefaultAsync(bb => bb.BookID == bookId);
                if (borrowedBook != null)
                {
                    _context.BorrowedBooks.Remove(borrowedBook);
                    await _context.SaveChangesAsync();
                }
        }
        public async Task DeleteByIdAndDateAsync(int bookId, DateTime borrowDate)
        {
                var borrowedBook = await _context.BorrowedBooks
                    .FirstOrDefaultAsync(bb => bb.BookID == bookId && bb.BorrowDate == borrowDate);
                if (borrowedBook != null)
                {
                    _context.BorrowedBooks.Remove(borrowedBook);
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
