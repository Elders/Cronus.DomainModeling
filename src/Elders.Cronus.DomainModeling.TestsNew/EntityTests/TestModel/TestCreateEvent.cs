namespace Elders.Cronus.EntityTests.TestModel
{
    public class TestCreateEvent : IEvent
    {
        public TestCreateEvent(TestAggregateId id, DateTimeOffset timestamp)
        {
            Id = id;
            Timestamp = timestamp;
        }

        public TestAggregateId Id { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }

    public class TestCreateEntityEvent : IEvent
    {
        public TestCreateEntityEvent(TestEntityId entityId, DateTimeOffset timestamp)
        {
            EntityId = entityId;
            Timestamp = timestamp;
        }

        public TestEntityId EntityId { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}
