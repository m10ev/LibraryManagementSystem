using Business;
using Data.Models;

namespace ConsoleApp.Presentation.SubDisplays
{
    internal class MemberDisplay
    {
        // Business layer objects to interact with data models
        private readonly BookBusiness bookBusiness = new BookBusiness();
        private readonly MemberBusiness memberBusiness = new MemberBusiness();

        // UI helper to assist with common input/output operations
        private readonly UIHelper uiHelper = new UIHelper();

        /// <summary>
        /// Main function to manage member-related operations.
        /// Displays a menu with options like adding, updating, deleting, or viewing members.
        /// </summary>
        public async Task MemberManager()
        {
            ShowMemberMenu(); // Display the member management menu
            var operation = uiHelper.ReadIntInput("Please select an option:");

            // Execute the selected operation based on user's choice
            switch (operation)
            {
                case 1:
                    await ShowAllMembers(); // Display all members
                    break;
                case 2:
                    await AddMember(); // Add a new member
                    break;
                case 3:
                    await RenewMembership(); // Renew membership for an existing member
                    break;
                case 4:
                    await UpdateMember(); // Update member details
                    break;
                case 5:
                    await FetchMemberById(); // Fetch a member by their ID
                    break;
                case 6:
                    await DeleteMember(); // Delete a member from the system
                    break;
                default:
                    Console.WriteLine("Please select a valid option."); // Handle invalid options
                    break;
            }
        }

        /// <summary>
        /// Displays the main menu for member management.
        /// </summary>
        private void ShowMemberMenu()
        {
            uiHelper.ShowHeader("Member Management");
            Console.WriteLine("1. All Members");
            Console.WriteLine("2. Add Member");
            Console.WriteLine("3. Renew Membership");
            Console.WriteLine("4. Update Member");
            Console.WriteLine("5. Fetch Member by ID");
            Console.WriteLine("6. Delete Member");
        }

        /// <summary>
        /// Displays a list of all members in the system.
        /// </summary>
        private async Task ShowAllMembers()
        {
            var members = await memberBusiness.GetAllAsync(); // Retrieve all members
            Console.WriteLine("List of Members:");
            foreach (var member in members)
            {
                // Display member details
                Console.WriteLine($"ID: {member.Id}, Name: {member.FirstName} {member.LastName}, Membership Expire Date: {member.MembershipExpireDate:yyyy-MM-dd} Phone Number: {member.PhoneNumber}");
            }
        }

        /// <summary>
        /// Prompts the user to input member details and adds the new member to the system.
        /// </summary>
        private async Task AddMember()
        {
            var member = new Member();
            member.FirstName = uiHelper.ReadStringInput("Enter first name:");
            member.LastName = uiHelper.ReadStringInput("Enter last name:");
            member.PhoneNumber = uiHelper.ReadStringInput("Enter phone number:");
            member.MembershipExpireDate = DateTime.Now.AddYears(1).Date; // Set membership expiry to 1 year from now
            await memberBusiness.AddAsync(member); // Add member to the system
            Console.WriteLine("Member added successfully.");
        }

        /// <summary>
        /// Allows the user to renew the membership of an existing member.
        /// </summary>
        private async Task RenewMembership()
        {
            var memberId = uiHelper.ReadIntInput("Enter Member ID to renew membership:");
            var member = await memberBusiness.GetAsync(memberId); // Fetch member by ID
            if (member != null)
            {
                var years = uiHelper.ReadIntInput("Enter number of years to renew membership:");
                await memberBusiness.RenewMembership(member, years); // Renew membership for another year and update
                Console.WriteLine("Membership renewed successfully.");
            }
            else
            {
                Console.WriteLine("Member not found."); // Handle case where member doesn't exist
            }
        }

        /// <summary>
        /// Prompts the user to update an existing member's details.
        /// </summary>
        private async Task UpdateMember()
        {
            var memberId = uiHelper.ReadIntInput("Enter Member ID to update:");
            var member = await memberBusiness.GetAsync(memberId); // Fetch member by ID
            if (member != null)
            {
                // Prompt for new details
                member.FirstName = uiHelper.ReadStringInput("Enter new first name:");
                member.LastName = uiHelper.ReadStringInput("Enter new last name:");
                member.PhoneNumber = uiHelper.ReadStringInput("Enter new phone number:");
                DateTime date = uiHelper.ReadDateInput("Enter new membership expire date:");
                member.MembershipExpireDate = date; // Update membership expiry date
                await memberBusiness.UpdateAsync(member); // Save updated member details
                Console.WriteLine("Member updated successfully.");
            }
            else
            {
                Console.WriteLine("Member not found."); // Handle case where member doesn't exist
            }
        }

        /// <summary>
        /// Fetches and displays a member's details by their ID.
        /// </summary>
        private async Task FetchMemberById()
        {
            var memberId = uiHelper.ReadIntInput("Enter Member ID to fetch:");
            var member = await memberBusiness.GetWithIncludesAsync(memberId); // Fetch member by ID
            if (member != null)
            {
                // Display member details
                Console.WriteLine($"ID: {member.Id}, Name: {member.FirstName} {member.LastName}, Membership Expire Date: {member.MembershipExpireDate:yyyy-MM-dd} Phone Number: {member.PhoneNumber}\nBorrowed Books:");
                foreach (var borrowedBook in member.BorrowedBooks.OrderBy(bb => bb.BorrowDate).Reverse())
                {
                    Book book = await bookBusiness.GetWithIncludesAsync(borrowedBook.BookID);
                    if (borrowedBook.ReturnDate != null)
                    {
                        Console.WriteLine($"Book ID: {book.Id}, Title: {book.Title}, Author: {book.Author.FirstName} {book.Author.LastName}, Borrowed on: {borrowedBook.BorrowDate:yyyy-MM-dd} Return Date: {borrowedBook.DueDate:yyyy-MM-dd}");
                    }
                    else
                    {
                        Console.WriteLine($"Book ID: {book.Id}, Title: {book.Title}, Author: {book.Author.FirstName} {book.Author.LastName}, Borrowed on: {borrowedBook.BorrowDate:yyyy-MM-dd} Due Date: {borrowedBook.DueDate:yyyy-MM-dd}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Member not found."); // Handle case where member doesn't exist
            }
        }

        /// <summary>
        /// Prompts the user to delete a member by their ID.
        /// </summary>
        private async Task DeleteMember()
        {
            var memberId = uiHelper.ReadIntInput("Enter Member ID to delete:");
            var member = await memberBusiness.GetAsync(memberId); // Fetch member by ID
            if (member != null)
            {
                await memberBusiness.DeleteAsync(memberId); // Delete the member
                Console.WriteLine("Member deleted successfully.");
            }
            else
            {
                Console.WriteLine("Member not found."); // Handle case where member doesn't exist
            }
        }
    }
}