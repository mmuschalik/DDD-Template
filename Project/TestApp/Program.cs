using Domain.Common;
using Domain.Common.Application;
using Domain.Common.Domain.Model;
using Domain.Common.Infrastructure;
using Project.Adapters.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var agg = new TestAgg("item1", "name1");
            //var repo = new PostgresqlRepository<TestAgg>("Server=127.0.0.1;Port=5432;Database=test;User Id=postgres;Password=admin");
            //repo.Save(agg);
            //repo.Save(agg);
            //repo.Save(agg);

            //agg.ChangeName("name3");
            //agg.ChangeName("name4");
            //agg.ChangeName("name5");
            //repo.Save(agg);

            var es = new PostgresqlEventStore("Server=127.0.0.1;Port=5432;Database=test;User Id=postgres;Password=admin");

            var repo1 = new PostgresqlRepository<DomainEventsPublishedTracker>("Server=127.0.0.1;Port=5432;Database=test;User Id=postgres;Password=admin");
            var bus = new ActionDictionaryBus();

            var s = new DomainEventPublisherService(repo1, es, bus);
            s.Publish("channel1");
        }
    }

    public class TestAgg : AggregateRoot
    {
        public TestAgg(string id, string name) : base(id)
        {
            Name = name;
        }

        public string Name { get; set; }

        public void ChangeName(string newname)
        {
            Name = newname;
            RaiseEvent(new TestAggNameChanged(Id, newname));
        }
    }

    public class TestAggNameChanged : DomainEvent
    {
        public string Id { get; private set; }
        public string NameChanged { get; private set; }

        public TestAggNameChanged(string id, string namechanged)
        {
            Id = id;
            NameChanged = namechanged;
        }
    }
}
