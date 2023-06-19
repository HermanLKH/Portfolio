using System;
using System.Collections.Generic;

namespace PassTask
{
    public class Account
    {
        private string _accountNo;
        private string _bank;
        private double _balance;
        private Card? _card;
        private List<Transaction> _transcations = new List<Transaction>();

        public string AccountNo
        {
            get
            {
                return _accountNo;
            }
            set
            {
                _accountNo = value;
            }
        }
        public string Bank
        {
            get
            {
                return _bank;
            }
            set
            {
                _bank = value;
            }
        }
        public double Balance
        {
            get
            {
                return _balance;
            }
            set
            {
                _balance = value;
            }
        }
        public Card? Card
        {
            get
            {
                return _card;
            }
            set
            {
                _card = value;
            }
        }
        public List<Transaction> Transactions
        {
            get
            {
                return _transcations;
            }
            set
            {
                _transcations = value;
            }
        }

        /// <summary>
        /// This is a default constructor which set the account no, bank and balance
        /// </summary>
        /// <param name="accountNo"></param>
        /// <param name="bank"></param>
        /// <param name="balance"></param>
        public Account(string accountNo, string bank, double balance)
        {
            _accountNo = accountNo;
            _bank = bank;
            _balance = balance;
        }

        /// <summary>
        /// This method returns the account information, including account number, associated bank, balance, and linked card (if any).
        /// </summary>
        /// <returns></returns>
        public string AccountInfo()
        {
            string accountInfo = $"Account No: {_accountNo}\n";
            accountInfo += $"Bank Associated: {_bank}\n";
            accountInfo += $"Balance: {_balance}\n";
            if (_card != null)
            {
                // accountInfo += $"Card Name: {_card.CardName}\n";
                accountInfo += _card.CardInfo();
            }
            else
            {
                accountInfo += "Card linked: (no card linked)\n";
            }

            return accountInfo;
        }
        /// <summary>
        /// This method returns the transactions information for a specific date or month, depending on the input parameters, including the transaction number and details.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="daily"></param>
        /// <returns></returns>
        public string TransactionsInfo(DateTime dateTime, bool daily)
        {
            string transactionsInfo = "";

            if (_transcations.Count != 0)
            {
                if (daily)
                {
                    transactionsInfo += "------ Transactions Info - Daily ------\n";
                }
                else
                {
                    transactionsInfo += "------ Transactions Info - Monthly ------\n";
                }

                foreach (Transaction t in _transcations)
                {
                    transactionsInfo += $"{_transcations.IndexOf(t) + 1}.\n";

                    if (daily)
                    {
                        if (t.TransactionDate == dateTime.Date)
                        {
                            transactionsInfo += $"{t.TransactionInfo()}";
                        }
                    }
                    else
                    {
                        if (t.TransactionDate.Year == dateTime.Year && t.TransactionDate.Month == dateTime.Month)
                        {
                            transactionsInfo += $"{t.TransactionInfo()}";
                        }
                    }
                }
            }
            else
            {
                transactionsInfo += "No transactions found";
            }

            return transactionsInfo;
        }
        /// <summary>
        /// This method returns all transaction information, including transaction number and details.
        /// </summary>
        /// <returns></returns>
        public string TransactionsInfo()
        {
            string transactionsInfo = "";

            foreach (Transaction t in _transcations)
            {
                transactionsInfo += $"{_transcations.IndexOf(t) + 1}.\n";
                transactionsInfo += $"{t.TransactionInfo()}";
            }

            return transactionsInfo;
        }
        /// <summary>
        /// This method links a card to the account and returns a string confirming the card name and number that has been linked.
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public string LinkCard(Card card)
        {
            _card = card;
            return $"{card.CardName}({card.CardNo}) has been linked to account {_accountNo}\n";
        }
        /// <summary>
        /// This method unlinks the current card from the account and returns a string confirming the card name and number that has been unlinked.
        /// </summary>
        /// <returns></returns>
        public string UnlinkCard()
        {
            string result = $"{_card!.CardName}({_card!.CardNo}) has been unlinked from account {_accountNo}\n";
            _card = null;
            return result;
        }

        /// <summary>
        /// This private method updates the account balance and linked credit card's current credit limit (if applicable) for a new transaction.
        /// </summary>
        /// <param name="newTransaction"></param>
        /// <param name="revertNo"></param>
        private void _updateBalance(Transaction newTransaction, int revertNo = 1)
        {
            if (newTransaction.TransactionMethod == TransactMethod.CreditCard && _card is Credit card)
            {
                card.CurrentCreditLimit += newTransaction.TransactionAmount * revertNo;
            }
            else
            {
                _balance += newTransaction.TransactionAmount * revertNo;
            }
        }

