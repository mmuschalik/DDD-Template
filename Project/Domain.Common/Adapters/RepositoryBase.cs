using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Adapters
{
    public abstract class RepositoryBase
    {
        protected void SetSurrogateId(AggregateRoot root, long surrogateId)
        {
            root.SurrogateId = surrogateId;
        }

        protected void SetVersion(AggregateRoot root, int version)
        {
            root.Version = version;
        }

        public void EventsCommitted(AggregateRoot root)
        {
            root.EventsCommitted();
        }
    }
}
