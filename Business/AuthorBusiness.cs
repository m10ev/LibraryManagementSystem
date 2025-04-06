using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Business
{
    public class AuthorBusiness
    {
        private LibraryDbContext context;

        public List<Author> GetAll()
        {
            using (context = new LibraryDbContext())
            {
                return context.Authors.ToList();
            }
        }


        public Author Get(int id)
        {
            using (context = new LibraryDbContext())
            {
                return context.Authors.Find(id);
            }
        }

        public Author GetWithIncludes(int id)
        {
            using (context = new LibraryDbContext())
            {
                return context.Authors
                    .Include(a => a.Books)
                    .ThenInclude(b => b.BorrowedBooks)
                    .FirstOrDefault(a => a.Id == id);
            }
        }

        public void Add(Author author)
        {
            using (context = new LibraryDbContext())
            {
                context.Authors.Add(author);
                context.SaveChanges();
            }
        }

        public void Update(Author author)
        {
            using (context = new LibraryDbContext())
            {
                var item = context.Authors.Find(author.Id);
                if (item != null)
                {
                    context.Entry(item).CurrentValues.SetValues(author);
                    context.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (context = new LibraryDbContext())
            {
                var author = context.Authors.Find(id);
                if (author != null)
                {
                    context.Authors.Remove(author);
                    context.SaveChanges();
                }
            }
        }
    }
}
