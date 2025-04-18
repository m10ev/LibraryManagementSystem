using ConsoleApp.Presentation.SubDisplays;


namespace ConsoleApp.Presentation
{
    internal class Display
    {
        // Creating instances of sub-displays for each domain (Book, Member, Author, Borrowed Book)
        private readonly AuthorDisplay authorDisplay = new AuthorDisplay();
        private readonly BookDisplay bookDisplay = new BookDisplay();
        private readonly BorrowedBookDisplay borrowedBookDisplay = new BorrowedBookDisplay();
        private readonly MemberDisplay memberDisplay = new MemberDisplay();

        // UI helper to assist with common input/output operations
        private readonly UIHelper uiHelper = new UIHelper();

        /// <summary>
        /// The entry point for starting the display interaction.
        /// </summary>
        public static async Task OnStart()
        {
            var display = new Display();
            await display.Input();
        }

        // Private constructor to prevent instantiation outside the class
        private Display()
        {
        }

        /// <summary>
        /// Displays the main menu options for the library management system.
        /// </summary>
        public void ShowMenu()
        {
            uiHelper.ShowHeader("Library Management System");
            Console.WriteLine("1. Books");
            Console.WriteLine("2. Members");
            Console.WriteLine("3. Authors");
            Console.WriteLine("4. Borrowed Books History");
            Console.WriteLine("5. Exit");
        }

        /// <summary>
        /// Handles the user input and navigates to the appropriate display based on selection.
        /// </summary>
        private async Task Input()
        {
            var operation = -1;
            do
            {
                ShowMenu(); // Show the menu options
                operation = uiHelper.ReadIntInput("Please select an option:"); // Read user input

                // Switch case for different menu options
                switch (operation)
                {
                    case 1:
                        await bookDisplay.BookManager(); // Navigate to book manager
                        break;
                    case 2:
                        await memberDisplay.MemberManager(); // Navigate to member manager
                        break;
                    case 3:
                        await authorDisplay.AuthorManager(); // Navigate to author manager
                        break;
                    case 4:
                        await borrowedBookDisplay.BorrowedBookManager(); // Navigate to borrowed book manager
                        break;
                    case 5:
                        Console.WriteLine("Exiting..."); // Inform the user that the program is exiting
                        break;
                    default:
                        Console.WriteLine("Invalid option selected, please try again."); // Handle invalid options
                        break;
                }
            } while (operation != 5); // Continue the loop until the user chooses to exit
        }
    }
}