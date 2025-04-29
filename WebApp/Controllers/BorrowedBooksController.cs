using Business;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Controllers
{
    public class BorrowedBooksController : Controller
    {
        private readonly BorrowedBookBusiness _borrowedBookBusiness;
        private readonly BookBusiness _bookBusiness;
        private readonly MemberBusiness _memberBusiness;

        public BorrowedBooksController(LibraryDbContext context)
        {
            _borrowedBookBusiness = new BorrowedBookBusiness(context);
            _bookBusiness = new BookBusiness(context);
            _memberBusiness = new MemberBusiness(context);
        }

        // GET: /BorrowedBooks
        public async Task<IActionResult> Index()
        {
            var borrowedBooks = await _borrowedBookBusiness.GetAllAsync();
            return View(borrowedBooks);
        }

        // GET: /BorrowedBooks/Details
        public async Task<IActionResult> Details(int bookId, DateTime date)
        {
            var borrowedBook = await _borrowedBookBusiness.GetByBookIdAndDateAsync(bookId, date);
            if (borrowedBook == null)
                return NotFound();

            return View(borrowedBook);
        }

        // GET: /BorrowedBooks/Create
        public async Task<IActionResult> Create()
        {
            // Filter books: Get books that are not currently borrowed or have been returned (i.e., ReturnDate is not null)
            var availableBooks = await _bookBusiness.GetAllWithIncludesAsync();
            var booksNotBorrowed = availableBooks.Where(b => b.BorrowedBooks == null || !b.BorrowedBooks.Any(bb => bb.ReturnDate == null )).ToList();

            // Filter members: Get members with non-expired memberships
            var activeMembers = await _memberBusiness.GetAllAsync();
            var membersWithActiveMembership = activeMembers.Where(m => m.MembershipExpireDate >= DateTime.Now).ToList();

            // Create the SelectList for books
            ViewData["BookID"] = new SelectList(booksNotBorrowed, "Id", "Title");

            // Create the SelectList for members
            ViewData["MemberID"] = new SelectList(membersWithActiveMembership, "Id", "FirstName");

            return View();
        }

        // POST: /BorrowedBooks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BorrowedBook borrowedBook)
        {
            if (ModelState.IsValid)
            {
                // Add the new borrowed book record
                await _borrowedBookBusiness.AddAsync(borrowedBook);
                return RedirectToAction(nameof(Index));
            }

            var availableBooks = await _bookBusiness.GetAllWithIncludesAsync();
            var booksNotBorrowed = availableBooks.Where(b => b.BorrowedBooks == null || !b.BorrowedBooks.Any(bb => bb.ReturnDate == null)).ToList();

            var activeMembers = await _memberBusiness.GetAllAsync();
            var membersWithActiveMembership = activeMembers.Where(m => m.MembershipExpireDate >= DateTime.Now).ToList();

            ViewData["BookID"] = new SelectList(booksNotBorrowed, "Id", "Title", borrowedBook.BookID);
            ViewData["MemberID"] = new SelectList(membersWithActiveMembership, "Id", "FirstName", borrowedBook.MemberID);

            return View(borrowedBook);
        }


        // GET: /BorrowedBooks/Edit
        public async Task<IActionResult> Edit(int bookId, DateTime date)
        {
            var borrowedBook = await _borrowedBookBusiness.GetByBookIdAndDateAsync(bookId, date);
            if (borrowedBook == null)
                return NotFound();

            ViewData["BookId"] = new SelectList(
        _bookBusiness.GetAllAsync().Result.Select(a => new SelectListItem
        {
            Value = a.Id.ToString(),
            Text = $"{a.Title}"
        }),
        "Value", "Text");

            ViewData["MemberId"] = new SelectList(
        _memberBusiness.GetAllAsync().Result.Select(a => new SelectListItem
        {
            Value = a.Id.ToString(),
            Text = $"{a.FirstName} {a.LastName}"
        }),
        "Value", "Text");

            return View(borrowedBook);
        }

        // POST: /BorrowedBooks/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BorrowedBook borrowedBook)
        {
            if (ModelState.IsValid)
            {
                await _borrowedBookBusiness.UpdateAsync(borrowedBook);
                return RedirectToAction(nameof(Index));
            }

            ViewData["BookID"] = new SelectList(await _bookBusiness.GetAllAsync(), "Id", "Title", borrowedBook.BookID);
            ViewData["MemberID"] = new SelectList(await _memberBusiness.GetAllAsync(), "Id", "FirstName", borrowedBook.MemberID);

            return View(borrowedBook);
        }

        // GET: /BorrowedBooks/Delete
        public async Task<IActionResult> Delete(int bookId, DateTime date)
        {
            var borrowedBook = await _borrowedBookBusiness.GetByBookIdAndDateAsync(bookId, date);
            if (borrowedBook == null)
                return NotFound();

            return View(borrowedBook);
        }

        // POST: /BorrowedBooks/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int bookId, DateTime date)
        {
            await _borrowedBookBusiness.DeleteByIdAndDateAsync(bookId, date);
            return RedirectToAction(nameof(Index));
        }

        // GET: /BorrowedBooks/Return/{bookId}/{borrowDate}
        public async Task<IActionResult> Return(int bookId, DateTime date)
        {
            // Retrieve the borrowed book from the database
            var borrowedBook = await _borrowedBookBusiness.GetByBookIdAndDateAsync(bookId, date);

            if (borrowedBook == null)
            {
                return NotFound();
            }

            // Return the return view, passing the borrowed book details
            return View(borrowedBook);
        }

        // POST: /BorrowedBooks/Return
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnConfiremd(int bookId, DateTime date)
        {
            var borrowedBook = await _borrowedBookBusiness.GetByBookIdAndDateAsync(bookId, date);

            if (borrowedBook == null)
            {
                return NotFound();
            }
            borrowedBook.ReturnDate = DateTime.Now;
            await _borrowedBookBusiness.UpdateAsync(borrowedBook);

            return RedirectToAction(nameof(Index));
        }
    }
}
