using Elders.Cronus.EntityTests.TestModel;
using Elders.Cronus.Testing;
using Machine.Specifications;
using System.Linq;

namespace Elders.Cronus.EntityTests
{
    [Subject("Entity")]
    public class When_an_entity_makes_a_public_event_from_a_new_aggregate
    {
        Establish context = () =>
        {
            id = new TestAggregateId();
            aggregateRoot = new TestAggregateRoot(id);

            var entityId = new TestEntityId(id);
            entity = aggregateRoot.CreateEntity(entityId);

        };

        Because of = () => entity.MakeEntityPublicEvent();

        It should_instansiate_aggregate_root = () => aggregateRoot.ShouldNotBeNull();

        It should_instansiate_aggregate_root_with_valid_state = () => aggregateRoot.State.Id.ShouldEqual(id);

        It should_instansiate_aggregate_root_with_latest_state = () => aggregateRoot.State.Entities.Count.ShouldEqual(1);

        It should_instansiate_aggregate_root_with_latest_state_reversion = () => (aggregateRoot as IAggregateRoot).Revision.ShouldEqual(1);

        It should_have_new_events = () => aggregateRoot.HasNewEvents().ShouldBeTrue();

        It should_have_new_public_events = () => aggregateRoot.HasNewPublicEvents().ShouldBeTrue();

        static TestAggregateId id;
        static TestEntity entity;
        static TestAggregateRoot aggregateRoot;
    }

    [Subject("Entity")]
    public class When_an_entity_makes_a_public_event_from_an_existing_loaded_aggregate
    {
        Establish context = () =>
        {
            id = new TestAggregateId();
            var entityId = new TestEntityId(id);

            aggregateRoot = Aggregate<TestAggregateRoot>
                .FromHistory(stream => stream
                    .AddEvent(new TestCreateEvent(id))
                    .AddEvent(new TestCreateEntityEvent(entityId)));

            entity = aggregateRoot.State.Entities.First();

        };

        Because of = () => entity.MakeEntityPublicEvent();

        It should_instansiate_aggregate_root = () => aggregateRoot.ShouldNotBeNull();

        It should_instansiate_aggregate_root_with_valid_state = () => aggregateRoot.State.Id.ShouldEqual(id);

        It should_instansiate_aggregate_root_with_latest_state = () => aggregateRoot.State.Entities.Count.ShouldEqual(1);

        It should_instansiate_aggregate_root_with_latest_state_reversion = () => (aggregateRoot as IAggregateRoot).Revision.ShouldEqual(1);

        It should_have_new_events = () => aggregateRoot.HasNewEvents().ShouldBeFalse();

        It should_have_new_public_events = () => aggregateRoot.HasNewPublicEvents().ShouldBeTrue();

        static TestAggregateId id;
        static TestEntity entity;
        static TestAggregateRoot aggregateRoot;
    }
}
