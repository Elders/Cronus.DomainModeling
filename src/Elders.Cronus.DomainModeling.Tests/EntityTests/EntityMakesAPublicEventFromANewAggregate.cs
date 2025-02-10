using Elders.Cronus.EntityTests.TestModel;
using Elders.Cronus.Testing;

namespace Elders.Cronus.EntityTests;

public class EntityMakesAPublicEventFromANewAggregate
{
    static TestAggregateId id;
    static TestEntity entity;
    static TestAggregateRoot aggregateRoot;

    [Before(Class)]
    public static void Setup()
    {
        id = new TestAggregateId();
        aggregateRoot = new TestAggregateRoot(id);

        var entityId = new TestEntityId(id);
        entity = aggregateRoot.CreateEntity(entityId);

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
        await Assert.That(aggregateRoot.HasNewEvents()).IsTrue();
    }

    [Test]
    public async Task ShouldHaveNewPublicEvents()
    {
        await Assert.That(aggregateRoot.HasNewPublicEvents()).IsTrue();
    }
}
