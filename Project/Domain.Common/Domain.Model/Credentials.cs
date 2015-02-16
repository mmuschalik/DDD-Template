using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class Credentials
    {
        public string UserName { get; private set; }
        public string Password { get; private set; }

        public Credentials(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

    }
}