        /// <summary>
        /// This method adds a transfer transaction, clones it, and adds it to the destination account.
        /// </summary>
        /// <param name="newTransfer"></param>
        /// <returns></returns>
        public string AddTransfer(Transaction newTransfer)
        {
            AddTransaction(newTransfer);
            Transaction cloneTransfer = new Transaction(newTransfer);
            return newTransfer.DestAccount!.AddTransaction(cloneTransfer);
        }
        /// <summary>
        /// This method validates and adds a transaction based on balance and type. It returns a success or failure message.
        /// </summary>
        /// <param name="newTransaction"></param>
        /// <returns></returns>
        public string AddTransaction(Transaction newTransaction)
        {
            bool isTransferSource = newTransaction.TransactionType == TransactType.Transfer
                                    && this == newTransaction.SourceAccount;
            bool isTransferDest = newTransaction.TransactionType == TransactType.Transfer
                                    && this == newTransaction.DestAccount;
            bool isTransactionValid = ((isTransferSource
                                       || newTransaction.TransactionType == TransactType.Expenses)
                                       && _balance >= Math.Abs(newTransaction.TransactionAmount))
                                       || newTransaction.TransactionType == TransactType.Income
                                       || isTransferDest;

            if (isTransactionValid)
            {
                _updateBalance(newTransaction);
                _transcations.Add(newTransaction);
                return "Transaction Successful";
            }
            else
            {
                return "Transaction Failed";
            }
        }
        /// <summary>
        /// This method validates and edits a transaction based on balance and type. It returns a success or failure message.
        /// </summary>
        /// <param name="pastTransaction"></param>
        /// <param name="editedTransaction"></param>
        /// <returns></returns>
        public string EditTransaction(Transaction pastTransaction, Transaction editedTransaction)
        {
            bool isTransactionValid = (editedTransaction.TransactionType == TransactType.Expenses
               && _balance >= Math.Abs(editedTransaction.TransactionAmount))
               || editedTransaction.TransactionType == TransactType.Income;

            if (isTransactionValid)
            {
                _updateBalance(pastTransaction, -1);
                _updateBalance(editedTransaction);
                _transcations[_transcations.IndexOf(pastTransaction)] = editedTransaction;

                return "Transaction Updated Successful";
            }
            else
            {
                return "Transaction Updated Failed";
            }
        }
        /// <summary>
        /// This method validates and edits a transfer transaction based on balance. It returns a success or failure message.
        /// </summary>
        /// <param name="pastTransaction"></param>
        /// <param name="editedTransfer"></param>
        /// <returns></returns>
        public string EditTransfer(Transaction pastTransaction, Transaction editedTransfer)
        {
            bool isTransactionValid = _balance >= Math.Abs(editedTransfer.TransactionAmount);

            if (isTransactionValid)
            {
                _updateBalance(pastTransaction, -1);
                _updateBalance(editedTransfer);

                Account dest = pastTransaction.DestAccount!;

                dest._updateBalance(pastTransaction);
                dest._updateBalance(editedTransfer, -1);

                for (int i = 0; i < _transcations.Count; i++)
                {
                    if (_transcations[i].ID == pastTransaction.ID)
                    {
                        // update transaction object
                        _transcations[i] = new Transaction(editedTransfer);
                    }
                }

                for (int i = 0; i < pastTransaction.DestAccount!.Transactions.Count; i++)
                {
                    if (pastTransaction.DestAccount.Transactions[i].ID == pastTransaction.ID)
                    {
                        // set destination account transfer pair = clone transfer
                        pastTransaction.DestAccount.Transactions[i] = new Transaction(editedTransfer);
                    }
                }

                return "Transaction Updated Successful";
            }
            else
            {
                return "Transaction Updated Failed";
            }
        }
        /// <summary>
        /// This method deletes a transaction and updates the account balance accordingly. It returns a success message.
        /// </summary>
        /// <param name="pastTransaction"></param>
        /// <returns></returns>
        public string DeleteTransaction(Transaction pastTransaction)
        {
            _updateBalance(pastTransaction, -1);

            _transcations.Remove(pastTransaction);

            return "Transaction Deleted Successful";
        }
        /// <summary>
        /// This method deletes a transfer transaction and updates account balances accordingly. It returns a success message.
        /// </summary>
        /// <param name="pastTransfer"></param>
        /// <returns></returns>
        public string DeleteTransfer(Transaction pastTransfer)
        {
            _updateBalance(pastTransfer, -1);
            pastTransfer.DestAccount!._updateBalance(pastTransfer);

            _transcations.Remove(pastTransfer);
            pastTransfer.DestAccount.Transactions.Remove(pastTransfer);

            return "Transaction Deleted Successful";
        }
    }
}