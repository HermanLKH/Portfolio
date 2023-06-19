using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace PassTask
{
    [TestFixture()]
    public class AccountTest
    {
        private DateTime paymentDue;
        private DateTime expiryDate;
        private DateTime transactionDate;
        private Account testAccount1 = null!;
        private Account testAccount2 = null!;
        private Card testCCard = null!;
        private Card testDCard = null!;
        private Transaction incomeO = null!;
        private Transaction expenseO = null!;
        private Transaction expenseC = null!;
        private Transaction transfer1 = null!;
        private Transaction transfer2 = null!;
        private Transaction invalidTransfer = null!;
        [SetUp()]
        public void SetUp()
        {
            paymentDue = new DateTime(2023, 4, 29);
            expiryDate = new DateTime(2026, 12, 24);
            transactionDate = new DateTime(2023, 4, 29, 14, 30, 37);
            testAccount1 = new Account("1234123456785678", "Standard Chartered", 5000.00);
            testAccount2 = new Account("1234123456785679", "Standard Chartered", 5000.00);
            testCCard = new Credit(1000.00, 10.5, CreditType.Cashback, paymentDue, "1111222233334444", expiryDate, "Standard Chartered Credit Card");
            testDCard = new Debit("5555666677778888", expiryDate, "Standard Chartered Mastercard");
            incomeO = new Transaction(TransactType.Income, 600.00, transactionDate, "Sell Keychron Keyboard", TransactMethod.OnlineBanking);
            expenseO = new Transaction(TransactType.Expenses, -1000.00, transactionDate, "Buy 2 Keychron Keyboards", TransactMethod.OnlineBanking);
            expenseC = new Transaction(TransactType.Expenses, -500.00, transactionDate, "Buy Keychron Keyboard", TransactMethod.CreditCard);
            transfer1 = new Transaction(TransactType.Transfer, -200.00, transactionDate, "Birthday Present for Yourself", testAccount1, testAccount2);
            transfer2 = new Transaction(TransactType.Transfer, -300.00, transactionDate, "Birthday Present for Mother", testAccount1, testAccount2);
            invalidTransfer = new Transaction(TransactType.Transfer, -100000.00, transactionDate, "Birthday Present for President Xi", testAccount1, testAccount2);
        }

        [Test()]
        public void TestLinkUnlinkCard()
        {
            // link card
            string expectedResult1 = $"Standard Chartered Credit Card(1111222233334444) has been linked to account 1234123456785678\n";
            Assert.AreEqual(expectedResult1, testAccount1.LinkCard(testCCard));
            Assert.AreEqual(testAccount1.Card, testCCard);

            // unlink card
            string expectedResult2 = $"Standard Chartered Credit Card(1111222233334444) has been unlinked from account 1234123456785678\n";
            Assert.AreEqual(expectedResult2, testAccount1.UnlinkCard());
            Assert.AreEqual(testAccount1.Card, null);
        }

        [Test()]
        public void TestAddTransaction()
        {
            // situation 1: using online banking, income
            Assert.AreEqual("Transaction Successful", testAccount1.AddTransaction(incomeO));
            // situation 2: using credit card, expenses
            testAccount1.LinkCard(testCCard);
            Assert.AreEqual("Transaction Successful", testAccount1.AddTransaction(expenseC));

            // check current credit limit & account balance
            Credit card = (Credit)testAccount1.Card!;
            Assert.AreEqual(5600, testAccount1.Balance);
            Assert.AreEqual(500, card.CurrentCreditLimit);
        }

        [Test()]
        public void TestEditTransaction()
        {
            // situation 1: using online banking, income -> expenses
            testAccount1.AddTransaction(incomeO);
            Assert.AreEqual(
                "Transaction Updated Successful",
                testAccount1.EditTransaction(incomeO, expenseO));

            // check account balance
            Assert.AreEqual(4000, testAccount1.Balance);
            // check updated transaction data
            Assert.AreEqual(expenseO, testAccount1.Transactions[0]);

            // situation 2: using credit card, expenses -> income
            testAccount1.LinkCard(testCCard);
            testAccount1.AddTransaction(expenseC);
            Assert.AreEqual(
                "Transaction Updated Successful",
                testAccount1.EditTransaction(expenseC, incomeO));

            // check current credit limit & account balance
            Credit card = (Credit)testAccount1.Card!;
            Assert.AreEqual(1000, card.CreditLimit);
            Assert.AreEqual(4600, testAccount1.Balance);
            // check updated transaction data
            Assert.AreEqual(incomeO, testAccount1.Transactions[1]);
        }

        [Test()]
        public void TestAddTransfer()
        {
            Assert.AreEqual("Transaction Successful", testAccount1.AddTransfer(transfer1));
            Assert.AreEqual(4800, testAccount1.Balance);
            Assert.AreEqual(5200, testAccount2.Balance);

            Assert.AreEqual("Transaction Failed", testAccount1.AddTransaction(invalidTransfer));
            Assert.AreEqual(4800, testAccount1.Balance);
            Assert.AreEqual(5200, testAccount2.Balance);
        }

        [Test()]
        public void TestEditTransfer()
        {
            Assert.AreEqual("Transaction Successful", testAccount1.AddTransfer(transfer1));
            Assert.AreEqual(4800, testAccount1.Balance);
            Assert.AreEqual(5200, testAccount2.Balance);

            Assert.AreEqual("Transaction Updated Failed", testAccount1.EditTransfer(transfer1, invalidTransfer));
            Assert.AreEqual(4800, testAccount1.Balance);
            Assert.AreEqual(5200, testAccount2.Balance);

            Assert.AreEqual("Transaction Updated Successful", testAccount1.EditTransfer(transfer1, transfer2));
            Assert.AreEqual(4700, testAccount1.Balance);
            Assert.AreEqual(5300, testAccount2.Balance);
        }

        [Test()]
        public void TestDeleteTransaction()
        {
            // situation 1: using online banking, income -> expenses
            testAccount1.AddTransaction(incomeO);
            // situation 2: using credit card, expenses -> income
            testAccount1.LinkCard(testCCard);
            Credit card = (Credit)testAccount1.Card!;
            testAccount1.AddTransaction(expenseC);

            string actualResult1 = testAccount1.DeleteTransaction(incomeO);
            string actualResult2 = testAccount1.DeleteTransaction(expenseC);

            // check status message
            Assert.AreEqual(actualResult1, "Transaction Deleted Successful");
            Assert.AreEqual(actualResult2, "Transaction Deleted Successful");
            // check current credit limit & account balance
            Assert.AreEqual(5000, testAccount1.Balance);
            Assert.AreEqual(1000, card.CurrentCreditLimit);
            CollectionAssert.DoesNotContain(testAccount1.Transactions, incomeO);
            CollectionAssert.DoesNotContain(testAccount1.Transactions, expenseC);
        }

        [Test()]
        public void TestTransactionsInfo()
        {
            testAccount1.AddTransaction(incomeO);

            string expectedResult = "1.\n";
            expectedResult += $"Transaction Type: Income\n";
            expectedResult += $"Transaction Amount: 600\n";
            expectedResult += $"Transaction Method: OnlineBanking\n";
            expectedResult += $"Transaction Date: 4/29/2023 2:30:37 PM\n";
            expectedResult += $"Transaction Description: Sell Keychron Keyboard\n";

            Assert.AreEqual(expectedResult, testAccount1.TransactionsInfo());
        }

        [Test()]
        public void TestAccountInfo()
        {
            string expectedResult = "Account No: 1234123456785678\nBank Associated: Standard Chartered\nBalance: 5000\n";
            expectedResult += "Card linked: (no card linked)\n";
            Assert.AreEqual(expectedResult, testAccount1.AccountInfo());
        }
    }
}