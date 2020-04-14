namespace Elders.Cronus
{
    public class TestEntityId : EntityId<AggregateRootId>
    {
        public TestEntityId(string idBase, AggregateRootId rootId, string entityName) : base(idBase, rootId, entityName)
        {
        }
    }
}
