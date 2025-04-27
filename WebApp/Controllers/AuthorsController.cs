using Business;
using Data.Models;
using Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly AuthorBusiness _authorBusiness;

        public AuthorsController(LibraryDbContext context)
        {
            _authorBusiness = new AuthorBusiness(context);
        }

        // GET: /Authors
        public async Task<IActionResult> Index()
        {
            var authors = await _authorBusiness.GetAllAsync();
            return View(authors);
        }

        // GET: /Authors/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var author = await _authorBusiness.GetWithIncludesAsync(id);
            if (author == null)
                return NotFound();

            return View(author);
        }

        // GET: /Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            if (ModelState.IsValid)
            {
                await _authorBusiness.AddAsync(author);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: /Authors/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var author = await _authorBusiness.GetWithIncludesAsync(id);
            if (author == null)
                return NotFound();

            return View(author);
        }

        // POST: /Authors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Author author)
        {
            if (ModelState.IsValid)
            {
                await _authorBusiness.UpdateAsync(author);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: /Authors/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _authorBusiness.GetWithIncludesAsync(id);
            if (author == null)
                return NotFound();

            return View(author);
        }

        // POST: /Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _authorBusiness.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
