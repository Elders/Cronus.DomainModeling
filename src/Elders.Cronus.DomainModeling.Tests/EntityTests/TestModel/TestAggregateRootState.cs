using System.Collections.Generic;

namespace Elders.Cronus.EntityTests.TestModel
{
    public class TestAggregateRootState : AggregateRootState<TestAggregateRoot, TestAggregateId>
    {
        public TestAggregateRootState()
        {
            Entities = new List<TestEntity>();
        }

        public List<TestEntity> Entities { get; set; }

        public string UpdatableField { get; private set; }

        public override TestAggregateId Id { get; set; }

        public void When(TestCreateEvent e)
        {
            Id = e.Id;
        }

        public void When(TestUpdateEvent e)
        {
            UpdatableField = e.UpdatedFieldValue;
        }

        public void When(TestCreateEntityEvent e)
        {
            var entity = new TestEntity(Root, e.EntityId);
            Entities.Add(entity);
        }

        public void When(TestCreatePublicEvent e)
        {
            Id = e.Id;
        }

        public void When(TestCreateEntityPublicEvent e)
        {
            var entity = new TestEntity(Root, e.EntityId);
            Entities.Add(entity);
        }
    }
}
