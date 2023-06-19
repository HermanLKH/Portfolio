using System;
using System.Collections.Generic;

namespace PassTask
{
    public class App
    {
        private Menu _currentMenu;
        private List<Menu> _menus;
        private bool _isActive = true;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = false;
            }
        }

        /// <summary>
        /// 
        /// This is a default constructor initializes menus for user and account management. It sets the active menu as the user menu.
        /// </summary>
        public App()
        {
            Console.WriteLine("Welcome to Swinoop Bank!\n");

            List<string> userOptions = new List<string>()
            {
                "Register",
                "Login",
                "Exit"
            };
            List<string> mainOptions = new List<string>()
            {
                "Profile Management",
                "Card Management",
                "Transaction Management",
                "Logout",
                "Exit"
            };
            List<string> profileOptions = new List<string>()
            {
                "Edit username",
                "Edit password",
                "Edit phonenumber",
                "View profile info",
                "Back to main menu"
            };
            List<string> cardOptions = new List<string>()
            {
                "Add account",
                "View accounts info",
                "Edit account/ card",
                "Back to main menu"
            };
            List<string> cardSubOptions = new List<string>()
            {
                "Edit Account",
                "Add / Edit Card",
                "Delete Account",
                "Delete Card",
                "Back"
            };
            List<string> transactionOptions = new List<string>()
            {
                // view all transaction info first
                "Make new transaction",  // if expense transaction, suggest credit card
                "Edit transaction",
                "Delete transaction",
                "View transactions",  // daily or monthly format
                "Back to Main Menu"
            };

            Menu mainMenu = new Menu("Main Menu", mainOptions);
            Menu profileMenu = new Menu("Profile Management", profileOptions);
            Menu cardMenu = new Menu("Card Management", cardOptions);
            Menu transactionMenu = new Menu("Transaction Management", transactionOptions);
            Menu cardSubMenu = new Menu("Card Management", cardSubOptions);
            Menu userMenu = new Menu("User Management", userOptions);

            _menus = new List<Menu>(){
                mainMenu,
                profileMenu,
                cardMenu,
                transactionMenu,
                cardSubMenu,
                userMenu
            };

            _currentMenu = _menus[5];
            _isActive = true;
        }
        /// <summary>
        /// This method validates user login credentials and returns a User object if successful. Otherwise, it returns null.
        /// </summary>
        /// <returns></returns>
        public User? ValidateLogin()
        {
            bool isLogin = false;
            string username;
            string password;

            while (!isLogin)
            {
                username = Functions.GetString("Username: ");
                password = Functions.GetString("Password: ");

                User? user = User.ValidateCredential(username, password);
                if (user != null)
                {
                    isLogin = true;
                    Console.WriteLine($"Welcome, {user.Username}!");
                    return user;
                }
            }
            Console.WriteLine("Login failed, please login again...");
            return null;
        }
        /// <summary>
        /// This method registers a new user by prompting for a username and password and adding a new User object to the system.
        /// </summary>
        /// <param name="first"></param>
        public void RegisterUser(bool first = false)
        {
            if (first)
            {
                string username = Functions.GetString("Username: (temp)");
                string password = Functions.GetString("Password: (password)");

                while (username != "temp" || password != "password")
                {
                    username = Functions.GetString("Username: (temp)");
                    password = Functions.GetString("\nPassword: (password)");
                }
            }

            string newUsername = Functions.GetString("New Username: ");
            string newPassword = Functions.GetString("New Password: ");

            User newUser = new User(newUsername, newPassword);
            User.AddUser(newUser);
        }

        /// <summary>
        /// This method displays the current menu and returns the selected option.
        /// </summary>
        /// <returns></returns>
        public int DisplayMenu()
        {
            Console.WriteLine(_currentMenu.MenuInfo());
            return _currentMenu.GetOption();
        }
        /// <summary>
        /// This method changes the active menu to the one at the specified index.
        /// </summary>
        /// <param name="index"></param>
        public void ChangeMenu(int index)
        {
            _currentMenu = _menus[index];
        }
        /// <summary>
        /// This method displays the current menu, calls a function with the option and User object, and returns to the main menu when selecting "Back".
        /// </summary>
        /// <param name="func"></param>
        /// <param name="user"></param>
        public void HandleMenu(Func<int, User, bool> func, User user)
        {
            bool isBack = false;

            while (!isBack)
            {
                int subOption = DisplayMenu();
                isBack = func(subOption, user);
            }

            if (isBack)
            {
                ChangeMenu(0);
                isBack = false;
            }
        }
        /// <summary>
        /// This method handles profile management options, including edit username, password, phone number and view profile.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool ProfileManagement(int option, User user)
        {
            switch (option)
            {
                case 5:
                    return true;
                case 1:
                    user.Username = Functions.GetString("New username: ");
                    Console.WriteLine("Username updated successful");
                    break;
                case 2:
                    user.Password = Functions.GetString("New password: ");
                    Console.WriteLine("Password updated successful");
                    break;
                case 3:
                    user.PhoneNo = Functions.GetString("New phonenumber: ");
                    Console.WriteLine("Phonenumber updated successful");
                    break;
                case 4:
                    Console.WriteLine(user.ProfileInfo());
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
            return false;
        }
        /// <summary>
        /// This method handles card management options, including adding an account, viewing accounts info, editing account/card, deleting account/card.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool CardManagement(int option, User user)
        {
            switch (option)
            {
                case 4:
                    return true;
                case 1:
                    user.AddAccount();
                    Console.WriteLine("New Account Created Successful");
                    break;
                case 2:
                    Console.WriteLine(user.AccountsInfo());
                    break;
                case 3:
                    Console.WriteLine(user.AccountsSummary());
                    int accountIdx = Functions.GetInt("Please select the index of account to be view: ") - 1;
                    Account accountSelected = user.Accounts[accountIdx];

                    Console.WriteLine(user.AccountInfo(accountIdx));
                    ChangeMenu(4);
                    int subOption = DisplayMenu();
                    switch (subOption)
                    {
                        case 5:
                            return true;
                        case 1:
                            user.EditAccount(accountSelected);
                            Console.WriteLine("Account updated successful");
                            break;
                        case 2:
                            if (accountSelected.Card != null)
                            {
                                user.EditCard(accountSelected.Card);
                                Console.WriteLine("Card updated successful");
                            }
                            else
                            {
                                user.AddCard(accountSelected);
                                Console.WriteLine("Card added successful");
                            }
                            break;
                        case 3:
                            user.DeleteAccount(accountIdx);
                            Console.WriteLine("Account deleted successful");
                            break;
                        case 4:
                            if (accountSelected.Card != null)
                            {
                                Console.WriteLine(accountSelected.UnlinkCard());
                            }
                            else
                            {
                                Console.WriteLine("No card linked");
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid input");
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
            ChangeMenu(2);
            return false;
        }
        /// <summary>
        /// This method handles transaction management options, including making new transaction, editing transaction, deleting transaction, and viewing all transactions info.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool TransactionManagement(int option, User user)
        {
            Account accountSelected;
            Transaction transactionSelected;

            switch (option)
            {
                case 5:
                    return true;
                case 1:
                    // make new transaction based on transaction type)
                    int tTypeInt = Functions.GetInt("1. Income\n2. Expenses\n3. Transfer\n");

                    switch (tTypeInt)
                    {
                        case 1:
                            user.MakeTransaction(TransactType.Income);
                            break;
                        case 2:
                            user.MakeTransaction(TransactType.Expenses);
                            break;
                        case 3:
                            user.MakeTransfer();
                            break;
                        default:
                            Console.WriteLine("Invalid input");
                            break;
                    }
                    Console.WriteLine("Transaction added successful");
                    break;
                case 2:  // edit transaction
                    // select transaction to be edited - transfer/ transaction
                    Console.WriteLine(user.AccountsSummary());
                    int accountIdx = Functions.GetInt("Please select the index of account to be selected: ") - 1;
                    accountSelected = user.Accounts[accountIdx];

                    Console.WriteLine(accountSelected.TransactionsInfo());

                    int editIdx = Functions.GetInt("Please select the index of transaction to be edited: ") - 1;
                    transactionSelected = accountSelected.Transactions[editIdx];

                    if (transactionSelected.TransactionType == TransactType.Transfer)
                    {
                        user.EditTransfer(accountSelected, transactionSelected);
                    }
                    else
                    {
                        user.EditTransaction(accountSelected, transactionSelected);
                    }

                    Console.WriteLine("Transaction edited successful");
                    break;
                case 3:  // delete transaction
                    // select transaction to be edited - transfer/ transaction
                    Console.WriteLine(user.AccountsSummary());
                    accountIdx = Functions.GetInt("Please select the index of account to be selected: ") - 1;
                    accountSelected = user.Accounts[accountIdx];

                    Console.WriteLine(accountSelected.TransactionsInfo());

                    int deleteIdx = Functions.GetInt("Please select the index of transaction to be deleted: ") - 1;
                    transactionSelected = accountSelected.Transactions[deleteIdx];

                    if (transactionSelected.TransactionType == TransactType.Transfer)
                    {
                        accountSelected.DeleteTransfer(transactionSelected);
                    }
                    else
                    {
                        accountSelected.DeleteTransaction(transactionSelected);
                    }

                    Console.WriteLine("Transaction deleted successful");
                    break;
                case 4:  // view all transactions info (monthly or daily)
                    // select account to view transactions
                    Console.WriteLine(user.AccountsSummary());
                    accountIdx = Functions.GetInt("Please select the index of account to be selected: ") - 1;
                    accountSelected = user.Accounts[accountIdx];

                    int format = Functions.GetInt("1. Daily\n2. Monthly\n");
                    switch (format)
                    {
                        case 1:
                            DateTime day = Functions.GetDatetime("Please select a day: (yyyy-mm-dd)");
                            Console.WriteLine(accountSelected.TransactionsInfo(day, true));
                            break;
                        case 2:
                            DateTime month = Functions.GetDatetime("Please select a month: (yyyy-mm-dd)");
                            Console.WriteLine(accountSelected.TransactionsInfo(month, false));
                            break;
                        default:
                            Console.WriteLine("Invalid input");
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
            return false;
        }
    }
}