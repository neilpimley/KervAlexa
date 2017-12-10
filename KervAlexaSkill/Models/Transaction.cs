using System;
using Kerv.Common;

namespace Kerv
{
    public class Transaction
    {
        public enum TransactionType { Credit, Debit };

        public DateTime Date { get; set; }
        public String Device { get; set; }
        public String Description { get; set; }
        public Money Amount { get; set; }
        public TransactionType Type {
            get
            {
                return Description.StartsWith("Load to Kerv") ?
                                       TransactionType.Credit : TransactionType.Debit;
            }
        }

    }
}
