using Elders.Cronus.EntityTests.TestModel;
using Elders.Cronus.Testing;

namespace Elders.Cronus.EntityTests;

public class EntityMakesAPublicEventFromAnExistingLoadedAggregate
{
    static TestAggregateId id;
    static TestEntity entity;
    static TestAggregateRoot aggregateRoot;

    [Before(Class)]
    public static void Setup()
    {
        id = new TestAggregateId();
        var entityId = new TestEntityId(id);

        aggregateRoot = Aggregate<TestAggregateRoot>
            .FromHistory(stream => stream
                .AddEvent(new TestCreateEvent(id, DateTimeOffset.Now))
                .AddEvent(new TestCreateEntityEvent(entityId, DateTimeOffset.Now)));

        entity = aggregateRoot.State.Entities.First();
        entity.MakeEntityPublicEvent();
    }

    [Test]
    public async Task ShouldInstansiateAggregateRoot()
    {
        await Assert.That(aggregateRoot).IsNotNull();
    }

    [Test]
    public async Task ShouldInstansiateAggregateRootWithValidState()
    {
        await Assert.That(aggregateRoot.State.Id).IsEqualTo(id);
    }

    [Test]
    public async Task ShouldInstansiateAggregateRootWithLatestState()
    {
        await Assert.That(aggregateRoot.State.Entities.Count).IsEqualTo(1);
    }

    [Test]
    public async Task ShouldInstansiateAggregateRootWithLatestStateReversion()
    {
        await Assert.That((aggregateRoot as IAggregateRoot).Revision).IsEqualTo(1);
    }

    [Test]
    public async Task ShouldHaveNewEvents()
    {
        await Assert.That(aggregateRoot.HasNewEvents()).IsFalse();
    }

    [Test]
    public async Task ShouldHaveNewPublicEvents()
    {
        await Assert.That(aggregateRoot.HasNewPublicEvents()).IsTrue();
    }
}
