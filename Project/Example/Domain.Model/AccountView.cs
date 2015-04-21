using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Domain.Model
{
    public class AccountView
    {
        public int AccountNumber { get; set; }

        public string AccountName { get; set; }

        public DateTime? ActivationDate { get; set; }

        public Contact ContactPerson { get; set; }

        public List<BankAccount> BankAccounts { get; set; }
    }
}
