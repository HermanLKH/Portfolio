using System;
using System.Collections.Generic;

namespace PassTask
{
    public class Debit : Card
    {
        /// <summary>
        /// This is a default constructor for the Debit class, which inherits from the Card class.
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="expiryDate"></param>
        /// <param name="cardName"></param>
        /// <returns></returns>
        public Debit(string cardNo, DateTime expiryDate, string cardName) : base(cardNo, expiryDate, cardName) { }
    }
}