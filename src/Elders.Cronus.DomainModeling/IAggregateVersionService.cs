using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elders.Cronus.DomainModeling
{
    public interface IAggregateVersionService : IDisposable
    {
        int ReserveVersion(IAggregateRootId aggregateId, int requestedVersion);
    }
}