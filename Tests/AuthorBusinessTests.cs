using Business;
using Data.Enums;
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
    public class AuthorBusinessTests
    {
        [Test]
        public async Task GetAllTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                context.Authors.Add(new Author { Id = 1, FirstName = "Author", LastName = "1", Biography = "abcdefg", DateOfBirth = DateTime.Now.AddYears(-1), ImageUrl = "randomUrl" });
                context.Authors.Add(new Author { Id = 2, FirstName = "Author", LastName = "2", Biography = "gfedcba", DateOfBirth = DateTime.Now.AddYears(-2), ImageUrl = "randomUrl" });
                context.SaveChanges();

                AuthorBusiness authorBusiness = new AuthorBusiness(context);

                // Act
                List<Author> authors = await authorBusiness.GetAllAsync();

                // Assert
                Assert.That(authors.Count, Is.EqualTo(2));
            }
        }

        [Test]
        public async Task GetTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                context.Authors.Add(new Author { Id = 1, FirstName = "Author", LastName = "1", Biography = "abcdefg", DateOfBirth = DateTime.Now.AddYears(-1), ImageUrl = "randomUrl" });
                context.Authors.Add(new Author { Id = 2, FirstName = "Author", LastName = "2", Biography = "gfedcba", DateOfBirth = DateTime.Now.AddYears(-2), ImageUrl = "randomUrl" });
                context.SaveChanges();

                AuthorBusiness authorBusiness = new AuthorBusiness(context);

                // Act
                Author author = await authorBusiness.GetAsync(2);

                // Assert
                Assert.That(author, Is.Not.Null);
                Assert.That(author.Id, Is.EqualTo(2));
                Assert.That(author, Is.EqualTo(context.Authors.Find(2)));
            }
        }

        [Test]
        public async Task GetWithIncludesTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                context.Authors.Add(new Author { Id = 1, FirstName = "Author", LastName = "1", Biography = "abcdefg", DateOfBirth = DateTime.Now.AddYears(-1), ImageUrl = "randomUrl" });
                context.Authors.Add(new Author { Id = 2, FirstName = "Author", LastName = "2", Biography = "gfedcba", DateOfBirth = DateTime.Now.AddYears(-2), ImageUrl = "randomUrl" });
                context.SaveChanges();

                AuthorBusiness authorBusiness = new AuthorBusiness(context);

                // Act
                Author author = await authorBusiness.GetWithIncludesAsync(2);

                // Assert
                Assert.That(author, Is.Not.Null);
                Assert.That(author.Id, Is.EqualTo(2));
                Assert.That(author, Is.EqualTo(context.Authors.Find(2)));
                Assert.That(author.Books, Is.Not.Null); // Navigation property expected to be loaded
            }
        }

        [Test]
        public async Task AddTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                AuthorBusiness authorBusiness = new AuthorBusiness(context);
                Author author = new Author { Id = 3, FirstName = "Author", LastName = "3", Biography = "abcdefg", DateOfBirth = DateTime.Now.AddYears(-1), ImageUrl = "randomUrl" };

                // Act
                await authorBusiness.AddAsync(author);

                // Assert
                Assert.That(author, Is.Not.Null);
                Assert.That(author.Id, Is.EqualTo(3));
                Assert.That(author, Is.EqualTo(context.Authors.Find(3)));
            }
        }

        [Test]
        public async Task UpdateTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                context.Authors.Add(new Author { Id = 1, FirstName = "Author", LastName = "1", Biography = "abcdefg", DateOfBirth = DateTime.Now.AddYears(-1), ImageUrl = "randomUrl" });
                context.Authors.Add(new Author { Id = 2, FirstName = "Author", LastName = "2", Biography = "gfedcba", DateOfBirth = DateTime.Now.AddYears(-2), ImageUrl = "randomUrl" });
                context.SaveChanges();

                AuthorBusiness authorBusiness = new AuthorBusiness(context);
                Author author = new Author { Id = 1, FirstName = "UpdatedAuthor", LastName = "1", Biography = "abcdefg", DateOfBirth = DateTime.Now.AddYears(-1), ImageUrl = "randomUrl" };

                // Act
                await authorBusiness.UpdateAsync(author);

                // Assert
                Assert.That(author, Is.Not.Null);
                Assert.That(context.Authors.Count(), Is.EqualTo(2));
                Assert.That(author.Id, Is.EqualTo(1));
                Assert.That(author.FirstName, Is.EqualTo("UpdatedAuthor"));
            }
        }

        [Test]
        public async Task DeleteTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                context.Authors.Add(new Author { Id = 1, FirstName = "Author", LastName = "1", Biography = "abcdefg", DateOfBirth = DateTime.Now.AddYears(-1), ImageUrl = "randomUrl" });
                context.Authors.Add(new Author { Id = 2, FirstName = "Author", LastName = "2", Biography = "gfedcba", DateOfBirth = DateTime.Now.AddYears(-2), ImageUrl = "randomUrl" });
                context.SaveChanges();

                AuthorBusiness authorBusiness = new AuthorBusiness(context);

                // Act
                await authorBusiness.DeleteAsync(2);

                // Assert
                Assert.That(context.Authors.Count(), Is.EqualTo(1));
                Assert.That(context.Authors.Find(2), Is.Null);
            }
        }
    }
}
