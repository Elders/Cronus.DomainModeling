namespace Elders.Cronus.EntityTests.TestModel
{
    public class TestUpdateEvent : IEvent
    {
        public TestUpdateEvent(TestAggregateId id, string updatedFieldValue)
        {
            Id = id;
            UpdatedFieldValue = updatedFieldValue;
        }

        public TestAggregateId Id { get; set; }

        public string UpdatedFieldValue { get; set; }
    }
}
