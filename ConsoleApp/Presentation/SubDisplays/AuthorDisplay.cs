using Business;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Presentation.SubDisplays
{
    internal class AuthorDisplay
    {
        // Business layer objects to interact with data models
        private AuthorBusiness authorBusiness = new AuthorBusiness();

        // UI helper to assist with common input/output operations
        private UIHelper uiHelper = new UIHelper();

        public void AuthorManager()
        {
            ShowAuthorMenu();
            var operation = uiHelper.ReadIntInput("Please select an option:");
            switch (operation)
            {
                case 1:
                    ShowAllAuthors();
                    break;
                case 2:
                    AddAuthor();
                    break;
                case 3:
                    UpdateAuthor();
                    break;
                case 4:
                    FetchAuthorById();
                    break;
                case 5:
                    DeleteAuthor();
                    break;
                default:
                    Console.WriteLine("Please select a valid option.");
                    break;
            }
        }
        private void ShowAuthorMenu()
        {
            uiHelper.ShowHeader("Author Management");
            Console.WriteLine("1. All Authors");
            Console.WriteLine("2. Add Author");
            Console.WriteLine("3. Update Author");
            Console.WriteLine("4. Fetch Author by ID");
            Console.WriteLine("5. Delete Author");
        }

        private void ShowAllAuthors()
        {
            var authors = authorBusiness.GetAll();
            if (authors.Count == 0)
            {
                Console.WriteLine("No authors found.");
                return;
            }
            foreach (var author in authors)
            {
                Console.WriteLine($"ID: {author.Id}, Name: {author.FirstName} {author.LastName}\nBio: {author.Biography}");
            }
        }

        private void AddAuthor()
        {
            var author = new Author();
            author.FirstName = uiHelper.ReadStringInput("Enter first name:");
            author.LastName = uiHelper.ReadStringInput("Enter last name:");
            author.DateOfBirth = DateTime.Parse(uiHelper.ReadStringInput("Enter date of birth (yyyy-mm-dd):"));
            author.ImageUrl = uiHelper.ReadStringInput($"Enter image URL:");
            author.Biography = uiHelper.ReadStringInput("Enter biography:");
            authorBusiness.Add(author);
            Console.WriteLine("Author added successfully.");
        }

        private void UpdateAuthor()
        {
            var authorId = uiHelper.ReadIntInput("Enter Author ID to update:");
            var author = authorBusiness.Get(authorId);
            if (author == null)
            {
                Console.WriteLine("Author not found.");
                return;
            }
            author.FirstName = uiHelper.ReadStringInput($"Enter new first name:");
            author.LastName = uiHelper.ReadStringInput($"Enter new last name:");
            author.DateOfBirth = DateTime.Parse(uiHelper.ReadStringInput($"Enter new date of birth:"));
            author.ImageUrl = uiHelper.ReadStringInput($"Enter new image URL:");
            author.Biography = uiHelper.ReadStringInput($"Enter new biography:");
            authorBusiness.Update(author);

            Console.WriteLine("Author updated successfully.");
        }

        private void FetchAuthorById()
        {
            var authorId = uiHelper.ReadIntInput("Enter Author ID to fetch:");
            var author = authorBusiness.GetWithIncludes(authorId);
            if (author == null)
            {
                Console.WriteLine("Author not found.");
                return;
            }
            Console.WriteLine($"ID: {author.Id}\nName: {author.FirstName} {author.LastName}\nDate of birth:{author.DateOfBirth.ToShortDateString()}\nBio:\n{author.Biography}\nBooks:");
            foreach (var book in author.Books)
            {
                if(book.BorrowedBooks.Any(bb => bb.ReturnDate == null))
                {
                    Console.WriteLine($"Book ID: {book.Id}, Title: {book.Title}, Genre: {book.Genre}, Published Year: {book.PublicationDate.ToShortDateString}, Status: Rented out");
                }
                else
                {
                    Console.WriteLine($"Book ID: {book.Id}, Title: {book.Title}, Genre: {book.Genre}, Published Year: {book.PublicationDate.ToShortDateString}, Status: Available");
                }
            }
        }

        private void DeleteAuthor()
        {
            var authorId = uiHelper.ReadIntInput("Enter Author ID to delete:");
            var author = authorBusiness.Get(authorId);
            if (author == null)
            {
                Console.WriteLine("Author not found.");
                return;
            }
            authorBusiness.Delete(authorId);
            Console.WriteLine("Author deleted successfully.");
        }
    }
}