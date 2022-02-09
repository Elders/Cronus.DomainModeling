namespace Elders.Cronus.EntityTests.TestModel
{
    public class TestCreateEvent : IEvent
    {
        public TestCreateEvent(TestAggregateId id)
        {
            Id = id;
        }

        public TestAggregateId Id { get; set; }
    }

    public class TestCreateEntityEvent : IEvent
    {
        public TestCreateEntityEvent(TestEntityId entityId)
        {
            EntityId = entityId;
        }

        public TestEntityId EntityId { get; set; }

    }
}
