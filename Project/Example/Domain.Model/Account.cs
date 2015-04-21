using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Domain.Model
{
    public class Account : AggregateRoot
    {
        private ISet<BankAccount> _bankAccounts;
        private ISet<Beneficiary> _beneficiaries;

        public Account(int accountNumber, string accountName, Contact contactPerson, DateTime? activationDate = null, ISet<BankAccount> bankAccounts = null, ISet<Beneficiary> beneficiaries = null)
            : base(accountNumber.ToString())
        {
            Ensure.Positive(accountNumber, "accountNumber");
            Ensure.NotNullOrEmpty(accountName, "accountName");
            Ensure.NotNull(contactPerson, "accountName");

            AccountName = accountName;
            AccountNumber = accountNumber;
            ContactPerson = contactPerson;
            ActivationDate = activationDate;
            _bankAccounts = bankAccounts == null ? new HashSet<BankAccount>() : bankAccounts;
            _beneficiaries = beneficiaries == null ? new HashSet<Beneficiary>() : beneficiaries;
        }

        public int AccountNumber { get; private set; }

        public string AccountName { get; private set; }

        public DateTime? ActivationDate { get; private set; }

        public Contact ContactPerson { get; private set; }

        public IEnumerable<BankAccount> BankAccounts { get { return _bankAccounts; } }

        public IEnumerable<Beneficiary> Beneficiaries { get { return _beneficiaries; } }

        public void ActivateAccount(string userName)
        {
            if (!ActivationDate.HasValue)
            {
                ActivationDate = DateTime.Now;
                RaiseEvent(new AccountActivated(Id, ActivationDate.Value, userName));
            }
            else
                throw new Exception("Account is already activated");
        }

        public void AddBankAccount(BankAccount bankAccount, string userName)
        {
            if (_bankAccounts.Add(bankAccount))
                RaiseEvent(new BankAccountAdded(Id, bankAccount, userName));
            else
                throw new Exception("Bank Account already exists");
        }

        public void AddBeneficiary(Beneficiary beneficiary, string userName)
        {
            if (_beneficiaries.Count >= 4)
                throw new Exception("No more than 4 beneficiaries allowed");
            else
            {
                _beneficiaries = ReallocateBeneficiaries(beneficiary);
                RaiseEvent(new BeneficiaryAdded(Id, beneficiary, userName));
            }
        }

        private ISet<Beneficiary> ReallocateBeneficiaries(Beneficiary beneficiary)
        {
            var updatedBenefeciaries = new HashSet<Beneficiary>();

            var reduceBy = _beneficiaries.Count == 0 ? 0m : (beneficiary.Allocation / (decimal)(_beneficiaries.Count));

            foreach (var b in _beneficiaries)
                updatedBenefeciaries.Add(new Beneficiary(b.Person, b.Allocation - reduceBy));

            updatedBenefeciaries.Add(beneficiary);

            var sum = updatedBenefeciaries.Sum(b => b.Allocation);
            Ensure.IsExactly(sum, 1.0m, "total allocation");

            return updatedBenefeciaries;
        }

        protected override DomainEvent CreateAggregateDomainEvent()
        {
            return new AccountCreated(Id, AccountName, ContactPerson, ActivationDate, BankAccounts);
        }
    }
}
