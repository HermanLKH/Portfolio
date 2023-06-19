using System;
using System.Collections.Generic;

namespace PassTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            App app = new App();
            User? user = null;
            app.RegisterUser(true);
            int menuOption;

            while (app.IsActive)
            {
                menuOption = app.DisplayMenu();
                switch (menuOption)
                {
                    case 3:  // exit
                        app.IsActive = false;
                        break;

                    case 1:  // register
                        app.RegisterUser(false);
                        break;

                    case 2:  // login
                        user = app.ValidateLogin();
                        if (user != null)
                        {
                            app.ChangeMenu(0);
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid input!");
                        break;
                }

                while (user != null)
                {
                    menuOption = app.DisplayMenu();

                    switch (menuOption)
                    {
                        case 5:  // exit
                            user = null;
                            app.IsActive = false;
                            break;

                        case 1:  // profile management
                            Console.WriteLine(user!.ProfileInfo());
                            app.ChangeMenu(1);
                            app.HandleMenu(app.ProfileManagement, user);
                            break;

                        case 2:  // card management
                            app.ChangeMenu(2);
                            app.HandleMenu(app.CardManagement, user);
                            break;

                        case 3:  // transaction management
                            app.ChangeMenu(3);
                            app.HandleMenu(app.TransactionManagement, user);
                            break;

                        case 4:  // logout
                            user = null;
                            app.ChangeMenu(5);
                            break;

                        default:
                            Console.WriteLine("Invalid input!");
                            break;
                    }

                }

            }

            Console.WriteLine("Thank you!");
        }
    }
}