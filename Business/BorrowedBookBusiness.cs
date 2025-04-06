using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Business
{
    public class BorrowedBookBusiness
    {
        private LibraryDbContext context;

        public List<BorrowedBook> GetAll()
        {
            using (context = new LibraryDbContext())
            {
                return context.BorrowedBooks
                    .Include(bb => bb.Book)
                    .Include(bb => bb.Member)
                    .OrderBy(bb => bb.BorrowDate)
                    .Reverse()
                    .ToList();
            }
        }

        public BorrowedBook GetByBookId(int bookId)
        {
            using (context = new LibraryDbContext())
            {
                return context.BorrowedBooks
                    .Include(bb => bb.Book)
                    .Include(bb => bb.Member)
                    .OrderBy(bb => bb.BorrowDate)
                    .Reverse()
                    .FirstOrDefault(bb => bb.BookID == bookId);
            }
        }

        public List<BorrowedBook> GetAllBetweenDates(DateTime startDate, DateTime endDate)
        {
            return context.BorrowedBooks
                .Where(bb => bb.BorrowDate >= startDate && bb.BorrowDate <= endDate)
                .ToList();
        }

        public BorrowedBook GetByMemberId(int memberId)
        {
            using (context = new LibraryDbContext())
            {
                return context.BorrowedBooks
                    .Include(bb => bb.Book)
                    .Include(bb => bb.Member)
                    .OrderBy(bb => bb.BorrowDate)
                    .Reverse()
                    .FirstOrDefault(bb => bb.MemberID == memberId);
            }
        }

        public BorrowedBook GetByBookIdAndDate(int bookId, DateTime date)
        {
            using (context = new LibraryDbContext())
            {
                return context.BorrowedBooks
                    .Include(bb => bb.Book)
                    .Include(bb => bb.Member)
                    .OrderBy(bb => bb.BorrowDate)
                    .Reverse()
                    .FirstOrDefault(bb => bb.BookID == bookId && bb.BorrowDate == date);
            }
        }

        public void Add(BorrowedBook borrowedBook)
        {
            using (context = new LibraryDbContext())
            {
                context.BorrowedBooks.Add(borrowedBook);
                context.SaveChanges();
            }
        }

        public void Update(BorrowedBook borrowedBook)
        {
            using (context = new LibraryDbContext())
            {
                var item = context.BorrowedBooks
                    .FirstOrDefault(bb => bb.BookID == borrowedBook.BookID);
                if (item != null)
                {
                    context.Entry(item).CurrentValues.SetValues(borrowedBook);
                    context.SaveChanges();
                }
            }
        }

        public void Delete(int bookId)
        {
            using (context = new LibraryDbContext())
            {
                var borrowedBook = context.BorrowedBooks
                    .OrderBy(bb => bb.BorrowDate)
                    .Reverse()
                    .FirstOrDefault(bb => bb.BookID == bookId);
                if (borrowedBook != null)
                {
                    context.BorrowedBooks.Remove(borrowedBook);
                    context.SaveChanges();
                }
            }
        }
        public void DeleteByIdAndDate(int bookId, DateTime borrowDate)
        {
            using (context = new LibraryDbContext())
            {
                var borrowedBook = context.BorrowedBooks
                    .FirstOrDefault(bb => bb.BookID == bookId && bb.BorrowDate == borrowDate);
                if (borrowedBook != null)
                {
                    context.BorrowedBooks.Remove(borrowedBook);
                    context.SaveChanges();
                }
            }
        }
    }
}
