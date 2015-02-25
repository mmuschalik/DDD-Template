using Domain.Common;
using Project.Adapters.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var agg = new TestAgg("item1", "name1");
            var repo = new PostgresqlRepository<TestAgg>("Server=127.0.0.1;Port=5432;Database=test;User Id=postgres;Password=admin;Enlist=true");
            repo.Save(agg);

            
            agg.ChangeName("name3");
            agg.ChangeName("name4");
            agg.ChangeName("name5");
            repo.Save(agg);

            var es = new PostgresqlEventStore("Server=127.0.0.1;Port=5432;Database=test;User Id=postgres;Password=admin;Enlist=true");
            var ev = es.GetAllDomainEventsSince(0);
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
