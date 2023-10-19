using Elders.Cronus.EntityTests.TestModel;
using Elders.Cronus.Testing;
using Machine.Specifications;
using System.Linq;
using System;

namespace Elders.Cronus.Tests.InMemoryEventStoreSuite
{
    [Subject("Entity")]
    public class When_loading_public_events_in_aggregate_root_with_entity_from_event_store
    {
        Establish context = () =>
        {

            id = new TestAggregateId();
            var entityId = new TestEntityId(id);

            aggregateRoot = Aggregate<TestAggregateRoot>
                .FromHistory(stream => stream
                    .AddEvent(new TestCreateEvent(id, DateTimeOffset.Now))
                    .AddEvent(new TestCreateEntityEvent(entityId, DateTimeOffset.Now)));

            entity = aggregateRoot.State.Entities.First();
            entity.MakeEntityPublicEvent();
        };

        Because of = () => exception = Catch.Exception(() => aggregateRepository.SaveAsync(aggregateRoot).GetAwaiter().GetResult());

        It should_throw_an_exception = () => exception.Message.Contains("Brad miii");

        static Exception exception;
        static TestAggregateId id;
        static TestEntity entity;
        static IAggregateRepository aggregateRepository;
        static TestAggregateRoot aggregateRoot;
    }
}
