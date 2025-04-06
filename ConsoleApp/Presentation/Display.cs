using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Business;
using ConsoleApp.Presentation.SubDisplays;
using Data.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ConsoleApp.Presentation
{
    internal class Display
    {
        private AuthorDisplay authorDisplay = new AuthorDisplay();
        private BookDisplay bookDisplay = new BookDisplay();
        private MemberDisplay memberDisplay = new MemberDisplay();

        private UIHelper uiHelper = new UIHelper();

        public Display()
        {
            Input();
        }

        public void ShowMenu()
        {
            uiHelper.ShowHeader("Library Management System");
            Console.WriteLine("1. Books");
            Console.WriteLine("2. Members");
            Console.WriteLine("3. Authors");
            Console.WriteLine("4. Borrowed Books History");
            Console.WriteLine("5. Exit");
        }

        private void Input()
        {
            var operation = -1;
            do
            {
                ShowMenu();
                operation = uiHelper.ReadIntInput("Please select an option:");
                switch (operation)
                {
                    case 1:
                        bookDisplay.BookManager();
                        break;
                    case 2:
                        memberDisplay.MemberManager();
                        break;
                    case 3:
                        authorDisplay.AuthorManager();
                        break;
                    /*case 4:
                        BorrowedBookManager();
                        break;*/
                    default:
                        Console.WriteLine("Please select a valid option.");
                        break;
                }
            } while (operation != 5);
            Environment.Exit(0);
        }
    }
}
