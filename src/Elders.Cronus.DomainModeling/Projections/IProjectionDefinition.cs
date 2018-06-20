using System.Collections.Generic;

namespace Elders.Cronus.Projections
{
    public interface IProjectionDefinition : IHaveState, IAmEventSourcedProjection
    {
        /// <summary>
        /// Gets all projection identifiers based on an event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        IEnumerable<IBlobId> GetProjectionIds(IEvent @event);

        void Apply(IEvent @event);
    }

}
