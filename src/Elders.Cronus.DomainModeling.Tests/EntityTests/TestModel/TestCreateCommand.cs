namespace Elders.Cronus.EntityTests.TestModel
{
    public class TestCreateCommand : ICommand
    {
        TestCreateCommand() { }

        public TestCreateCommand(TestAggregateId id)
        {

        }

        public TestAggregateId Id { get; set; }
    }
}
