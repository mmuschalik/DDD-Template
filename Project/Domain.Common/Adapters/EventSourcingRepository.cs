using Domain.Common.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Adapters
{
    public class EventSourcingRepository<T> : IRepository<T> where T : AggregateRoot
    {
        private IEventStore _eventStore;

        public EventSourcingRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public T GetById(string key)
        {
            var events = _eventStore.GetAllDomainEventsForStream(typeof(T).Name + "-" + key).ToList();
            var agg = (T)Activator.CreateInstance(typeof(T), true);

            agg.LoadFromHistory(events);
            agg.SurrogateId = events.Count;

            return agg;
        }

        public void Save(T item)
        {
            _eventStore.AppendToStream(item.GetType().Name + "-" + item.Id, item.GetUncommittedEvents());
        }
    }
}
