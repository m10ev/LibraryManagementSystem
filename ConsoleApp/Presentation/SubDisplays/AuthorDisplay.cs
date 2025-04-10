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
        private readonly AuthorBusiness authorBusiness = new AuthorBusiness();

        // UI helper to assist with common input/output operations
        private readonly UIHelper uiHelper = new UIHelper();

        /// <summary>
        /// Main function to manage author-related operations.
        /// Displays a menu with options like adding, updating, deleting, or viewing authors.
        /// </summary>
        public async Task AuthorManager()
        {
            ShowAuthorMenu(); // Display the author management menu
            var operation = uiHelper.ReadIntInput("Please select an option:");

            // Execute the selected operation based on user's choice
            switch (operation)
            {
                case 1:
                    await ShowAllAuthors(); // Display all authors
                    break;
                case 2:
                    await AddAuthor(); // Add a new author
                    break;
                case 3:
                    await UpdateAuthor(); // Update author details
                    break;
                case 4:
                    await FetchAuthorById(); // Fetch an author by their ID
                    break;
                case 5:
                    await DeleteAuthor(); // Delete an author from the system
                    break;
                default:
                    Console.WriteLine("Please select a valid option."); // Handle invalid options
                    break;
            }
        }

        /// <summary>
        /// Displays the main menu for author management.
        /// </summary>
        private void ShowAuthorMenu()
        {
            uiHelper.ShowHeader("Author Management");
            Console.WriteLine("1. All Authors");
            Console.WriteLine("2. Add Author");
            Console.WriteLine("3. Update Author");
            Console.WriteLine("4. Fetch Author by ID");
            Console.WriteLine("5. Delete Author");
        }

        /// <summary>
        /// Displays a list of all authors in the system.
        /// </summary>
        private async Task ShowAllAuthors()
        {
            var authors = await authorBusiness.GetAllAsync(); // Retrieve all authors
            if (authors.Count == 0)
            {
                Console.WriteLine("No authors found."); // Handle case where there are no authors
                return;
            }
            foreach (var author in authors)
            {
                // Display author details
                Console.WriteLine($"ID: {author.Id}, Name: {author.FirstName} {author.LastName}\nBio: {author.Biography}");
            }
        }

        /// <summary>
        /// Prompts the user to input author details and adds the new author to the system.
        /// </summary>
        private async Task AddAuthor()
        {
            var author = new Author();
            author.FirstName = uiHelper.ReadStringInput("Enter first name:"); // Prompt for first name
            author.LastName = uiHelper.ReadStringInput("Enter last name:"); // Prompt for last name
            author.DateOfBirth = DateTime.Parse(uiHelper.ReadStringInput("Enter date of birth (yyyy-mm-dd):")); // Prompt for date of birth
            author.ImageUrl = uiHelper.ReadStringInput($"Enter image URL:"); // Prompt for image URL
            author.Biography = uiHelper.ReadStringInput("Enter biography:"); // Prompt for biography
            await authorBusiness.AddAsync(author); // Add author to the system
            Console.WriteLine("Author added successfully.");
        }

        /// <summary>
        /// Allows the user to update an existing author's details.
        /// </summary>
        private async Task UpdateAuthor()
        {
            var authorId = uiHelper.ReadIntInput("Enter Author ID to update:"); // Prompt for author ID
            var author = await authorBusiness.GetAsync(authorId); // Fetch author by ID
            if (author == null)
            {
                Console.WriteLine("Author not found."); // Handle case where author doesn't exist
                return;
            }
            // Prompt for new details
            author.FirstName = uiHelper.ReadStringInput($"Enter new first name:");
            author.LastName = uiHelper.ReadStringInput($"Enter new last name:");
            author.DateOfBirth = DateTime.Parse(uiHelper.ReadStringInput($"Enter new date of birth:"));
            author.ImageUrl = uiHelper.ReadStringInput($"Enter new image URL:");
            author.Biography = uiHelper.ReadStringInput($"Enter new biography:");
            await authorBusiness.UpdateAsync(author); // Save updated author details

            Console.WriteLine("Author updated successfully.");
        }

        /// <summary>
        /// Fetches and displays an author's details by their ID, including the books they have authored.
        /// </summary>
        private async Task FetchAuthorById()
        {
            var authorId = uiHelper.ReadIntInput("Enter Author ID to fetch:"); // Prompt for author ID
            var author = await authorBusiness.GetWithIncludesAsync(authorId); // Fetch author by ID with related data
            if (author == null)
            {
                Console.WriteLine("Author not found."); // Handle case where author doesn't exist
                return;
            }
            // Display author details along with their books
            Console.WriteLine($"ID: {author.Id}\nName: {author.FirstName} {author.LastName}\nDate of birth:{author.DateOfBirth.ToShortDateString()}\nBio:\n{author.Biography}\nBooks:");
            foreach (var book in author.Books)
            {
                // Display book details, indicating if it's rented out or available
                if (book.BorrowedBooks.Any(bb => bb.ReturnDate == null))
                {
                    Console.WriteLine($"Book ID: {book.Id}, Title: {book.Title}, Genre: {book.Genre}, Published Year: {book.PublicationDate.ToShortDateString}, Status: Rented out");
                }
                else
                {
                    Console.WriteLine($"Book ID: {book.Id}, Title: {book.Title}, Genre: {book.Genre}, Published Year: {book.PublicationDate.ToShortDateString}, Status: Available");
                }
            }
        }

        /// <summary>
        /// Prompts the user to delete an author by their ID.
        /// </summary>
        private async Task DeleteAuthor()
        {
            var authorId = uiHelper.ReadIntInput("Enter Author ID to delete:"); // Prompt for author ID
            var author = await authorBusiness.GetAsync(authorId); // Fetch author by ID
            if (author == null)
            {
                Console.WriteLine("Author not found."); // Handle case where author doesn't exist
                return;
            }
            await authorBusiness.DeleteAsync(authorId); // Delete the author
            Console.WriteLine("Author deleted successfully.");
        }
    }
}
