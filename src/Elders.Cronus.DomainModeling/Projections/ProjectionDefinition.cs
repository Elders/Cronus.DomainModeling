using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elders.Cronus.Projections;

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

public abstract class ProjectionDefinition<TState, TId, TPartitionDefiner> : IProjectionDefinition, IProjection
    where TState : new()
    where TId : IBlobId
    where TPartitionDefiner : IProjectionPartitionDefinition, new()
{
    /// <summary>
    /// Signals the engine to skip this event without raising an error.
    /// </summary>
    public static IBlobId Skip => new ContinueId();

    private readonly Dictionary<Type, List<Func<IEvent, IBlobId>>> subscriptionResolvers;

    private readonly TPartitionDefiner partitionDefiner;

    public ProjectionDefinition()
    {
        ((IProjectionDefinition)this).State = new TState();
        subscriptionResolvers = new Dictionary<Type, List<Func<IEvent, IBlobId>>>();

        partitionDefiner = new TPartitionDefiner();
    }

    public TId ProjectionId { get { return (TId)((IHaveState)this).Id; } private set { ((IHaveState)this).Id = value; } }

    public TState State { get { return (TState)((IHaveState)this).State; } private set { ((IHaveState)this).State = value; } }

    IBlobId IHaveState.Id { get; set; }

    object IHaveState.State { get; set; }

    long IAmPartionableProjection.GetPartition(IEvent @event)
    {
        return partitionDefiner.CalculatePartition(@event);
    }

    IEnumerable<IBlobId> IProjectionDefinition.GetProjectionIds(IEvent @event)
    {
        foreach (var subscriptionResolver in subscriptionResolvers[@event.GetType()])
        {
            IBlobId resolvedId = subscriptionResolver(@event);
            if (resolvedId is null || resolvedId is ContinueId)
                continue;

            yield return resolvedId;
        }
    }

    async Task IProjectionDefinition.ApplyAsync(IEvent @event)
    {
        await ((dynamic)this).HandleAsync((dynamic)@event).ConfigureAwait(false);
    }

    void IHaveState.InitializeState(IBlobId projectionId, object state)
    {
        ((IHaveState)this).Id = projectionId;
        ((IHaveState)this).State = state ?? new TState();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="projectionId"></param>
    /// <param name="fallback">The fallback function in case the projectionId function fails.</param>
    /// <returns></returns>
    protected ProjectionDefinition<TState, TId, TPartitionDefiner> Subscribe<TEvent>(Func<TEvent, IBlobId> projectionId, Func<TEvent, IBlobId> fallback)
        where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);

        if (subscriptionResolvers.ContainsKey(eventType))
            subscriptionResolvers[eventType].Add(Safe(projectionId, fallback));
        else
            subscriptionResolvers.Add(typeof(TEvent), new List<Func<IEvent, IBlobId>>() { Safe(projectionId, fallback) });

        return this;

        static Func<IEvent, IBlobId> Safe(Func<TEvent, IBlobId> projectionId, Func<TEvent, IBlobId> fallback)
        {
            return x =>
            {
                try { return projectionId((TEvent)x); }
                catch (Exception)
                {
                    if (fallback is null == false)
                        return fallback((TEvent)x);

                    // TODO: Add ERROR log ex
                    throw;
                }
            };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="projectionId"></param>
    /// <param name="fallback">The fallback function in case the projectionId function fails.</param>
    /// <returns></returns>
    protected ProjectionDefinition<TState, TId, TPartitionDefiner> Subscribe<TEvent>(Func<TEvent, IBlobId> projectionId)
        where TEvent : IEvent
    {
        return Subscribe(projectionId, null);
    }

    // Used by replay projection atm
    protected ProjectionDefinition<TState, TId, TPartitionDefiner> Subscribe<TEvent>(Func<TEvent, TId> projectionId, Func<TEvent, TId> fallback)
        where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);

        if (subscriptionResolvers.ContainsKey(eventType))
            subscriptionResolvers[eventType].Add(Safe(projectionId, fallback));
        else
            subscriptionResolvers.Add(typeof(TEvent), new List<Func<IEvent, IBlobId>>() { Safe(projectionId, fallback) });

        return this;

        static Func<IEvent, IBlobId> Safe(Func<TEvent, TId> projectionId, Func<TEvent, TId> fallback)
        {
            return x =>
            {
                try { return projectionId((TEvent)x); }
                catch (Exception)
                {
                    if (fallback is null == false)
                        return fallback((TEvent)x);

                    // TODO: Add Warn ERROR ex
                    throw;
                }
            };
        }
    }

    protected ProjectionDefinition<TState, TId, TPartitionDefiner> Subscribe<TEvent>(Func<TEvent, TId> projectionId)
        where TEvent : IEvent
    {
        return Subscribe(projectionId, null);
    }

    public virtual Task OnReplayCompletedAsync()
    {
        return Task.CompletedTask;
    }
}
