using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Domain.Model
{
    public class BeneficiaryAdded : DomainEvent
    {
        public readonly string AccountId;
        public readonly Beneficiary Beneficiary;
        public readonly string UserName;

        public BeneficiaryAdded(string accountId, Beneficiary beneficiary, string userName)
        {
            AccountId = accountId;
            Beneficiary = beneficiary;
            UserName = userName;
        }
    }
}
