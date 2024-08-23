using System;

namespace Elders.Cronus.EntityTests.TestModel
{

    public class TestEntityId : EntityId<TestAggregateId>
    {
        public override string Entity_name_but_not_really_because_the_tests_are_failing => "TestEntityId";

        public TestEntityId(Guid id, TestAggregateId rootId)
            : base(id.ToString(), rootId)
        {

        }

        public TestEntityId(TestAggregateId rootId)
            : base(Guid.NewGuid().ToString(), rootId)
        {

        }
    }
}
