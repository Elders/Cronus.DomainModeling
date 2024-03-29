﻿using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_comparing_two_entity_ids
    {
        Establish context = () =>
        {
            urn = new Urn("urn:tenant:arName:123a/Entity:456E".ToLower());
            origin = new AggregateRootId("Tenant", "arName", "123a");
        };

        Because of = () => result = new TestEntityUrnId("456E", origin, "Entity");

        It should_be_equal = () => (result == urn).ShouldBeTrue();

        static Urn urn;
        static AggregateRootId origin;
        static EntityId<AggregateRootId> result;
    }
}
