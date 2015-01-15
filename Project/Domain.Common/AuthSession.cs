using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class AuthSession : IAuthSession
    {
        private IDictionary<string, object> _properties;
        private HashSet<string> _permissions;
        private HashSet<string> _roles;

        public string DisplayName { get; private set; }
        public bool IsAuthenticated { get; private set; }
        public object SessionId { get; private set; }
        public object UserId { get; private set; }

        public object this[string key]
        {
            get
            {
                return _properties[key];
            }
            set
            {
                _properties[key] = value;
            }
        }

        public AuthSession(object sessionId, string displayName, object UserId, IEnumerable<string> permissions, IEnumerable<string> roles)
        {
            _properties = new Dictionary<string, object>();
            _permissions = new HashSet<string>();
            _roles = new HashSet<string>();

            foreach (var p in permissions)
                _permissions.Add(p);

            foreach (var r in roles)
                _roles.Add(r);
        }
        
        public bool HasPermission(string permission)
        {
            return _permissions.Contains(permission);
        }

        public bool HasRole(string role)
        {
            return _roles.Contains(role);
        }
    }
}
