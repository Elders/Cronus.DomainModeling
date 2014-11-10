using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elders.Cronus.DomainModeling
{
    public interface IEventHandler<in T>
        where T : IEvent
    {
        void Handle(T @event);
    }
}
