using Business;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Presentation.SubDisplays
{
    internal class MemberDisplay
    {
        // Business layer objects to interact with data models
        private AuthorBusiness authorBusiness = new AuthorBusiness();
        private BookBusiness bookBusiness = new BookBusiness();
        private MemberBusiness memberBusiness = new MemberBusiness();
        private BorrowedBookBusiness borrowedBookBusiness = new BorrowedBookBusiness();

        // UI helper to assist with common input/output operations
        private UIHelper uiHelper = new UIHelper();

        /// <summary>
        /// Main function to manage member-related operations.
        /// Displays a menu with options like adding, updating, deleting, or viewing members.
        /// </summary>
        public void MemberManager()
        {
            ShowMemberMenu(); // Display the member management menu
            var operation = uiHelper.ReadIntInput("Please select an option:");

            // Execute the selected operation based on user's choice
            switch (operation)
            {
                case 1:
                    ShowAllMembers(); // Display all members
                    break;
                case 2:
                    AddMember(); // Add a new member
                    break;
                case 3:
                    RenewMembership(); // Renew membership for an existing member
                    break;
                case 4:
                    UpdateMember(); // Update member details
                    break;
                case 5:
                    FetchMemberById(); // Fetch a member by their ID
                    break;
                case 6:
                    DeleteMember(); // Delete a member from the system
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
        private void ShowAllMembers()
        {
            var members = memberBusiness.GetAll(); // Retrieve all members
            Console.WriteLine("List of Members:");
            foreach (var member in members)
            {
                // Display member details
                Console.WriteLine($"ID: {member.Id}, Name: {member.FirstName} {member.LastName}, Membership Expire Date: {member.MembershipExpireDate} Phone Number: {member.PhoneNumber}");
            }
        }

        /// <summary>
        /// Prompts the user to input member details and adds the new member to the system.
        /// </summary>
        private void AddMember()
        {
            var member = new Member();
            member.FirstName = uiHelper.ReadStringInput("Enter First Name:");
            member.LastName = uiHelper.ReadStringInput("Enter Last Name:");
            member.PhoneNumber = uiHelper.ReadStringInput("Enter Phone Number:");
            member.MembershipExpireDate = DateTime.Now.AddYears(1); // Set membership expiry to 1 year from now
            memberBusiness.Add(member); // Add member to the system
            Console.WriteLine("Member added successfully.");
        }

        /// <summary>
        /// Allows the user to renew the membership of an existing member.
        /// </summary>
        private void RenewMembership()
        {
            var memberId = uiHelper.ReadIntInput("Enter Member ID to renew membership:");
            var member = memberBusiness.Get(memberId); // Fetch member by ID
            if (member != null)
            {
                member.MembershipExpireDate = DateTime.Now.AddYears(1); // Renew membership for another year
                memberBusiness.Update(member); // Update the member in the system
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
        private void UpdateMember()
        {
            var memberId = uiHelper.ReadIntInput("Enter Member ID to update:");
            var member = memberBusiness.Get(memberId); // Fetch member by ID
            if (member != null)
            {
                // Prompt for new details
                member.FirstName = uiHelper.ReadStringInput("Enter new First Name:");
                member.LastName = uiHelper.ReadStringInput("Enter new Last Name:");
                member.PhoneNumber = uiHelper.ReadStringInput("Enter new Phone Number:");
                DateTime date;
                DateTime.TryParse(uiHelper.ReadStringInput("Enter new Membership Expire Date (yyyy-mm-dd):"), out date);
                member.MembershipExpireDate = date; // Update membership expiry date
                memberBusiness.Update(member); // Save updated member details
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
        private void FetchMemberById()
        {
            var memberId = uiHelper.ReadIntInput("Enter Member ID to fetch:");
            var member = memberBusiness.Get(memberId); // Fetch member by ID
            if (member != null)
            {
                // Display member details
                Console.WriteLine($"ID: {member.Id}, Name: {member.FirstName} {member.LastName}, Membership Expire Date: {member.MembershipExpireDate} Phone Number: {member.PhoneNumber}");
            }
            else
            {
                Console.WriteLine("Member not found."); // Handle case where member doesn't exist
            }
        }

        /// <summary>
        /// Prompts the user to delete a member by their ID.
        /// </summary>
        private void DeleteMember()
        {
            var memberId = uiHelper.ReadIntInput("Enter Member ID to delete:");
            var member = memberBusiness.Get(memberId); // Fetch member by ID
            if (member != null)
            {
                memberBusiness.Delete(memberId); // Delete the member
                Console.WriteLine("Member deleted successfully.");
            }
            else
            {
                Console.WriteLine("Member not found."); // Handle case where member doesn't exist
            }
        }
    }
}