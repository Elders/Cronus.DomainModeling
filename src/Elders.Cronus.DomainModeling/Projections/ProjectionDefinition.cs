using System;
using System.Collections.Generic;

namespace Elders.Cronus.Projections
{
    // This is a concept which we might use in future.
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
        private readonly Dictionary<Type, List<Func<IEvent, IBlobId>>> subscriptionResolvers;

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
                if (resolvedId is null) continue;

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
                subscriptionResolvers[eventType].Add(Safe(projectionId));
            else
                subscriptionResolvers.Add(typeof(TEvent), new List<Func<IEvent, IBlobId>>() { Safe(projectionId) });

            return this;

            static Func<IEvent, IBlobId> Safe(Func<TEvent, IBlobId> projectionId)
            {
                return x =>
                {
                    try { return projectionId((TEvent)x); }
                    catch (Exception ex)
                    {
                        // TODO: Add Warn log ex
                        return default;
                    }
                };
            }
        }

        // Used by replay projection atm
        protected ProjectionDefinition<TState, TId> Subscribe<TEvent>(Func<TEvent, TId> projectionId)
            where TEvent : IEvent
        {
            Type eventType = typeof(TEvent);

            if (subscriptionResolvers.ContainsKey(eventType))
                subscriptionResolvers[eventType].Add(Safe(projectionId));
            else
                subscriptionResolvers.Add(typeof(TEvent), new List<Func<IEvent, IBlobId>>() { Safe(projectionId) });

            return this;

            static Func<IEvent, IBlobId> Safe(Func<TEvent, TId> projectionId)
            {
                return x =>
                {
                    try { return projectionId((TEvent)x); }
                    catch (Exception ex)
                    {
                        // TODO: Add Warn log ex
                        return default;
                    }
                };
            }
        }
    }
}
