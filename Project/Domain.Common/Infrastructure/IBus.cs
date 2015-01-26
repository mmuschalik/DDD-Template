using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Infrastructure
{
    public interface IBus
    {
        void Subscribe<T>(Action<T> action) where T : IMessage;

        void Send<T>(T command) where T : Command;

        void Publish<T>(T @event) where T : DomainEvent;
    }
}
