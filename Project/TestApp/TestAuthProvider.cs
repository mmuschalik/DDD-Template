using Domain.Common;
using Domain.Common.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public class TestAuthProvider : IAuthProvider
    {
        public Domain.Common.IAuthSession Authenticate(Domain.Common.Credentials credentials)
        {
            var session = new AuthSession(new AuthToken(Guid.NewGuid().ToString()), credentials.UserName, credentials.UserName, Enumerable.Empty<string>(),  Enumerable.Empty<string>());
            return session;
        }

        public Domain.Common.IAuthSession Authenticate(Domain.Common.AuthToken token)
        {
            throw new NotImplementedException();
        }

        public bool Logout(Domain.Common.IAuthSession session)
        {
            throw new NotImplementedException();
        }
    }
}
