namespace Elders.Cronus
{
    public class TestEntityUrnId : EntityId<AggregateRootId>
    {
        public TestEntityUrnId(string idBase, AggregateRootId rootId, string entityName) : base(idBase, rootId, entityName)
        {
        }
    }
}
