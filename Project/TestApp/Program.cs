using Domain.Common;
using Domain.Common.Application;
using Domain.Common.Domain.Model;
using Domain.Common.Infrastructure;
using Example.Application;
using Example.Domain.Model;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentials = new Credentials("admin", "adminpassword");
            Register.Authenticate(credentials);

            CreateNewAccount();
            AddBankDetails();
            ActivateAccount();
            AddBeneficiary();
            
            var accountDetails = GetAccountDetails();
        }

        static void CreateNewAccount()
        {
            Register.
                AccountApplicationService.
                CreateNewAccount(1000, "maurice muschalik", "maurice", "muschalik", "maurice.muschalik@gmail.com", "987654321");
        }

        static void AddBankDetails()
        {
            Register.
                AccountApplicationService.
                AddBankDetailsToAccount(1000, "CBA", 123123, 1000, "maurice's CBA account");
        }

        static void ActivateAccount()
        {
            Register.
                AccountApplicationService.
                ActivateAccount(1000);
        }

        static AccountView GetAccountDetails()
        {
            return Register.
                AccountApplicationService.
                GetAccountDetailsForAccountNumber(1000);
        }

        static void AddBeneficiary()
        {
            Register.
                AccountApplicationService.
                AddBeneficiary(1000, "reika", "muschalik", (decimal)1.0);

            Register.
                AccountApplicationService.
                AddBeneficiary(1000, "sophie", "muschalik", (decimal)0.5);
        }

    }
}
