using Business;
using Data.Models;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ConsoleApp.Presentation.SubDisplays
{
    internal class BookDisplay
    {
        // Business layer objects to interact with data models
        private readonly AuthorBusiness authorBusiness = new AuthorBusiness();
        private readonly BookBusiness bookBusiness = new BookBusiness();
        private readonly MemberBusiness memberBusiness = new MemberBusiness();
        private readonly BorrowedBookBusiness borrowedBookBusiness = new BorrowedBookBusiness();

        // UI helper to assist with common input/output operations
        private readonly UIHelper uiHelper = new UIHelper();

        /// <summary>
        /// Main function to manage book-related operations.
        /// Provides a menu for the user to choose actions like borrowing, browsing, adding, updating, or deleting books.
        /// </summary>
        public async Task BookManager()
        {
            ShowBookMenu(); // Display the book management menu
            var operation = uiHelper.ReadIntInput("Please select an option:");

            // Execute the selected operation based on user's input
            switch (operation)
            {
                case 1:
                    await Borrow(); // Borrow a book
                    break;
                case 2:
                    await Browse(); // Browse available books
                    break;
                case 3:
                    await AddBook(); // Add a new book to the system
                    break;
                case 4:
                    await UpdateBook(); // Update book details
                    break;
                case 5:
                    await DeleteBook(); // Delete a book from the system
                    break;
                default:
                    Console.WriteLine("Invalid option."); // Invalid choice handling
                    break;
            }
        }

        // Menus for book management and browsing
        /// <summary>
        /// Displays the main menu for book management.
        /// </summary>
        private void ShowBookMenu()
        {
            uiHelper.ShowHeader("Book Management");
            Console.WriteLine("1. Borrow a Book");
            Console.WriteLine("2. Browse Books");
            Console.WriteLine("3. Add books");
            Console.WriteLine("4. Update books");
            Console.WriteLine("5. Delete books");
        }

        /// <summary>
        /// Displays the menu for browsing books by different criteria.
        /// </summary>
        private void ShowBrowseBooks()
        {
            uiHelper.ShowHeader("Browse Books");
            Console.WriteLine("1. All Books");
            Console.WriteLine("2. By Genre");
            Console.WriteLine("3. Fetch by ID");
            Console.WriteLine("4. Fetch by ISBN");
        }

        // Main functions to perform actions on books
        /// <summary>
        /// Allows the user to borrow a book. First prompts the user to browse books.
        /// </summary>
        private async Task Borrow()
        {
            ShowBrowseBooks(); // Show browsing options
            var operation = uiHelper.ReadIntInput("Please select an option:");

            // Handle different borrow scenarios based on user's choice
            switch (operation)
            {
                case 1:
                    await PrintAllBooks(); // Display all books
                    await BorrowById(); // Borrow by ID
                    break;
                case 2:
                    await PrintByGenre(); // Display books by genre
                    await BorrowById(); // Borrow by ID
                    break;
                case 3:
                    await BorrowById(); // Directly borrow by ID
                    break;
                case 4:
                    await BorrowByISBN(); // Borrow by ISBN
                    break;
                default:
                    Console.WriteLine("Invalid option."); // Invalid choice handling
                    break;
            }
        }

        /// <summary>
        /// Allows the user to browse books. Provides different browsing criteria.
        /// </summary>
        private async Task Browse()
        {
            ShowBrowseBooks(); // Show browsing options
            var operation = uiHelper.ReadIntInput("Please select an option:");

            // Handle different browsing scenarios based on user's choice
            switch (operation)
            {
                case 1:
                    await PrintAllBooks(); // Display all books
                    break;
                case 2:
                    await PrintByGenre(); // Display books by genre
                    break;
                case 3:
                    await PrintById(); // Display books by ID
                    break;
                case 4:
                    await PrintByISBN(); // Display books by ISBN
                    break;
                default:
                    Console.WriteLine("Invalid option."); // Invalid choice handling
                    break;
            }
        }

        /// <summary>
        /// Allows the user to add a new book to the system.
        /// Prompts the user for book details and validates the genre.
        /// </summary>
        private async Task AddBook()
        {
            var book = new Book();
            book.Title = uiHelper.ReadStringInput("Please enter the book title:");
            string genre = uiHelper.ReadStringInput("Please enter the book genre:").ToLower();

            // Validate and assign the genre based on user's input
            switch (genre)
            {
                case "fiction":
                    book.Genre = Genre.Fiction;
                    break;
                case "non - fiction":
                    book.Genre = Genre.NonFiction;
                    break;
                case "fantasy":
                    book.Genre = Genre.Fantasy;
                    break;
                case "mystery":
                    book.Genre = Genre.Mystery;
                    break;
                case "romance":
                    book.Genre = Genre.Romance;
                    break;
                case "science fiction":
                    book.Genre = Genre.ScienceFiction;
                    break;
                case "biography":
                    book.Genre = Genre.Biography;
                    break;
                case "thriller":
                    book.Genre = Genre.Thriller;
                    break;
                case "horror":
                    book.Genre = Genre.Horror;
                    break;
                case "historical fiction":
                    book.Genre = Genre.HistoricalFiction;
                    break;
                case "health and wellness":
                    book.Genre = Genre.HealthAndWellness;
                    break;
                case "travel":
                    book.Genre = Genre.Travel;
                    break;
                case "children literature":
                    book.Genre = Genre.ChildrenLiterature;
                    break;
                default:
                    Console.WriteLine($"Genre - {genre} doesn't exist");
                    return; // Return if the genre is invalid
            }

            // Input and assign other book details
            book.PublicationDate = DateTime.Parse(uiHelper.ReadStringInput("Please enter the book published year (yyyy-mm-dd):"));
            book.ISBN = uiHelper.ReadStringInput("Please enter the book ISBN:");
            var authorId = uiHelper.ReadIntInput("Please enter the author ID:");
            var author = await authorBusiness.GetAsync(authorId);

            if (author != null)
            {
                book.AuthorID = author.Id; // Assign the author to the book
                await bookBusiness.AddAsync(book); // Add the book to the system
                Console.WriteLine($"Book {book.Title} added successfully.");
            }
            else
            {
                Console.WriteLine("Author not found.");
            }
        }

        /// <summary>
        /// Allows the user to update an existing book's details.
        /// Prompts the user for new values and validates them.
        /// </summary>
        private async Task UpdateBook()
        {
            var bookId = uiHelper.ReadIntInput("Please enter the book ID:");
            var book = await bookBusiness.GetAsync(bookId);

            if (book != null)
            {
                book.Title = uiHelper.ReadStringInput("Please enter the new book title:");
                string genre = uiHelper.ReadStringInput("Please enter the new book genre:").ToLower();

                // Validate and assign the new genre
                switch (genre)
                {
                    case "fiction":
                        book.Genre = Genre.Fiction;
                        break;
                    case "non - fiction":
                        book.Genre = Genre.NonFiction;
                        break;
                    // Other genre cases...
                    default:
                        Console.WriteLine($"Genre - {genre} doesn't exist");
                        return;
                }

                // Input new ISBN and author details
                book.PublicationDate = DateTime.Parse(uiHelper.ReadStringInput("Please enter the book published year (yyyy-mm-dd):"));
                book.ISBN = uiHelper.ReadStringInput("Please enter the new book ISBN:");
                var authorId = uiHelper.ReadIntInput("Please enter the new author ID:");
                var author = await authorBusiness.GetAsync(authorId);

                if (author != null)
                {
                    book.AuthorID = author.Id; // Assign new author to book
                    await bookBusiness.AddAsync(book); // Update book details in the system
                    Console.WriteLine($"Book {book.Title} updated successfully.");
                }
                else
                {
                    Console.WriteLine("Author not found.");
                }
            }
            else
            {
                Console.WriteLine($"Book with ID - {bookId} doesn't exist");
            }
        }

        /// <summary>
        /// Allows the user to delete a book from the system.
        /// Prompts the user for the book ID and removes the book if found.
        /// </summary>
        private async Task DeleteBook()
        {
            var bookId = uiHelper.ReadIntInput("Please enter the book ID:");
            var book = await bookBusiness.GetAsync(bookId);

            if (book != null)
            {
                await bookBusiness.DeleteAsync(book.Id); // Delete the book from the system
                Console.WriteLine($"Book {book.Title} deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Book with ID - {bookId} doesn't exist");
            }
        }

        // Print functions to display books in various formats
        /// <summary>
        /// Prints all books in the system.
        /// </summary>
        private async Task PrintAllBooks()
        {
            var books = await bookBusiness.GetAllWithIncludesAsync();
            foreach (var book in books)
            {
                Console.WriteLine($"ID: {book.Id}, Title: {book.Title}, Genre: {book.Genre}, Author: {book.Author.FirstName} {book.Author.LastName}, Release Date: {book.PublicationDate.ToShortDateString()}, ISBN: {book.ISBN}");
            }
        }

        /// <summary>
        /// Prints books filtered by genre.
        /// </summary>
        private async Task PrintByGenre()
        {
            var genre = uiHelper.ReadStringInput("Please enter the genre:");
            var filteredBooks = await bookBusiness.GetAllByGenreWithIncludesAsync(genre);

            if (filteredBooks != null && filteredBooks.Count > 0)
            {
                foreach (var book in filteredBooks)
                {
                    Console.WriteLine($"ID: {book.Id}, Title: {book.Title}, Author: {book.Author.FirstName} {book.Author.LastName}, Release Date: {book.PublicationDate.ToShortDateString()}, ISBN: {book.ISBN}");
                }
            }
            else
            {
                Console.WriteLine($"Genre - {genre} doesn't exist or no books found.");
            }
        }

        /// <summary>
        /// Prints a book found by its ID.
        /// </summary>
        private async Task PrintById()
        {
            var bookId = uiHelper.ReadIntInput("Please enter the book ID:");
            var book = await bookBusiness.GetWithIncludesAsync(bookId);

            if (book != null)
            {
                Console.WriteLine($"ID: {book.Id}, Title: {book.Title}, Genre: {book.Genre}, Author: {book.Author.FirstName} {book.Author.LastName}, Release Date: {book.PublicationDate.ToShortDateString()}, ISBN: {book.ISBN}");
            }
            else
            {
                Console.WriteLine($"Book with ID - {bookId} doesn't exist");
            }
        }

        /// <summary>
        /// Prints a book found by its ISBN.
        /// </summary>
        private async Task PrintByISBN()
        {
            var bookISBN = uiHelper.ReadStringInput("Please enter the book ISBN:");
            var book = await bookBusiness.GetByISBNWithIncludesAsync(bookISBN);

            if (book != null)
            {
                Console.WriteLine($"ID: {book.Id}, Title: {book.Title}, Genre: {book.Genre}, Author: {book.Author.FirstName} {book.Author.LastName}, Release Date: {book.PublicationDate.ToShortDateString()}, ISBN: {book.ISBN}");
            }
            else
            {
                Console.WriteLine($"Book with ISBN - {bookISBN} doesn't exist");
            }
        }

        // Borrow functions for borrowing books by different criteria
        /// <summary>
        /// Allows the user to borrow a book by its ID.
        /// </summary>
        private async Task BorrowById()
        {
            var bookId = uiHelper.ReadIntInput("Please select a book by ID:");
            var book = await bookBusiness.GetWithIncludesAsync(bookId);

            if (book != null)
            {
                Console.WriteLine($"You selected: {book.Title}");
                var memberId = uiHelper.ReadIntInput("Please enter your member ID:");
                var member = await memberBusiness.GetAsync(memberId);

                if (member != null)
                {
                    if (book.BorrowedBooks.Any(bb => bb.ReturnDate == null))
                    {
                        Console.WriteLine($"Book {book.Title} with ID: {book.Id} is already borrowed.");
                        return; // Book is already borrowed, exit the method
                    }
                    // Create a borrowed book record
                    var borrowedBook = new BorrowedBook
                    {
                        BookID = book.Id,
                        MemberID = member.Id,
                        BorrowDate = DateTime.Now, // Current date
                        DueDate = DateTime.Now.AddMonths(2) // Assuming a 2-month borrowing period
                    };
                    await borrowedBookBusiness.AddAsync(borrowedBook); // Save the borrowed book
                    Console.WriteLine($"You have successfully borrowed {book.Title}.");
                }
                else
                {
                    Console.WriteLine("Member not found.");
                }
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }

        /// <summary>
        /// Allows the user to borrow a book by its ISBN.
        /// </summary>
        private async Task BorrowByISBN()
        {
            string bookISBN = uiHelper.ReadStringInput("Please enter the book ISBN:");
            var book = await bookBusiness.GetByISBNWithIncludesAsync(bookISBN);

            if (book != null)
            {
                Console.WriteLine($"You selected: {book.Title}");
                var memberId = uiHelper.ReadIntInput("Please enter your member ID:");
                var member = await memberBusiness.GetAsync(memberId);

                if (member != null)
                {
                    if (book.BorrowedBooks.Any(bb => bb.ReturnDate == null))
                    {
                        Console.WriteLine($"Book {book.Title} with ID: {book.Id} is already borrowed.");
                        return; // Book is already borrowed, exit the method
                    }
                    var borrowedBook = new BorrowedBook
                    {
                        BookID = book.Id,
                        MemberID = member.Id,
                        BorrowDate = DateTime.Now
                    };
                    await borrowedBookBusiness.AddAsync(borrowedBook); // Add the borrowed book record
                    Console.WriteLine($"You have successfully borrowed {book.Title}.");
                }
                else
                {
                    Console.WriteLine("Member not found.");
                }
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }
    }
}