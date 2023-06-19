using System;
using System.Collections.Generic;

namespace PassTask
{
    public class Transaction
    {
        private static int id = 0;
        private int _id;
        public int ID
        {
            get
            {
                return _id;
            }
        }
        private TransactType _transactionType;
        public TransactType TransactionType
        {
            get
            {
                return _transactionType;
            }
        }
        private double _transactionAmount;
        public double TransactionAmount
        {
            get
            {
                return _transactionAmount;
            }
            set
            {
                _transactionAmount = value;
            }
        }
        private TransactMethod _transactionMethod;
        public TransactMethod TransactionMethod
        {
            get
            {
                return _transactionMethod;
            }
        }
        private DateTime _transactionDate;
        public DateTime TransactionDate
        {
            get
            {
                return _transactionDate;
            }
            set
            {
                _transactionDate = value;
            }
        }
        private string _description;
        private Account? _destAccount;
        public Account? DestAccount
        {
            get
            {
                return _destAccount;
            }
            set
            {
                _destAccount = value;
            }
        }
        private Account? _sourceAccount;
        public Account? SourceAccount
        {
            get
            {
                return _sourceAccount;
            }
            set
            {
                _sourceAccount = value;
            }
        }
        // transaction
        /// <summary>
        /// This is a constructor which creates a transaction object with the provided transaction details.
        /// It initializes source and destination accounts to null. The ID of the transaction is also assigned.
        /// </summary>
        /// <param name="transactionType"></param>
        /// <param name="transactionAmount"></param>
        /// <param name="transactionDate"></param>
        /// <param name="transactionDescription"></param>
        /// <param name="transactionMethod"></param>
        public Transaction(
            TransactType transactionType,
            double transactionAmount,
            DateTime transactionDate,
            string transactionDescription,
            TransactMethod transactionMethod)
        {
            _id = id;
            id += 1;

            _transactionType = transactionType;
            _transactionAmount = transactionAmount;
            _transactionMethod = transactionMethod;
            _transactionDate = transactionDate;
            _description = transactionDescription;
            _sourceAccount = null;
            _destAccount = null;
        }
        // transfer
        /// <summary>
        /// This is a constructor which creates a new transaction object of the specified type, amount, and description with optional source and destination accounts. 
        /// It also sets the transaction date to the current date and the transaction method to "OnlineBanking".
        /// </summary>
        /// <param name="transactionType"></param>
        /// <param name="transactionAmount"></param>
        /// <param name="transactionDate"></param>
        /// <param name="transactionDescription"></param>
        /// <param name="sourceAccount"></param>
        /// <param name="destAccount"></param>
        /// <returns></returns>
        public Transaction(
            TransactType transactionType,
            double transactionAmount,
            DateTime transactionDate,
            string transactionDescription,
            Account? sourceAccount,
            Account? destAccount) : this(transactionType, transactionAmount, transactionDate, transactionDescription, TransactMethod.OnlineBanking)
        {
            _sourceAccount = sourceAccount;
            _destAccount = destAccount;
        }
        /// <summary>
        /// This is a constructor which creates a new transaction with the same information as the provided transaction, but with the transaction amount negated.
        /// This is to clone transaction with same id for transfer for another account
        /// </summary>
        /// <param name="transaction"></param>
        public Transaction(Transaction transaction)
        {
            _transactionType = transaction._transactionType;
            _transactionAmount = -transaction._transactionAmount;
            _transactionMethod = transaction._transactionMethod;
            _transactionDate = transaction._transactionDate;
            _description = transaction._description;
            _destAccount = transaction._destAccount;
            _sourceAccount = transaction._sourceAccount;
        }
        /// <summary>
        /// This method returns a string containing information about the transaction, including its type, amount, method, date, description, and source/destination account number (if applicable)
        /// </summary>
        /// <returns></returns>
        public string TransactionInfo()
        {
            string transactionInfo = $"Transaction Type: {_transactionType}\n";
            transactionInfo += $"Transaction Amount: {_transactionAmount}\n";
            transactionInfo += $"Transaction Method: {_transactionMethod}\n";
            transactionInfo += $"Transaction Date: {_transactionDate}\n";
            transactionInfo += $"Transaction Description: {_description}\n";
            if (_sourceAccount != null)
            {
                transactionInfo += $"Source Account: {_sourceAccount.AccountNo}\n";
            }
            if (_destAccount != null)
            {
                transactionInfo += $"Destination Account: {_destAccount.AccountNo}\n";
            }
            return transactionInfo;
        }
    }
}