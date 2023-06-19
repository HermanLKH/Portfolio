using System;
using System.Collections.Generic;

namespace PassTask
{
    public class Menu
    {
        private string _menuTitle;
        private List<string> _menuOptions;
        /// <summary>
        /// This is a default constructor for creating a Menu object with a specified menu title and options.
        /// </summary>
        /// <param name="menuTitle"></param>
        /// <param name="menuOptions"></param>
        public Menu(string menuTitle, List<string> menuOptions)
        {
            _menuTitle = menuTitle;
            _menuOptions = menuOptions;
        }
        /// <summary>
        /// This method gets an option from the user based on the menu options and handles invalid inputs.
        /// </summary>
        /// <returns></returns>
        public int GetOption()
        {
            string? input = null;
            int option = 0;
            int max = _menuOptions.Count;

            while (input == null)
            {
                input = Console.ReadLine();
                if (int.TryParse(input, out option))
                {
                    if (option >= 1 && option <= max)
                    {
                        break;
                    }
                }
                input = null;
            }
            return option;
        }
        /// <summary>
        /// This method returns a formatted string representing the title and options of the menu to be displayed.
        /// </summary>
        /// <returns></returns>
        public string MenuInfo()
        {
            string info = $"\n  --------- {_menuTitle} ---------  \n";

            for (int i = 1; i <= _menuOptions.Count; i++)
            {
                info += $"  {i}. {_menuOptions[i - 1]} \n";
            }

            info += "  Please select an option: ";

            return info;
        }
    }
}