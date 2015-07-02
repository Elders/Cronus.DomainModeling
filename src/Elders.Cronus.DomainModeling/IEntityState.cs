using System;
using System.Runtime.Serialization;

namespace Elders.Cronus.DomainModeling
{

    public interface IEntityState
    {
        IEntityId EntityId { get; }
    }
}