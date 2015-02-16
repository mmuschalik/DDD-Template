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
        public AuthToken AuthenticatedToken { get; private set; }
        public string UserId { get; private set; }

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

        public AuthSession(AuthToken sessionId, string displayName, string UserId, IEnumerable<string> permissions, IEnumerable<string> roles)
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

        public void EnsureHasPermission(string permission)
        {
            if (!_permissions.Contains(permission))
                throw new UnauthorizedAccessException(string.Format("User {0} does not have permission '{1}'", DisplayName, permission));
        }

        public void EnsureHasRole(string role)
        {
            if (!_roles.Contains(role))
                throw new UnauthorizedAccessException(string.Format("User {0} is not a member of role '{1}'", DisplayName, role));
        }
    }
}
