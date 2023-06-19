using System;
using System.Collections.Generic;

namespace PassTask
{
    public class User
    {
        // static
        private static List<User> _users = new List<User>();
        public static User? ValidateCredential(string username, string password)
        {
            foreach (User user in _users)
            {
                if (user._username == username && user._password == password)
                {
                    return user;
                }
            }
            return null;
        }
        public static void AddUser(User user)
        {
            _users.Add(user);
        }
        // private attributes
        private string _username;
        private string _password;
        private List<Account> _accounts;
        private string? _phoneNo;
        // public properties
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }
        public List<Account> Accounts
        {
            get
            {
                return _accounts;
            }
            set
            {
                _accounts = value;
            }
        }
        public string? PhoneNo
        {
            get
            {
                return _phoneNo;
            }
            set
            {
                _phoneNo = value;
            }
        }
        /// <summary>
        /// This is a default constructor of the User class which initializes the username, password, and an empty list of accounts.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public User(string username, string password)
        {
            _username = username;
            _password = password;
            _accounts = new List<Account>();
        }
        // methods
        /// <summary>
        /// This method returns a string containing the user's profile information including their username, password, and phone number if set.
        /// </summary>
        /// <returns></returns>
        public string ProfileInfo()
        {
            string profileInfo = $"Username: {_username}\n";
            profileInfo += $"Password: {_password}\n";

            if (_phoneNo == null)
            {
                profileInfo += "Phonenumber: (not set yet)\n";
            }
            else
            {
                profileInfo += $"Phonenumber: {_phoneNo}\n";
            }
            return profileInfo;
        }
        /// <summary>
        /// This method returns the account information for the given index if it exists, or a message indicating no account exists.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string AccountInfo(int index)
        {
            if (index < _accounts.Count)
            {
                return _accounts[index].AccountInfo();
            }
            else
            {
                return "No account yet";
            }
        }
        /// <summary>
        /// This method returns a formatted string containing information about all the user's accounts.
        /// </summary>
        /// <returns></returns>
        public string AccountsInfo()
        {
            string accountsInfo = "";
            foreach (Account a in _accounts)
            {
                accountsInfo += $"{_accounts.IndexOf(a) + 1}.\n";
                accountsInfo += $"{a.AccountInfo()}";
            }
            return accountsInfo;
        }
        /// <summary>
        /// This method returns a list of the user's account numbers.
        /// </summary>
        /// <returns></returns>
        public string AccountsSummary()
        {
            string accountsInfo = "";
            foreach (Account a in _accounts)
            {
                accountsInfo += $"{_accounts.IndexOf(a) + 1}. ";
                accountsInfo += $"{a.AccountNo}\n";
            }
            return accountsInfo;
        }
        /// <summary>
        /// This method adds a new account with the given account information and allows the user to add a card to it.
        /// </summary>
        public void AddAccount()
        {
            string accountNo = Functions.GetString("Account No: ");
            string bank = Functions.GetString("Bank Associated: ");
            double balance = Functions.GetDouble("Balance: ");
            Account newAccount = new Account(accountNo, bank, balance);

            bool isAddCard = Functions.GetString("Add Card? (y/n)") == "y";
            if (isAddCard)
            {
                AddCard(newAccount);
            }
            _accounts.Add(newAccount);
        }
        /// <summary>
        /// This method updates the account's information with user-inputted values for account number, associated bank, and balance.
        /// </summary>
        /// <param name="account"></param>
        public void EditAccount(Account account)
        {
            Console.WriteLine("New Account Info: ");
            account.AccountNo = Functions.GetString("Account No: ");
            account.Bank = Functions.GetString("Bank Associated: ");
            account.Balance = Functions.GetDouble("Balance: ");
        }
        /// <summary>
        /// This method deletes an account at the given index.
        /// </summary>
        /// <param name="idx"></param>
        public void DeleteAccount(int idx)
        {
            _accounts.Remove(_accounts[idx]);
            return;
        }
        /// <summary>
        /// This method adds a new credit or debit card to the specified account and prompts the user for the card information.
        /// </summary>
        /// <param name="account"></param>
        public void AddCard(Account account)
        {
            string cardNo = Functions.GetString("Card No: ");
            string cardName = Functions.GetString("Card Name: ");
            DateTime expiryDate = Functions.GetDatetime("Expiry Date: (yyyy-mm-dd)");
            string cardType = Functions.GetString("Card type(credit/ debit): ");
            if (cardType == "credit")
            {
                double creditLimit = Functions.GetDouble("Credit Limit: ");
                double interestRate = Functions.GetDouble("Interest Rate: ");
                DateTime paymentDue = Functions.GetDatetime("Payment Due Date: (yyyy-mm-dd)");
                CreditType creditType = CreditType.Unknown;

                int creditTypeOpt = Functions.GetInt("1 - Rewards\n2 - Cashback\n3 - Low Interest");
                switch (creditTypeOpt)
                {
                    case 1:
                        creditType = CreditType.Rewards;
                        break;
                    case 2:
                        creditType = CreditType.Cashback;
                        break;
                    case 3:
                        creditType = CreditType.LowInterest;
                        break;
                    default:
                        break;
                }
                Credit creditCard = new Credit(creditLimit, interestRate, creditType, paymentDue, cardNo, expiryDate, cardName);
                Console.WriteLine(account.LinkCard(creditCard));
            }
            else
            {
                Debit debitCard = new Debit(cardNo, expiryDate, cardName);
                Console.WriteLine(account.LinkCard(debitCard));
            }
        }
        /// <summary>
        /// This method edits the card information by prompting the user to enter new values.
        /// </summary>
        /// <param name="card"></param>
        public void EditCard(Card card)
        {
            Console.WriteLine("New Card Info: ");
            card.CardNo = Functions.GetString("Card No: ");
            card.CardName = Functions.GetString("Card Name: ");
            card.ExpiryDate = Functions.GetDatetime("Expiry Date: (yyyy-mm-dd)");
            if (card is Credit)
            {
                Credit creditCard = (Credit)card;
                creditCard.CreditLimit = Functions.GetDouble("Credit Limit: ");

                creditCard.InterestRate = Functions.GetDouble("Interest Rate: ");
                int creditTypeInt = Functions.GetInt("Credit Type: \n1. Rewards\n2. Cashback\n3. Low Interest");
                switch (creditTypeInt)
                {
                    case 1:
                        creditCard.CreditType = CreditType.Rewards;
                        break;
                    case 2:
                        creditCard.CreditType = CreditType.Cashback;
                        break;
                    case 3:
                        creditCard.CreditType = CreditType.LowInterest;
                        break;
                }
                creditCard.PaymentDue = Functions.GetDatetime("Payment Due Date: (yyyy-mm-dd)");

                card = creditCard;
            }
            else
            {
                Debit debitCard = (Debit)card;
                card = debitCard;
            }
        }
        /// <summary>
        /// This method creates a new transaction to transfer funds between two accounts owned by the user, based on user inputs.
        /// </summary>
        public void MakeTransfer()
        {
            Console.WriteLine(AccountsSummary());
            int accountIdx = Functions.GetInt("Please select the index of account to be selected: ") - 1;
            Account source = _accounts[accountIdx];

            DateTime tDate = Functions.GetDatetime("Transaction date: (yyyy-mm-dd)");
            TransactType tType = TransactType.Transfer;

            Console.WriteLine(AccountsSummary());
            Account dest = _accounts[Functions.GetInt("Enter the index of destination account: ") - 1];
            double tAmount = Functions.GetDouble("Transaction amount: ") * -1;

            string tDescription = Functions.GetString("Transaction description: ");

            Transaction transfer = new Transaction(tType, tAmount, tDate, tDescription, source, dest);
            source.AddTransfer(transfer);
        }
        /// <summary>
        /// This method creates a new transaction to make an expense or income, based on user inputs and add it to selected account.
        /// </summary>
        /// <param name="tType"></param>
        public void MakeTransaction(TransactType tType)
        {
            TransactMethod tMethod = TransactMethod.OnlineBanking;
            int tMethodInt;
            double tAmount;
            Account account = null!;

            bool isSuggestionChosen = false;

            if (tType == TransactType.Expenses)
            {
                tMethodInt = Functions.GetInt("1. Debit\n2. Online banking\n3. Credit\n");
                tAmount = Functions.GetDouble("Transaction amount: ") * -1;

                Credit? suggestedCreditCard = GetCreditCardSuggestion(tAmount);

                if (suggestedCreditCard != null)
                {
                    Console.WriteLine($"{suggestedCreditCard.CardName}, {suggestedCreditCard.InterestRate}");

                    isSuggestionChosen = Functions.GetString("Do you want to choose this credit card to make transaction? (y/n)") == "y";
                    if (isSuggestionChosen)
                    {
                        foreach (Account a in _accounts)
                        {
                            if (a.Card == suggestedCreditCard)
                            {
                                account = a;
                                tMethod = TransactMethod.CreditCard;
                            }
                        }
                    }
                }
            }
            else
            {
                tMethodInt = Functions.GetInt("1. Debit\n2. Online banking");
                tAmount = Functions.GetDouble("Transaction amount: ");
            }

            if (!isSuggestionChosen)
            {
                Console.WriteLine(AccountsSummary());
                int accountIdx = Functions.GetInt("Please select the index of account to be selected: ") - 1;
                account = _accounts[accountIdx];
            }

            switch (tMethodInt)
            {
                case 1:
                    tMethod = TransactMethod.DebitCard;
                    break;
                case 2:
                    tMethod = TransactMethod.OnlineBanking;
                    break;
                case 3:
                    tMethod = TransactMethod.CreditCard;
                    break;
            }

            string tDescription = Functions.GetString("Transaction description: ");
            DateTime tDate = Functions.GetDatetime("Transaction date: (yyyy-mm-dd)");

            Transaction transaction = new Transaction(tType, tAmount, tDate, tDescription, tMethod);
            account.AddTransaction(transaction);
        }
        /// <summary>
        /// This method edits a transaction by prompting the user to enter new values.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pastTransaction"></param>
        public void EditTransaction(Account account, Transaction pastTransaction)
        {
            TransactMethod tMethod;
            int tMethodInt;
            double tAmount;

            DateTime tDate = Functions.GetDatetime("Transaction date: (yyyy-mm-dd)");

            if (pastTransaction.TransactionType == TransactType.Expenses)
            {
                tAmount = -1;
                tMethodInt = Functions.GetInt("1. Debit\n2. Online banking\n3. Credit\n");
            }
            else
            {
                tAmount = 1;
                tMethodInt = Functions.GetInt("1. Debit\n2. Online banking\n");
            }

            switch (tMethodInt)
            {
                case 1:
                    tMethod = TransactMethod.DebitCard;
                    break;
                case 2:
                    tMethod = TransactMethod.OnlineBanking;
                    break;
                case 3:
                    tMethod = TransactMethod.CreditCard;
                    break;
                default:
                    tMethod = TransactMethod.OnlineBanking;
                    break;
            }

            tAmount *= Functions.GetDouble("Transaction amount: ");
            string tDescription = Functions.GetString("Transaction description: ");

            Transaction newTransaction = new Transaction(pastTransaction.TransactionType, tAmount, tDate, tDescription, tMethod);
            account.EditTransaction(pastTransaction, newTransaction);
        }
        /// <summary>
        /// This method edits a transfer by prompting the user to enter new values.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pastTransfer"></param>
        public void EditTransfer(Account account, Transaction pastTransfer)
        {
            TransactType tType;

            DateTime tDate = Functions.GetDatetime("Transaction date: (yyyy-mm-dd)");
            tType = TransactType.Transfer;

            Console.WriteLine(AccountsSummary());
            // assume cannot change destination account
            double tAmount = Functions.GetDouble("Transaction amount: ") * -1;

            string tDescription = Functions.GetString("Transaction description: ");

            Transaction newTransfer = new Transaction(tType, tAmount, tDate, tDescription, pastTransfer.SourceAccount, pastTransfer.DestAccount);
            account.EditTransfer(pastTransfer, newTransfer);
        }
        /// <summary>
        /// This method returns a credit card suggestion based on the transaction amount.
        /// </summary>
        /// <param name="transactAmount"></param>
        /// <returns></returns>
        public Credit? GetCreditCardSuggestion(double transactAmount)
        {
            return Credit.SuggestCreditCard(this, transactAmount);
        }
    }
}