using Business;
using Data.Models;
using Data;
using Microsoft.AspNetCore.Mvc;
using Data.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookBusiness _bookBusiness;
        private readonly AuthorBusiness _authorBusiness;

        public BooksController(LibraryDbContext context)
        {
            _authorBusiness = new AuthorBusiness(context);
            _bookBusiness = new BookBusiness(context);
        }

        // GET: /Books
        public async Task<IActionResult> Index()
        {
            var books = await _bookBusiness.GetAllWithIncludesAsync();
            return View(books);
        }

        // GET: /Books/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookBusiness.GetWithIncludesAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        // GET: /Books/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AuthorId"] = new SelectList(
        _authorBusiness.GetAllAsync().Result.Select(a => new SelectListItem
        {
            Value = a.Id.ToString(),
            Text = $"{a.FirstName} {a.LastName}" // Concatenating FirstName and LastName
        }),
        "Value", "Text");

            return View();
        }


        // POST: /Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                await _bookBusiness.AddAsync(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: /Books/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookBusiness.GetWithIncludesAsync(id);
            if (book == null)
                return NotFound();

            ViewData["AuthorId"] = new SelectList(
        _authorBusiness.GetAllAsync().Result.Select(a => new SelectListItem
        {
            Value = a.Id.ToString(),
            Text = $"{a.FirstName} {a.LastName}" // Concatenating FirstName and LastName
        }),
        "Value", "Text");

            return View(book);
        }

        // POST: /Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                await _bookBusiness.UpdateAsync(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: /Books/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookBusiness.GetWithIncludesAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        // POST: /Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookBusiness.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
