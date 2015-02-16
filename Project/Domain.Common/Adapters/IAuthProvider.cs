using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Infrastructure
{
    public interface IAuthProvider
    {
        IAuthSession Authenticate(Credentials credentials);

        IAuthSession Authenticate(AuthToken token);

        bool Logout(IAuthSession session);
    }
}
