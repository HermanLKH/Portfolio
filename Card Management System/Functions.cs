using System;
using System.Collections.Generic;

namespace PassTask
{
    public static class Functions
    {
        /// <summary>
        /// This method prompts the user to enter a string until a non-null value is entered, and returns it.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public static string GetString(string prompt)
        {
            string? input = null;
            while (input == null)
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();
            }
            return input;
        }
        /// <summary>
        ///  This method prompts the user to enter a double until a non-null value is entered, and returns it.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public static double GetDouble(string prompt)
        {
            string? input = null;
            double number = 0;
            bool isDouble = false;

            while (!isDouble)
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();

                if (double.TryParse(input, out number))
                {
                    isDouble = true;
                }
            }
            return number;
        }
        /// <summary>
        /// This method prompts the user to enter a datetime until a non-null value is entered, and returns it.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public static DateTime GetDatetime(string prompt)
        {
            string? input = null;
            DateTime datetime = new DateTime();
            bool isDatetime = false;

            while (!isDatetime)
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();

                if (DateTime.TryParse(input, out datetime))
                {
                    isDatetime = true;
                }
            }
            return datetime;
        }
        /// <summary>
        /// This method prompts the user to enter an integer until a non-null value is entered, and returns it.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public static int GetInt(string prompt)
        {
            string? input = null;
            int number = 0;
            bool isInt = false;

            while (!isInt)
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();

                if (int.TryParse(input, out number))
                {
                    isInt = true;
                }
            }
            return number;
        }

    }
}