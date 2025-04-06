using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Business
{
    public class MemberBusiness
    {
        private LibraryDbContext context;

        public List<Member> GetAll()
        {
            using (context = new LibraryDbContext())
            {
                return context.Members.ToList();
            }
        }

        public Member Get(int id)
        {
            using (context = new LibraryDbContext())
            {
                return context.Members.Find(id);
            }
        }

        public Member GetWithIncludes(int id)
        {
            using (context = new LibraryDbContext())
            {
                return context.Members
                    .Include(m => m.BorrowedBooks)
                    .ThenInclude(bb => bb.Book)
                    .FirstOrDefault(m => m.Id == id);
            }
        }

        public void Add(Member member)
        {
            using (context = new LibraryDbContext())
            {
                context.Members.Add(member);
                context.SaveChanges();
            }
        }

        public void Update(Member member)
        {
            using (context = new LibraryDbContext())
            {
                var item = context.Members.Find(member.Id);
                if (item != null)
                {
                    context.Entry(item).CurrentValues.SetValues(member);
                    context.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (context = new LibraryDbContext())
            {
                var member = context.Members.Find(id);
                if (member != null)
                {
                    context.Members.Remove(member);
                    context.SaveChanges();
                }
            }
        }
    }
}
