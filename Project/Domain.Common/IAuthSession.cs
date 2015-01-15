using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public interface IAuthSession
    {
        object SessionId { get; }
        string DisplayName { get; }
        object UserId { get; }

        bool IsAuthenticated { get; }
        bool HasRole(string role);
        bool HasPermission(string permission);

        object this[string key] { get; set; }
    }
}
