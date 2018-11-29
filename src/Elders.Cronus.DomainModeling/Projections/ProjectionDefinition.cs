using System;
using System.Collections.Generic;
using System.Linq;

namespace Elders.Cronus.Projections
{
    public class ContinueId : IBlobId
    {
        public byte[] RawId => null;
    }

    public class SubscriptionIdResolver
    {
        public SubscriptionIdResolver(Type projectionType, Func<IEvent, IBlobId> idResolver)
        {
            ProjectionType = projectionType;
            IdResolver = idResolver;
        }

        public Type ProjectionType { get; private set; }

        public Func<IEvent, IBlobId> IdResolver { get; private set; }
    }

    public abstract class ProjectionDefinition<TState, TId> : IProjectionDefinition, IProjection
        where TState : new()
        where TId : IBlobId
    {
        Dictionary<Type, List<Func<IEvent, IBlobId>>> subscriptionResolvers;

        public ProjectionDefinition()
        {
            ((IProjectionDefinition)this).State = new TState();
            subscriptionResolvers = new Dictionary<Type, List<Func<IEvent, IBlobId>>>();
        }

        public TId ProjectionId { get { return (TId)((IHaveState)this).Id; } private set { ((IHaveState)this).Id = value; } }

        public TState State { get { return (TState)((IHaveState)this).State; } private set { ((IHaveState)this).State = value; } }

        IBlobId IHaveState.Id { get; set; }

        object IHaveState.State { get; set; }

        IEnumerable<IBlobId> IProjectionDefinition.GetProjectionIds(IEvent @event)
        {
            foreach (var subscriptionResolver in subscriptionResolvers[@event.GetType()])
            {
                IBlobId resolvedId = subscriptionResolver(@event);
                if (resolvedId is ContinueId) continue;
                if (ReferenceEquals(null, resolvedId)) continue; //TODO LOG

                yield return resolvedId;
            }
        }

        void IProjectionDefinition.Apply(IEvent @event)
        {
            ((dynamic)this).Handle((dynamic)@event);
        }

        void IAmEventSourcedProjection.ReplayEvents(IEnumerable<IEvent> events)
        {
            var projection = this as IProjectionDefinition;
            foreach (IEvent @event in events)
            {
                projection.Apply(@event);
            }
        }

        void IHaveState.InitializeState(IBlobId projectionId, object state)
        {
            ((IHaveState)this).Id = projectionId;
            ((IHaveState)this).State = state ?? new TState();
        }

        protected ProjectionDefinition<TState, TId> Subscribe<TEvent>(Func<TEvent, IBlobId> projectionId)
            where TEvent : IEvent
        {
            Type eventType = typeof(TEvent);

            if (subscriptionResolvers.ContainsKey(eventType))
                subscriptionResolvers[eventType].Add(x => projectionId((TEvent)x));
            else
                subscriptionResolvers.Add(typeof(TEvent), new List<Func<IEvent, IBlobId>>() { x => projectionId((TEvent)x) });

            return this;
        }

        protected ProjectionDefinition<TState, TId> Subscribe<TEvent>(Func<TEvent, TId> projectionId)
            where TEvent : IEvent
        {
            Type eventType = typeof(TEvent);

            if (subscriptionResolvers.ContainsKey(eventType))
                subscriptionResolvers[eventType].Add(x => projectionId((TEvent)x));
            else
                subscriptionResolvers.Add(typeof(TEvent), new List<Func<IEvent, IBlobId>>() { x => projectionId((TEvent)x) });

            return this;
        }

        protected IBlobId Continue()
        {
            return new ContinueId();
        }
    }
}
