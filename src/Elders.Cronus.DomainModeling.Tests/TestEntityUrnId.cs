namespace Elders.Cronus
{
    public class TestEntityUrnId : EntityId<AggregateRootId>
    {
        public TestEntityUrnId(string idBase, AggregateRootId rootId, string entityName) : base(idBase, rootId, entityName)
        {
            Entity_name_but_not_really_because_the_tests_are_failing = entityName;
        }

        public override string Entity_name_but_not_really_because_the_tests_are_failing { get; }
    }
}
