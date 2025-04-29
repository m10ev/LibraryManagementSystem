using Business;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class MembersController : Controller
    {
        private readonly MemberBusiness _memberBusiness;

        public MembersController(LibraryDbContext context)
        {
            _memberBusiness = new MemberBusiness(context);
        }

        // GET: /Members
        public async Task<IActionResult> Index()
        {
            var members = await _memberBusiness.GetAllAsync();
            return View(members);
        }

        // GET: /Members/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var member = await _memberBusiness.GetWithIncludesAsync(id);
            if (member == null)
                return NotFound();

            return View(member);
        }

        // GET: /Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
            if (ModelState.IsValid)
            {
                await _memberBusiness.AddAsync(member);
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: /Members/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var member = await _memberBusiness.GetAsync(id);
            if (member == null)
                return NotFound();

            return View(member);
        }

        // POST: /Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Member member)
        {
            if (ModelState.IsValid)
            {
                await _memberBusiness.UpdateAsync(member);
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: /Members/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var member = await _memberBusiness.GetWithIncludesAsync(id);
            if (member == null)
                return NotFound();

            return View(member);
        }

        // POST: /Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _memberBusiness.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Members/RenewMembership/5
        public async Task<IActionResult> RenewMembership(int id)
        {
            var member = await _memberBusiness.GetAsync(id);
            if (member == null)
                return NotFound();

            return View(member);
        }

        // POST: /Members/RenewMembership/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RenewMembership(int id, int years)
        {
            if (years <= 0)
            {
                var member = await _memberBusiness.GetAsync(id);
                ModelState.AddModelError("years", "Please enter a positive number of years.");
                return View(member);
            }

            var targetMember = await _memberBusiness.GetAsync(id);
            if (targetMember == null)
                return NotFound();

            await _memberBusiness.RenewMembership(targetMember, years);
            return RedirectToAction(nameof(Index));
        }
    }
}
