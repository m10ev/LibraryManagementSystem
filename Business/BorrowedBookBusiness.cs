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
                    .First(bb => bb.BookID == bookId);
            }
        }

        public BorrowedBook GetByMemberId(int memberId)
        {
            using (context = new LibraryDbContext())
            {
                return context.BorrowedBooks
                    .Include(bb => bb.Book)
                    .Include(bb => bb.Member)
                    .First(bb => bb.MemberID == memberId);
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
                    .FirstOrDefault(bb => bb.BookID == borrowedBook.BookID && bb.MemberID == borrowedBook.MemberID);
                if (item != null)
                {
                    context.Entry(item).CurrentValues.SetValues(borrowedBook);
                    context.SaveChanges();
                }
            }
        }

        public void Delete(int bookId, int memberId)
        {
            using (context = new LibraryDbContext())
            {
                var borrowedBook = context.BorrowedBooks
                    .FirstOrDefault(bb => bb.BookID == bookId && bb.MemberID == memberId);
                if (borrowedBook != null)
                {
                    context.BorrowedBooks.Remove(borrowedBook);
                    context.SaveChanges();
                }
            }
        }
    }
}
