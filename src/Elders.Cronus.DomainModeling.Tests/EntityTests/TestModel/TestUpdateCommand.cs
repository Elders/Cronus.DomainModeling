namespace Elders.Cronus.EntityTests.TestModel
{
    public class TestUpdateCommand : ICommand
    {
        TestUpdateCommand() { }

        public TestUpdateCommand(TestAggregateId id)
        {

        }

        public TestAggregateId Id { get; set; }
    }
}
