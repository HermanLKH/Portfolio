using System;
using System.Collections.Generic;

namespace PassTask
{
    public class Credit : Card
    {
        /// <summary>
        /// This method suggests a credit card with enough credit limit for the transaction amount and with lower interest rate.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="transactAmount"></param>
        /// <returns></returns>
        public static Credit? SuggestCreditCard(User user, double transactAmount)
        {
            Credit? candidateCard = null;

            foreach (Account a in user.Accounts)
            {
                if (a.Card is Credit card)
                {
                    if (card.CurrentCreditLimit >= Math.Abs(transactAmount))
                    {
                        if (candidateCard != null)
                        {
                            if (card.InterestRate >= candidateCard.InterestRate)
                            {
                                continue;
                            }
                        }
                        candidateCard = card;
                        Console.WriteLine(candidateCard.CardNo + " " + candidateCard.CurrentCreditLimit);
                    }
                }
            }
            return candidateCard;
        }

        private double _creditLimit;
        private double _currentCreditLimit;
        private double _interestRate;
        private CreditType _creditType;
        private DateTime _paymentDue;
        public double CreditLimit
        {
            get
            {
                return _creditLimit;
            }
            set
            {
                _creditLimit = value;
            }
        }
        public double CurrentCreditLimit
        {
            get
            {
                return _currentCreditLimit;
            }
            set
            {
                _currentCreditLimit = value;
            }
        }
        public double InterestRate
        {
            get
            {
                return _interestRate;
            }
            set
            {
                _interestRate = value;
            }
        }
        public CreditType CreditType
        {
            get
            {
                return _creditType;
            }
            set
            {
                _creditType = value;
            }
        }
        public DateTime PaymentDue
        {
            get
            {
                return _paymentDue;
            }
            set
            {
                _paymentDue = value;
            }
        }
        /// <summary>
        /// This is a default constructor for the Credit class, which initializes the credit card properties.
        /// </summary>
        /// <param name="creditLimit"></param>
        /// <param name="interestRate"></param>
        /// <param name="creditType"></param>
        /// <param name="paymentDue"></param>
        /// <param name="cardNo"></param>
        /// <param name="expiryDate"></param>
        /// <param name="cardName"></param>
        /// <returns></returns>
        public Credit(double creditLimit, double interestRate, CreditType creditType, DateTime paymentDue, string cardNo, DateTime expiryDate, string cardName) : base(cardNo, expiryDate, cardName)
        {
            _creditLimit = creditLimit;
            _currentCreditLimit = creditLimit;
            _interestRate = interestRate;
            _creditType = creditType;
            _paymentDue = paymentDue;
        }
        /// <summary>
        /// This method returns a string containing information about a credit card.
        /// </summary>
        /// <returns></returns>
        public override string CardInfo()
        {
            string cardInfo = base.CardInfo();
            cardInfo += $"Credit Limit: ${_creditLimit}\n";
            cardInfo += $"Current Credit Limit: ${_currentCreditLimit}\n";
            cardInfo += $"Interest Rate: {_interestRate}%\n";
            cardInfo += $"Credit Type: {_creditType}\n";
            cardInfo += $"Payment Due: {_paymentDue}\n";
            return cardInfo;
        }
    }
}