using System.Collections.Generic;

namespace Elders.Cronus.DomainModeling
{
    public class AggregateRoot<TState> : IAggregateRoot
        where TState : IAggregateRootState, new()
    {
        protected TState state;
        protected List<IEvent> uncommittedEvents;
        private int revision;

        public AggregateRoot()
        {
            state = new TState();
            uncommittedEvents = new List<IEvent>();
            revision = 0;
        }

        IAggregateRootState ICanRestoreStateFromEvents<IAggregateRootState>.State { get { return state; } }

        IEnumerable<IEvent> IAggregateRoot.UncommittedEvents { get { return uncommittedEvents.AsReadOnly(); } }

        int IAggregateRoot.Revision { get { return uncommittedEvents.Count > 0 ? revision + 1 : revision; } }

        void ICanRestoreStateFromEvents<IAggregateRootState>.ReplayEvents(List<IEvent> events, int revision)
        {
            state = new TState();
            foreach (IEvent @event in events)
            {
                state.Apply(@event);
            }
            this.revision = revision;

            if (state.Id == null || state.Id.RawId == default(byte[]))
                throw new AggregateRootException("Invalid aggregate root state. The initial event which created the aggregate root is missing.");
        }

        protected void Apply(IEvent @event)
        {
            state.Apply(@event);
            uncommittedEvents.Add(@event);
        }
    }
}