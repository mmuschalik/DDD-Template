using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Domain.Model
{
    public class BankAccountAdded : DomainEvent
    {
        public readonly string AccountId;
        public readonly BankAccount BankAccount;
        public readonly string UserName;

        public BankAccountAdded(string accountId, BankAccount bankAccount, string userName)
        {
            AccountId = accountId;
            BankAccount = bankAccount;
            UserName = userName;
        }
    }
}
