using System;
using System.Collections.Generic;

namespace PassTask
{
    public abstract class Card
    {
        private string _cardNo;
        private DateTime _expiryDate;
        private string _cardName;

        public string CardNo
        {
            get
            {
                return _cardNo;
            }
            set
            {
                _cardNo = value;
            }
        }
        public DateTime ExpiryDate
        {
            get
            {
                return _expiryDate;
            }
            set
            {
                _expiryDate = value;
            }
        }
        public string CardName
        {
            get
            {
                return _cardName;
            }
            set
            {
                _cardName = value;
            }
        }
        /// <summary>
        /// This is a default constructor for the Card class that initializes the card number, expiry date, and card name.
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="expiryDate"></param>
        /// <param name="cardName"></param>
        public Card(string cardNo, DateTime expiryDate, string cardName)
        {
            _cardNo = cardNo;
            _expiryDate = expiryDate;
            _cardName = cardName;
        }
        /// <summary>
        /// This is a virtual method which returns a string containing information about the card, including its name, number, and expiry date
        /// </summary>
        /// <returns></returns>
        public virtual string CardInfo()
        {
            string cardInfo = $"Card Name: {_cardName}\n";
            cardInfo += $"Card No: {_cardNo}\n";
            cardInfo += $"Expiry Date: {_expiryDate}\n";
            return cardInfo;
        }
    }
}