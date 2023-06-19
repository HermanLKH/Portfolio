using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace PassTask
{
    [TestFixture()]
    public class UserTest
    {
        private DateTime paymentDue;
        private DateTime expiryDate;
        private DateTime transactionDate;
        private User testUser = null!;
        private Account testAccount1 = null!;
        private Account testAccount2 = null!;
        private Account testAccount3 = null!;
        private Card lowICCard = null!;
        private Card cashbackCCard = null!;
        private Transaction expense1 = null!;
        private Transaction expense2 = null!;
        private Transaction invalidTransfer = null!;

        [SetUp()]
        public void SetUp()
        {
            paymentDue = new DateTime(2023, 4, 29);
            expiryDate = new DateTime(2026, 12, 24);
            transactionDate = new DateTime(2023, 4, 29, 14, 30, 37);

            testUser = new User("Test Name", "123");

            testAccount1 = new Account("1234123456785677", "Standard Chartered", 4000.00);
            testAccount2 = new Account("1234123456785678", "Standard Chartered", 5000.00);
            testAccount3 = new Account("1234123456785679", "Standard Chartered", 6000.00);

            lowICCard = new Credit(800.00, 5.50, CreditType.LowInterest, paymentDue, "1111222233334444", expiryDate, "Standard Chartered LE Credit Card");
            cashbackCCard = new Credit(1500.00, 10.50, CreditType.Cashback, paymentDue, "1111222233334444", expiryDate, "Standard Chartered Platinum Credit Card");

            // lower interest rate
            expense1 = new Transaction(TransactType.Expenses, 500.00, transactionDate, "Buy Keychron Keyboard", TransactMethod.OnlineBanking);
            // sufficient credit limit
            expense2 = new Transaction(TransactType.Expenses, 1000.00, transactionDate, "Buy 2 Keychron Keyboards", TransactMethod.OnlineBanking);
            // insufficient credit limit
            invalidTransfer = new Transaction(TransactType.Transfer, 3000.00, transactionDate, "Money Laundry", testAccount1, testAccount2);
        }

        [Test()]
        public void TestProfileInfo()
        {
            // without phonenumber
            string expectedResult1 = "Username: Test Name\nPassword: 123\nPhonenumber: (not set yet)\n";
            Assert.AreEqual(expectedResult1, testUser.ProfileInfo());

            // with phonenumber
            testUser.PhoneNo = "0123456789";
            string expectedResult2 = "Username: Test Name\nPassword: 123\nPhonenumber: 0123456789\n";
            Assert.AreEqual(expectedResult2, testUser.ProfileInfo());
        }

        [Test()]
        public void TestSuggestCreditCard()
        {
            testUser.Accounts.Add(testAccount1);
            testUser.Accounts.Add(testAccount2);
            testUser.Accounts.Add(testAccount3);
            testAccount1.Card = lowICCard;
            testAccount2.Card = cashbackCCard;

            Assert.AreEqual(lowICCard, testUser.GetCreditCardSuggestion(expense1.TransactionAmount));
            Assert.AreEqual(cashbackCCard, testUser.GetCreditCardSuggestion(expense2.TransactionAmount));
            Assert.AreEqual(null, testUser.GetCreditCardSuggestion(invalidTransfer.TransactionAmount));
        }

        [Test()]
        public void TestAccountsSummary()
        {
            testUser.Accounts.Add(testAccount1);
            testUser.Accounts.Add(testAccount2);
            testUser.Accounts.Add(testAccount3);

            string expectedResult = "1. 1234123456785677\n";
            expectedResult += "2. 1234123456785678\n";
            expectedResult += "3. 1234123456785679\n";

            Assert.AreEqual(expectedResult, testUser.AccountsSummary());
        }

        [Test()]
        public void TestDeleteAccount()
        {
            testUser.Accounts.Add(testAccount1);
            testUser.Accounts.Add(testAccount2);
            testUser.Accounts.Add(testAccount3);

            Assert.AreEqual(3, testUser.Accounts.Count);
            testUser.DeleteAccount(0);

            Assert.AreEqual(2, testUser.Accounts.Count);
            CollectionAssert.DoesNotContain(testUser.Accounts, testAccount1);
        }
    }
}