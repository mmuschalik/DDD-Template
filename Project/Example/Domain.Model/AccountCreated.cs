using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Domain.Model
{
    public class AccountCreated : DomainEvent
    {
        public readonly string Id;
        public readonly string AccountName;
        public readonly Contact ContactPerson;
        public readonly IEnumerable<BankAccount> BankAccounts;

        public AccountCreated(string id, string accountName, Contact contactPerson, DateTime? activationDate, IEnumerable<BankAccount> bankAccounts)
        {
            Id= id;
            AccountName = accountName;
            ContactPerson = contactPerson;
            BankAccounts = bankAccounts;
        }
    }
}
