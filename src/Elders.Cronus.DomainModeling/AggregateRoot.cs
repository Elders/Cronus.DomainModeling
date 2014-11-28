using System.Collections.Generic;

namespace Elders.Cronus.DomainModeling
{
    public class AggregateRoot<ST> : IAggregateRoot
        where ST : IAggregateRootState, new()
    {
        protected ST state;
        protected List<IEvent> uncommittedEvents;
        private int revision;

        public AggregateRoot()
        {
            state = new ST();
            uncommittedEvents = new List<IEvent>();
            revision = 0;
        }

        IAggregateRootState IAggregateRootStateManager.State { get { return state; } }

        IEnumerable<IEvent> IAggregateRoot.UncommittedEvents { get { return uncommittedEvents.AsReadOnly(); } }

        int IAggregateRoot.Revision { get { return uncommittedEvents.Count > 0 ? revision + 1 : revision; } }

        void IAggregateRootStateManager.BuildStateFromHistory(List<IEvent> events, int revision)
        {
            state = new ST();
            foreach (IEvent @event in events)
            {
                state.Apply(@event);
            }
            this.revision = revision;

            if (state.Id.RawId == default(byte[]))
                throw new AggregateRootException("Invalid aggregate root state. The initial event which created the aggregate root is missing.");
        }

        protected void Apply(IEvent @event)
        {
            state.Apply(@event);
            uncommittedEvents.Add(@event);
        }
    }
}