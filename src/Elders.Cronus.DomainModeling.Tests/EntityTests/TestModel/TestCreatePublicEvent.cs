namespace Elders.Cronus.EntityTests.TestModel
{
    public class TestCreatePublicEvent : IPublicEvent
    {
        public TestCreatePublicEvent(TestAggregateId id, DateTimeOffset timestamp)
        {
            Id = id;
            Tenant = Id.Tenant;
            Timestamp = timestamp;
        }

        public TestAggregateId Id { get; set; }

        public string Tenant { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }

    public class TestCreateEntityPublicEvent : IPublicEvent
    {
        public TestCreateEntityPublicEvent(TestEntityId entityId, DateTimeOffset timestamp)
        {
            EntityId = entityId;
            Tenant = entityId.NSS;
            Timestamp = timestamp;
        }

        public TestEntityId EntityId { get; set; }

        public string Tenant { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}
