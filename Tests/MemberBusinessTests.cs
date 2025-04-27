using Business;
using Data.Models;
using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    internal class MemberBusinessTests
    {
        [Test]
        public async Task GetAllTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Members.Add(new Member { Id = 1, FirstName = "Member", LastName = "1", PhoneNumber = "1234567890", MembershipExpireDate = DateTime.Now.AddYears(1) });
                context.Members.Add(new Member { Id = 2, FirstName = "Member", LastName = "2", PhoneNumber = "0987654321", MembershipExpireDate = DateTime.Now.AddYears(1 / 2) });

                context.SaveChanges();

                MemberBusiness memberBusiness = new MemberBusiness(context);
                List<Member> members = await memberBusiness.GetAllAsync();

                Assert.That(members.Count, Is.EqualTo(2));
            }
        }

        [Test]
        public async Task GetTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Members.Add(new Member { Id = 1, FirstName = "Member", LastName = "1", PhoneNumber = "1234567890", MembershipExpireDate = DateTime.Now.AddYears(1) });
                context.Members.Add(new Member { Id = 2, FirstName = "Member", LastName = "2", PhoneNumber = "0987654321", MembershipExpireDate = DateTime.Now.AddYears(1 / 2) });

                context.SaveChanges();

                MemberBusiness memberBusiness = new MemberBusiness(context);
                Member member = await memberBusiness.GetAsync(1);

                Assert.That(member, Is.Not.Null);
                Assert.That(member.Id, Is.EqualTo(1));
                Assert.That(member, Is.EqualTo(context.Members.Find(1)));
            }
        }

        [Test]
        public async Task GetWithIncludesTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Members.Add(new Member { Id = 1, FirstName = "Member", LastName = "1", PhoneNumber = "1234567890", MembershipExpireDate = DateTime.Now.AddYears(1) });
                context.Members.Add(new Member { Id = 2, FirstName = "Member", LastName = "2", PhoneNumber = "0987654321", MembershipExpireDate = DateTime.Now.AddYears(1 / 2) });

                context.SaveChanges();

                MemberBusiness memberBusiness = new MemberBusiness(context);
                Member member = await memberBusiness.GetWithIncludesAsync(1);

                Assert.That(member, Is.Not.Null);
                Assert.That(member.Id, Is.EqualTo(1));
                Assert.That(member, Is.EqualTo(context.Members.Find(1)));

                Assert.That(member.BorrowedBooks, Is.Not.Null);
            }
        }

        [Test]
        public async Task AddTask()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                MemberBusiness memberBusiness = new MemberBusiness(context);
                Member member = new Member { Id = 1, FirstName = "Member", LastName = "1", PhoneNumber = "1234567890", MembershipExpireDate = DateTime.Now.AddYears(1) };
                await memberBusiness.AddAsync(member);

                Assert.That(context.Members.Count(), Is.EqualTo(1));
                Assert.That(context.Members.First().Id, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task UpdateTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Members.Add(new Member { Id = 1, FirstName = "Member", LastName = "1", PhoneNumber = "1234567890", MembershipExpireDate = DateTime.Now.AddYears(1) });
                context.Members.Add(new Member { Id = 2, FirstName = "Member", LastName = "2", PhoneNumber = "0987654321", MembershipExpireDate = DateTime.Now.AddYears(1 / 2) });

                context.SaveChanges();

                MemberBusiness memberBusiness = new MemberBusiness(context);
                Member member = new Member { Id = 1, FirstName = "MemberUpdated", LastName = "1", PhoneNumber = "1234567890", MembershipExpireDate = DateTime.Now.AddYears(2) };
                await memberBusiness.UpdateAsync(member);

                Assert.That(context.Members.Count(), Is.EqualTo(2));
                Assert.That(member.Id, Is.EqualTo(1));
                Assert.That(member.FirstName, Is.EqualTo(context.Members.Find(1).FirstName));
            }
        }

        [Test]
        public async Task DeleteTest()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Members.Add(new Member { Id = 1, FirstName = "Member", LastName = "1", PhoneNumber = "1234567890", MembershipExpireDate = DateTime.Now.AddYears(1) });
                context.Members.Add(new Member { Id = 2, FirstName = "Member", LastName = "2", PhoneNumber = "0987654321", MembershipExpireDate = DateTime.Now.AddYears(1 / 2) });

                context.SaveChanges();

                MemberBusiness memberBusiness = new MemberBusiness(context);
                await memberBusiness.DeleteAsync(1);

                Assert.That(context.Members.Count(), Is.EqualTo(1));
                Assert.That(context.Members.Find(1), Is.Null);
            }
        }

        [Test]
        public async Task RenewMembership()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            // Insert seed data into the database using one instance of the context

            using (var context = new LibraryDbContext(options))
            {
                context.Members.Add(new Member { Id = 1, FirstName = "Member", LastName = "1", PhoneNumber = "1234567890", MembershipExpireDate = DateTime.Now.AddYears(-1) });
                context.Members.Add(new Member { Id = 2, FirstName = "Member", LastName = "2", PhoneNumber = "0987654321", MembershipExpireDate = DateTime.Now.AddYears(1 / 2) });

                context.SaveChanges();

                MemberBusiness memberBusiness = new MemberBusiness(context);
                var member = await memberBusiness.GetAsync(1);
                await memberBusiness.RenewMembership(member, 1);

                Assert.That(memberBusiness.GetAsync(1).Result.MembershipExpireDate, Is.EqualTo(DateTime.Now.AddYears(1).Date));
            }
        }
    }
}
