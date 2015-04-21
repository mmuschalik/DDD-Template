using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Domain.Model
{
    public class BankAccount
    {
        public readonly string BankName;
        public readonly int BSB;
        public readonly int AccountNumber;
        public readonly string AccountName;

        public BankAccount(string bankName, int bsb, int accountNumber, string accountName)
        {
            Ensure.NotNullOrEmpty(bankName, "bankName");
            Ensure.Between(bsb, 100000, 999999, "bsb");
            Ensure.Positive(accountNumber, "accountNumber");
            Ensure.NotNullOrEmpty(accountName, "accountName");

            BankName = bankName;
            BSB = bsb;
            AccountNumber = accountNumber;
            AccountName = accountName;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BankAccount))
                return false;

            var ba = (BankAccount)obj;

            return BSB == ba.BSB && AccountNumber == ba.AccountNumber;
        }

        public override int GetHashCode()
        {
            return BSB.GetHashCode() ^ AccountNumber.GetHashCode();
        }
    }
}
