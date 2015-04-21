using Domain.Common;
using Domain.Common.Infrastructure;
using Example.Application;
using Example.Domain.Model;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public class Register
    {
        private static IKernel _kernel = null;

        private static IKernel Kernel
        {
            get
            {
                if (_kernel == null)
                {
                    _kernel = new StandardKernel();
                    _kernel.Load(ConfigurationManager.AppSettings["binding"]);
                    _kernel.Bind<IAuthProvider>().To<TestAuthProvider>();
                }
                return _kernel;
            }
        }

        public static IRepository<Account> AccountRepository
        {
            get
            {
                return Kernel.Get<IRepository<Account>>();
            }
        }

        public static AccountApplicationService AccountApplicationService
        {
            get
            {
                return Kernel.Get<AccountApplicationService>();
            }
        }

        public static IAuthSession Session
        {
            get
            {
                return Kernel.Get<IAuthSession>();
            }
        }

        public static void Authenticate(Credentials credentials)
        {
            var session = Kernel.Get<IAuthProvider>().Authenticate(credentials);
            Kernel.Bind<IAuthSession>().ToConstant(session);
        }

    }
}
