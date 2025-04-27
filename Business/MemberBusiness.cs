using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{
    public class MemberBusiness : IDisposable
    {
        private readonly LibraryDbContext _context;
        private readonly bool _contextOwned;

        // Constructor for ASP.NET Core (DI provides the context)
        public MemberBusiness(LibraryDbContext context)
        {
            _context = context;
            _contextOwned = false;
        }

        // Constructor for Console App (creates its own context)
        public MemberBusiness()
        {
            _context = new LibraryDbContext();
            _contextOwned = true;
        }

        public async Task<List<Member>> GetAllAsync()
        {
                return await _context.Members.ToListAsync();
        }

        public async Task<Member> GetAsync(int id)
        {
                return await _context.Members.FindAsync(id);
        }

        public async Task<Member> GetWithIncludesAsync(int id)
        {
                return await _context.Members
                    .Include(m => m.BorrowedBooks)
                    .ThenInclude(bb => bb.Book)
                    .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Member member)
        {
                await _context.Members.AddAsync(member);
                await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Member member)
        {
                var item = await _context.Members.FindAsync(member.Id);
                if (item != null)
                {
                    _context.Entry(item).CurrentValues.SetValues(member);
                    await _context.SaveChangesAsync();
                }
        }

        public async Task DeleteAsync(int id)
        {
                var member = await _context.Members.FindAsync(id);
                if (member != null)
                {
                    _context.Members.Remove(member);
                    await _context.SaveChangesAsync();
                }
        }

        public async Task RenewMembership(Member member, int years)
        {
            if(member.MembershipExpireDate > DateTime.Now)
            {
                member.MembershipExpireDate = member.MembershipExpireDate.AddYears(years).Date;
            }
            else
            {
                member.MembershipExpireDate = DateTime.Now.AddYears(years).Date;
                await UpdateAsync(member);
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