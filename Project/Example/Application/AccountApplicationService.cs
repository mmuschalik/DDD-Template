using Domain.Common;
using Domain.Common.Infrastructure;
using Example.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Application
{
    public class AccountApplicationService
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly IAuthSession _session;

        public AccountApplicationService(IRepository<Account> accountRepository, IAuthSession session)
        {
            _accountRepository = accountRepository;
            _session = session;
        }

        public void CreateNewAccount(int accountNumber, string accountName, string firstName, string surName, string email, string homePhone)
        {
            var account = new Account(accountNumber, accountName, new Contact(new Person(firstName, surName), new Address(), email, homePhone));

            _accountRepository.Save(account);
        }


        public AccountView GetAccountDetailsForAccountNumber(int accountNumber)
        {
            var account = _accountRepository.GetById(accountNumber.ToString());

            return new AccountView { AccountName = account.AccountName, 
                AccountNumber = account.AccountNumber, 
                ActivationDate = account.ActivationDate, 
                BankAccounts = account.BankAccounts.ToList(), 
                ContactPerson = account.ContactPerson };
        }

        public void ActivateAccount(int accountNumber)
        {
            var account = _accountRepository.GetById(accountNumber.ToString());

            account.ActivateAccount(_session.UserId);

            _accountRepository.Save(account);
        }

        public void AddBankDetailsToAccount(int accountNumber, string bankName, int bsb, int bankAccountNumber, string accountName)
        {
            var account = _accountRepository.GetById(accountNumber.ToString());

            account.AddBankAccount(new BankAccount(bankName, bsb, bankAccountNumber, accountName), _session.UserId);

            _accountRepository.Save(account);
        }

        public void AddBeneficiary(int accountNumber, string firstName, string surName, decimal allocation)
        {
            var account = _accountRepository.GetById(accountNumber.ToString());

            account.AddBeneficiary(new Beneficiary(new Person(firstName, surName), allocation), _session.UserId);

            _accountRepository.Save(account);
        }

    }
}
