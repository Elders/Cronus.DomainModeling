namespace Elders.Cronus.EntityTests.TestModel
{
    public class TestUpdateEvent : IEvent
    {
        public TestUpdateEvent(TestAggregateId id, string updatedFieldValue, DateTimeOffset timestamp)
        {
            Id = id;
            UpdatedFieldValue = updatedFieldValue;
            Timestamp = timestamp;
        }

        public TestAggregateId Id { get; set; }

        public string UpdatedFieldValue { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}
