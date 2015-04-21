using Domain.Common.Domain.Model;
using Domain.Common.Infrastructure;
using Example.Domain.Model;
using Ninject.Modules;
using Project.Adapters.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Domain.Common.Application;
using Domain.Common.Adapters;
using Example.Application;

namespace NinjectBinding
{
    public class NinjectBindings : NinjectModule, INinjectModule
    {
        public override void Load()
        {
            //this.Bind<IRepository<TestAgg>>().To<EventSourcingRepository<TestAgg>>();
            this.Bind<IRepository<Account>>().To<PostgresqlRepository<Account>>().
                WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["DocumentStore"].ConnectionString);
            this.Bind<AccountApplicationService>().ToSelf();
            this.Bind<IEventStore>().To<PostgresqlEventStore>().
                WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["DocumentStore"].ConnectionString);
            this.Bind<IRepository<DomainEventsPublishedTracker>>().To<PostgresqlRepository<DomainEventsPublishedTracker>>().
                WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["DocumentStore"].ConnectionString);
            this.Bind<IBus>().To<ActionDictionaryBus>();
            this.Bind<DomainEventPublisherService>().ToSelf();
        }
    }
}
