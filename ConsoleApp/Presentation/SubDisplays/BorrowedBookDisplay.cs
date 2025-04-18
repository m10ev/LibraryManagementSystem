using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Presentation.SubDisplays
{
    internal class BorrowedBookDisplay
    {
        // Business layer objects to interact with data models
        private readonly AuthorBusiness authorBusiness = new AuthorBusiness();
        private readonly BookBusiness bookBusiness = new BookBusiness();
        private readonly MemberBusiness memberBusiness = new MemberBusiness();
        private readonly BorrowedBookBusiness borrowedBookBusiness = new BorrowedBookBusiness();

        // UI helper to assist with common input/output operations
        private readonly UIHelper uiHelper = new UIHelper();

        /// <summary>
        /// Main function to manage borrowed book-related operations.
        /// Displays a menu with options like showing, updating, fetching, and deleting borrowed books.
        /// </summary>
        public async Task BorrowedBookManager()
        {
            ShowBorrowedBookMenu(); // Display the borrowed book management menu
            var operation = uiHelper.ReadIntInput("Please select an option:");

            // Execute the selected operation based on user's choice
            switch (operation)
            {
                case 1:
                    await ShowAllBorrowedBooks(); // Display all borrowed books
                    break;
                case 2:
                    await ShowAllCurrentlyBorrowedBooks(); // Display all currently borrowed books
                    break;
                case 3:
                    await UpdateBorrowedBook(); // Update details of an existing borrowed book
                    break;
                case 4:
                    await FetchBorrowedBookByIdAndDate(); // Fetch a borrowed book by ID and borrow date
                    break;
                case 5:
                    await DeleteBorrowedBook(); // Delete a borrowed book record
                    break;
                default:
                    Console.WriteLine("Please select a valid option."); // Handle invalid options
                    break;
            }
        }

        /// <summary>
        /// Displays the menu for managing borrowed book operations.
        /// </summary>
        private void ShowBorrowedBookMenu()
        {
            uiHelper.ShowHeader("Borrowed Books History Management");
            Console.WriteLine("1. All Borrowed Books History");
            Console.WriteLine("2. All Currently Borrowed Books");
            Console.WriteLine("3. Update Borrowed Book");
            Console.WriteLine("4. Fetch Borrowed Book by ID and Date Borrowed");
            Console.WriteLine("5. Delete Borrowed Book");
        }

        /// <summary>
        /// Displays the menu for deleting borrowed book history records.
        /// </summary>
        private void ShowDeleteBorrowedBookHystoryMenu()
        {
            uiHelper.ShowHeader("Borrowed Books History Management");
            Console.WriteLine("1. Delete Most Recent History of Borrowed Book by Id");
            Console.WriteLine("2. Delete Borrowed Book History by Id and Date");
            Console.WriteLine("3. Delete Between Dates");
        }

        /// <summary>
        /// Displays all borrowed books, including return dates if available.
        /// </summary>
        private async Task ShowAllBorrowedBooks()
        {
            var borrowedBooks = await borrowedBookBusiness.GetAllAsync(); // Retrieve all borrowed books
            if (borrowedBooks.Count == 0)
            {
                Console.WriteLine("No borrowed books found."); // Handle case with no borrowed books
                return;
            }
            // Display each borrowed book's details
            foreach (var borrowedBook in borrowedBooks)
            {
                if (borrowedBook.ReturnDate == null)
                {
                    Console.WriteLine($"Book ID: {borrowedBook.Book.Id}, Book Title: {borrowedBook.Book.Title}, Member Name: {borrowedBook.Member.FirstName} {borrowedBook.Member.LastName}, Borrow Date: {borrowedBook.BorrowDate:yyyy-MM-dd}, Due Date: {borrowedBook.ReturnDate:yyyy-MM-dd}");
                }
                else
                {
                    Console.WriteLine($"Book ID: {borrowedBook.Book.Id}, Book Title: {borrowedBook.Book.Title}, Member Name: {borrowedBook.Member.FirstName} {borrowedBook.Member.LastName}, Borrow Date: {borrowedBook.BorrowDate:yyyy-MM-dd}, Return Date: {borrowedBook.ReturnDate:yyyy-MM-dd}");
                }
            }
        }

        /// <summary>
        /// Displays all currently borrowed books, i.e., those without a return date.
        /// </summary>
        private async Task ShowAllCurrentlyBorrowedBooks()
        {
            var borrowedBooks = await borrowedBookBusiness.GetAllAsync(); // Retrieve all borrowed books
            if (borrowedBooks.Any(bb => bb.ReturnDate == null)) // Check for books that are currently borrowed
            {
                Console.WriteLine("No currently borrowed books found."); // Handle case with no currently borrowed books
                return;
            }
            borrowedBooks = borrowedBooks.Where(bb => bb.ReturnDate == null).ToList(); // Filter out books that have not been returned
            // Display each currently borrowed book's details
            foreach (var borrowedBook in borrowedBooks)
            {
                Console.WriteLine($"Book ID: {borrowedBook.Book.Id}, Book Title: {borrowedBook.Book.Title}, Member Name: {borrowedBook.Member.FirstName} {borrowedBook.Member.LastName}, Borrow Date: {borrowedBook.BorrowDate:yyyy-MM-dd}, Due Date: {borrowedBook.ReturnDate:yyyy-MM-dd}");
            }
        }

        /// <summary>
        /// Allows the user to update the due date of a borrowed book.
        /// </summary>
        private async Task UpdateBorrowedBook()
        {
            var borrowedBookId = uiHelper.ReadIntInput("Enter the ID of the borrowed book to update:"); // Prompt for borrowed book ID
            var borrowedBookDate = uiHelper.ReadDateInput("Enter the Borrow date:"); // Prompt for borrow date
            var borrowedBook = await borrowedBookBusiness.GetByBookIdAndDateAsync(borrowedBookId, borrowedBookDate); // Fetch borrowed book by ID
            if (borrowedBook == null)
            {
                Console.WriteLine("Borrowed book not found."); // Handle case where borrowed book doesn't exist
                return;
            }
            var newDueDate = uiHelper.ReadDateInput("Enter new Due date:"); // Prompt for new due date
            borrowedBook.DueDate = newDueDate; // Update the due date
            await borrowedBookBusiness.UpdateAsync(borrowedBook); // Save the updated borrowed book details
            Console.WriteLine("Borrowed book updated successfully.");
        }

        /// <summary>
        /// Fetches and displays a borrowed book's details by its ID and borrow date.
        /// </summary>
        private async Task FetchBorrowedBookByIdAndDate()
        {
            var borrowedBookId = uiHelper.ReadIntInput("Enter the ID of the borrowed book to fetch:"); // Prompt for borrowed book ID
            var borrowedBookDate = uiHelper.ReadDateInput("Enter the Borrow date:"); // Prompt for borrow date
            var borrowedBook = await borrowedBookBusiness.GetByBookIdAndDateAsync(borrowedBookId, borrowedBookDate); // Fetch borrowed book by ID and date
            if (borrowedBook == null)
            {
                Console.WriteLine("Borrowed book not found."); // Handle case where borrowed book doesn't exist
                return;
            }
            // Display borrowed book details based on return date status
            if (borrowedBook.ReturnDate == null)
            {
                Console.WriteLine($"Book ID: {borrowedBook.Book.Id}, Book Title: {borrowedBook.Book.Title}, Member Name: {borrowedBook.Member.FirstName} {borrowedBook.Member.LastName}, Borrow Date: {borrowedBook.BorrowDate:yyyy-MM-dd}, Due Date: {borrowedBook.DueDate:yyyy-MM-dd}");
            }
            else
            {
                Console.WriteLine($"Book ID: {borrowedBook.Book.Id}, Book Title: {borrowedBook.Book.Title}, Member Name: {borrowedBook.Member.FirstName} {borrowedBook.Member.LastName}, Borrow Date: {borrowedBook.BorrowDate:yyyy-MM-dd}, Return Date: {borrowedBook.ReturnDate:yyyy-MM-dd}");
            }
        }

        /// <summary>
        /// Allows the user to delete a borrowed book record.
        /// </summary>
        private async Task DeleteBorrowedBook()
        {
            ShowDeleteBorrowedBookHystoryMenu(); // Display delete options menu
            var operation = uiHelper.ReadIntInput("Please select an option:"); // Prompt for the delete operation choice
            switch (operation)
            {
                case 1:
                    await DeleteMostRecentBorrowedBookById(); // Delete most recent borrowed book by ID
                    break;
                case 2:
                    await DeleteBorrowedBookByIdAndDate(); // Delete borrowed book by ID and date
                    break;
                case 3:
                    await DeleteBetweenDates(); // Delete borrowed books between specific dates
                    break;
                default:
                    Console.WriteLine("Please select a valid option."); // Handle invalid options
                    break;
            }
        }

        /// <summary>
        /// Deletes the most recent borrowed book record by its ID.
        /// </summary>
        private async Task DeleteMostRecentBorrowedBookById()
        {
            var borrowedBookId = uiHelper.ReadIntInput("Enter the ID of the borrowed book to delete:"); // Prompt for borrowed book ID
            var borrowedBook = await borrowedBookBusiness.GetByBookIdAsync(borrowedBookId); // Fetch borrowed book by ID
            if (borrowedBook == null)
            {
                Console.WriteLine("Borrowed book not found."); // Handle case where borrowed book doesn't exist
                return;
            }
            await borrowedBookBusiness.DeleteAsync(borrowedBookId); // Delete the borrowed book
            Console.WriteLine("Borrowed book deleted successfully.");
        }

        /// <summary>
        /// Deletes a borrowed book record by its ID and borrow date.
        /// </summary>
        private async Task DeleteBorrowedBookByIdAndDate()
        {
            var borrowedBookId = uiHelper.ReadIntInput("Enter the ID of the borrowed book to delete:"); // Prompt for borrowed book ID
            var borrowedBookDate = uiHelper.ReadDateInput("Enter the Borrow date:"); // Prompt for borrow date
            var borrowedBook = await borrowedBookBusiness.GetByBookIdAndDateAsync(borrowedBookId, borrowedBookDate); // Fetch borrowed book by ID and date
            if (borrowedBook == null)
            {
                Console.WriteLine("Borrowed book not found."); // Handle case where borrowed book doesn't exist
                return;
            }
            await borrowedBookBusiness.DeleteByIdAndDateAsync(borrowedBookId, borrowedBookDate); // Delete the borrowed book by ID and date
            Console.WriteLine("Borrowed book deleted successfully.");
        }

        /// <summary>
        /// Deletes all borrowed books between two specific dates.
        /// </summary>
        private async Task DeleteBetweenDates()
        {
            var startDate = uiHelper.ReadDateInput("Enter the start date:"); // Prompt for start date
            var endDate = uiHelper.ReadDateInput("Enter the end date:"); // Prompt for end date
            var borrowedBooks = await borrowedBookBusiness.GetAllBetweenDatesAsync(startDate, endDate); // Fetch borrowed books between the given dates
            if (borrowedBooks.Count == 0)
            {
                Console.WriteLine("No borrowed books found between the specified dates."); // Handle case where no books are found
                return;
            }
            // Delete each borrowed book between the specified dates
            foreach (var borrowedBook in borrowedBooks)
            {
                await borrowedBookBusiness.DeleteByIdAndDateAsync(borrowedBook.BookID, borrowedBook.BorrowDate);
            }
        }
    }
}
