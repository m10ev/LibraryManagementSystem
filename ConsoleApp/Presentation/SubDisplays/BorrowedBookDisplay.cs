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
        private AuthorBusiness authorBusiness = new AuthorBusiness();
        private BookBusiness bookBusiness = new BookBusiness();
        private MemberBusiness memberBusiness = new MemberBusiness();
        private BorrowedBookBusiness borrowedBookBusiness = new BorrowedBookBusiness();

        // UI helper to assist with common input/output operations
        private UIHelper uiHelper = new UIHelper();

        public void BorrowedBookManager()
        {
            ShowBorrowedBookMenu();
            var operation = uiHelper.ReadIntInput("Please select an option:");
            switch (operation)
            {
                case 1:
                    ShowAllBorrowedBooks();
                    break;
                case 2:
                    ShowAllCurrentlyBorrowedBooks();
                    break;
                case 3:
                    UpdateBorrowedBook();
                    break;
                case 4:
                    FetchBorrowedBookByIdAndDate();
                    break;
                case 5:
                    DeleteBorrowedBook();
                    break;
                default:
                    Console.WriteLine("Please select a valid option.");
                    break;
            }
        }

        private void ShowBorrowedBookMenu()
        {
            uiHelper.ShowHeader("Borrowed Books History Management");
            Console.WriteLine("1. All Borrowed Books History");
            Console.WriteLine("2. All Currently Borrowed Books");
            Console.WriteLine("3. Update Borrowed Book");
            Console.WriteLine("4. Fetch Borrowed Book by ID and Date Borrowed");
            Console.WriteLine("5. Delete Borrowed Book");
        }

        private void ShowDeleteBorrowedBookHystoryMenu()
        {
            uiHelper.ShowHeader("Borrowed Books History Management");
            Console.WriteLine("1. Delete Most Recent Hystory of Borrowed Book by Id");
            Console.WriteLine("2. Delete Borrowed Book History by Id and Date");
            Console.WriteLine("3. Delete Between Dates");
        }

        private void ShowAllBorrowedBooks()
        {
            var borrowedBooks = borrowedBookBusiness.GetAll();
            if (borrowedBooks.Count == 0)
            {
                Console.WriteLine("No borrowed books found.");
                return;
            }
            foreach (var borrowedBook in borrowedBooks)
            {
                if (borrowedBook.ReturnDate == null)
                {
                    Console.WriteLine($"Book ID: {borrowedBook.Book.Id}, Book Title: {borrowedBook.Book.Title}, Member Name: {borrowedBook.Member.FirstName} {borrowedBook.Member.LastName}, Borrow Date: {borrowedBook.BorrowDate}, Due Date: {borrowedBook.ReturnDate}");
                }
                else
                {
                    Console.WriteLine($"Book ID: {borrowedBook.Book.Id}, Book Title: {borrowedBook.Book.Title}, Member Name: {borrowedBook.Member.FirstName} {borrowedBook.Member.LastName}, Borrow Date: {borrowedBook.BorrowDate}, Return Date: {borrowedBook.ReturnDate}");
                }
            }
        }

        private void ShowAllCurrentlyBorrowedBooks()
        {
            var borrowedBooks = borrowedBookBusiness.GetAll();
            if (borrowedBooks.Any(bb => bb.ReturnDate == null))
            {
                Console.WriteLine("No currently borrowed books found.");
                return;
            }
            borrowedBooks = borrowedBooks.Where(bb => bb.ReturnDate == null).ToList();
            foreach (var borrowedBook in borrowedBooks)
            {
                Console.WriteLine($"Book ID: {borrowedBook.Book.Id}, Book Title: {borrowedBook.Book.Title}, Member Name: {borrowedBook.Member.FirstName} {borrowedBook.Member.LastName}, Borrow Date: {borrowedBook.BorrowDate}, Due Date: {borrowedBook.ReturnDate}");
            }
        }

        private void UpdateBorrowedBook()
        {
            var borrowedBookId = uiHelper.ReadIntInput("Enter the ID of the borrowed book to update:");
            var borrowedBook = borrowedBookBusiness.GetByBookId(borrowedBookId);
            if (borrowedBook == null)
            {
                Console.WriteLine("Borrowed book not found.");
                return;
            }
            var newDueDate = DateTime.Parse(uiHelper.ReadStringInput("Enter new Due date (yyyy-mm-dd):"));
            borrowedBook.DueDate = newDueDate;
            borrowedBookBusiness.Update(borrowedBook);
            Console.WriteLine("Borrowed book updated successfully.");
        }

        private void FetchBorrowedBookByIdAndDate()
        {
            var borrowedBookId = uiHelper.ReadIntInput("Enter the ID of the borrowed book to fetch:");
            var borrowedBookDate = DateTime.Parse(uiHelper.ReadStringInput("Enter the Borrow date (yyyy-mm-dd):"));
            var borrowedBook = borrowedBookBusiness.GetByBookIdAndDate(borrowedBookId, borrowedBookDate);
            if (borrowedBook == null)
            {
                Console.WriteLine("Borrowed book not found.");
                return;
            }
            if (borrowedBook.ReturnDate == null)
            {
                Console.WriteLine($"Book ID: {borrowedBook.Book.Id}, Book Title: {borrowedBook.Book.Title}, Member Name: {borrowedBook.Member.FirstName} {borrowedBook.Member.LastName}, Borrow Date: {borrowedBook.BorrowDate}, Due Date: {borrowedBook.DueDate}");
            }
            else
            {
                Console.WriteLine($"Book ID: {borrowedBook.Book.Id}, Book Title: {borrowedBook.Book.Title}, Member Name: {borrowedBook.Member.FirstName} {borrowedBook.Member.LastName}, Borrow Date: {borrowedBook.BorrowDate}, Return Date: {borrowedBook.ReturnDate}");
            }
        }

        private void DeleteBorrowedBook()
        {
            ShowDeleteBorrowedBookHystoryMenu();
            var operation = uiHelper.ReadIntInput("Please select an option:");
            switch (operation)
            {
                case 1:
                    DeleteMostRecentBorrowedBookById();
                    break;
                case 2:
                    DeleteBorrowedBookByIdAndDate();
                    break;
                case 3:
                    DeleteBetweenDates();
                    break;
                default:
                    Console.WriteLine("Please select a valid option.");
                    break;
            }
        }

        private void DeleteMostRecentBorrowedBookById()
        {
            var borrowedBookId = uiHelper.ReadIntInput("Enter the ID of the borrowed book to delete:");
            var borrowedBook = borrowedBookBusiness.GetByBookId(borrowedBookId);
            if (borrowedBook == null)
            {
                Console.WriteLine("Borrowed book not found.");
                return;
            }
            borrowedBookBusiness.Delete(borrowedBookId);
            Console.WriteLine("Borrowed book deleted successfully.");
        }

        private void DeleteBorrowedBookByIdAndDate()
        {
            var borrowedBookId = uiHelper.ReadIntInput("Enter the ID of the borrowed book to delete:");
            var borrowedBookDate = DateTime.Parse(uiHelper.ReadStringInput("Enter the Borrow date (yyyy-mm-dd):"));
            var borrowedBook = borrowedBookBusiness.GetByBookIdAndDate(borrowedBookId, borrowedBookDate);
            if (borrowedBook == null)
            {
                Console.WriteLine("Borrowed book not found.");
                return;
            }
            borrowedBookBusiness.DeleteByIdAndDate(borrowedBookId, borrowedBookDate);
            Console.WriteLine("Borrowed book deleted successfully.");
        }

        private void DeleteBetweenDates()
        {
            var startDate = DateTime.Parse(uiHelper.ReadStringInput("Enter the start date (yyyy-mm-dd):"));
            var endDate = DateTime.Parse(uiHelper.ReadStringInput("Enter the end date (yyyy-mm-dd):"));
            var borrowedBooks = borrowedBookBusiness.GetAllBetweenDates(startDate, endDate);
            if (borrowedBooks.Count == 0)
            {
                Console.WriteLine("No borrowed books found between the specified dates.");
                return;
            }
            foreach (var borrowedBook in borrowedBooks)
            {
                borrowedBookBusiness.DeleteByIdAndDate(borrowedBook.BookID, borrowedBook.BorrowDate);
            }
        }
    }
}