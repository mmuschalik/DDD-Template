using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Domain.Model
{
    public class AccountActivated : DomainEvent
    {
        public readonly string AccountId;
        public readonly DateTime ActivatedDate;
        public readonly string UserName;

        public AccountActivated(string accountId, DateTime activatedDate, string userName)
        {
            AccountId = accountId;
            ActivatedDate = activatedDate;
            UserName = userName;
        }
    }
}
