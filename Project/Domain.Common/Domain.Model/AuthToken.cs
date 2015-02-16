using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class AuthToken
    {
        public string SessionId { get; private set; }

        public AuthToken(string sessionId)
        {
            SessionId = sessionId;
        }
    }
}
