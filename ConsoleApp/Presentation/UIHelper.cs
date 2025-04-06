using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Presentation
{
    internal class UIHelper
    {
        /// <summary>
        /// Displays a centered header with the given title.
        /// </summary>
        /// <param name="title">The title to display in the header.</param>
        public void ShowHeader(string title)
        {
            int totalWidth = 40; // Set the header width

            Console.WriteLine(new string('-', totalWidth)); // Top border
            Console.WriteLine(title.PadLeft((totalWidth + title.Length) / 2).PadRight(totalWidth)); // Centered title
            Console.WriteLine(new string('-', totalWidth)); // Bottom border
        }

        /// <summary>
        /// Reads and returns a valid integer input from the user.
        /// Keeps prompting until a valid integer is entered.
        /// </summary>
        /// <param name="prompt">The message to display to the user.</param>
        public int ReadIntInput(string prompt)
        {
            int result;
            while (true)
            {
                Console.WriteLine(prompt);
                if (int.TryParse(Console.ReadLine(), out result))
                {
                    return result;
                }
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }

        /// <summary>
        /// Reads and returns a non-empty string input from the user.
        /// Keeps prompting until a valid string is entered.
        /// </summary>
        /// <param name="prompt">The message to display to the user.</param>
        public string ReadStringInput(string prompt)
        {
            string input;
            while (true)
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Input cannot be empty. Please enter a valid string.");
                    continue;
                }

                return input;
            }
        }
    }
}