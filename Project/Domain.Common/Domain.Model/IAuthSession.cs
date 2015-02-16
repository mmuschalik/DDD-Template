using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public interface IAuthSession
    {
        AuthToken AuthenticatedToken { get; }
        string DisplayName { get; }
        string UserId { get; }

        bool HasRole(string role);
        bool HasPermission(string permission);

        void EnsureHasPermission(string permission);
        void EnsureHasRole(string role);


        object this[string key] { get; set; }
    }
}
