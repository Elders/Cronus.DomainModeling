using System;

namespace Elders.Cronus.EntityTests.TestModel
{

    public class TestEntityId : EntityId<TestAggregateId>
    {
        public TestEntityId(Guid id, TestAggregateId rootId)
            : base(id.ToString(), rootId, "TestEntityId")
        {

        }

        public TestEntityId(TestAggregateId rootId)
            : base(Guid.NewGuid().ToString(), rootId, "TestEntityId")
        {

        }
    }
}
