using System;

namespace Elders.Cronus.EntityTests.TestModel
{
    public class TestUpdateCommand : ICommand
    {
        TestUpdateCommand() { }

        public TestUpdateCommand(TestAggregateId id, DateTimeOffset timestamp)
        {
            Id = id;
            Timestamp = timestamp;
        }

        public TestAggregateId Id { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}
