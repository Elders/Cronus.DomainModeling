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

        IAggregateRootState IAggregateRootStateManager.BuildStateFromHistory(List<IEvent> events, int revision)
        {
            var stateFromHistory = new ST();
            foreach (IEvent @event in events)
            {
                stateFromHistory.Apply(@event);
            }
            this.revision = revision;
            return stateFromHistory;
        }

        void IAggregateRoot.Apply(IEvent @event)
        {
            state.Apply(@event);
            uncommittedEvents.Add(@event);
        }
    }
}