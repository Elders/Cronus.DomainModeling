using Elders.Cronus.Testing;
using Machine.Specifications;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Elders.Cronus
{
    [Subject(typeof(EventHandlerRegistrations))]
    public class When__GetEventHandler__for_event
    {
        static TestEventFromBaseClass TestEventFromBase;
        static TestEventFromInterface testEventFromInterface;
        static TestEventInRecursion testEventInRecursion;
        static TestAggregate aggregate;

        Establish ctx = () =>
        {
            aggregate = new TestAggregate();

            TestEventFromBase = new TestEventFromBaseClass("test");
            testEventFromInterface = new TestEventFromInterface("test");
            testEventInRecursion = new TestEventInRecursion("test");
        };

        class When_real_event_type_inherited_from_parrent_base
        {
            Because of = () => aggregate.PersistEvent(TestEventFromBase);

            It shoul_be_added_to_uncommited_events = () => aggregate.RootState().persistedEvents.Contains(TestEventFromBase).ShouldBeTrue();
        }

        class When_real_event_type_inherited_from_contract
        {
            Because of = () => aggregate.PersistEvent(testEventFromInterface);

            It shoul_be_added_to_uncommited_events = () => aggregate.RootState().persistedEvents.Contains(testEventFromInterface).ShouldBeTrue();
        }

        class When_real_event_type_inherited_from_nested_parrents
        {
            Because of = () => aggregate.PersistEvent(testEventInRecursion);

            It shoul_be_added_to_uncommited_events = () => aggregate.RootState().persistedEvents.Contains(testEventInRecursion).ShouldBeTrue();
        }
    }

    [DataContract(Name = "363667d5-864f-4198-825a-f32990072044")]
    public class TestEvent : IEvent
    {
        private TestEvent() { }

        public TestEvent(string name)
        {
            Name = name;
        }

        [DataMember(Order = 1)]
        public string Name { get; private set; }
    }

    public abstract class ParentBase : IParent { }

    [DataContract(Name = "3a1ff2728-0dc3-4eb4-aa37-62a9163a1fb6")]
    public class TestEventFromBaseClass : ParentBase
    {
        private TestEventFromBaseClass() { }

        public TestEventFromBaseClass(string name)
        {
            Name = name;
        }

        [DataMember(Order = 1)]
        public string Name { get; private set; }
    }

    public interface IParent : IEvent { }

    [DataContract(Name = "47dac65f-02a3-470a-8dba-8ced047fe469")]
    public class TestEventFromInterface : IParent
    {
        private TestEventFromInterface() { }

        public TestEventFromInterface(string name)
        {
            Name = name;
        }

        [DataMember(Order = 1)]
        public string Name { get; private set; }
    }
    public class ParentBase2 : ParentBase { }
    public class ParentBase3 : ParentBase2 { }
    public class ParentBase4 : ParentBase3 { }

    [DataContract(Name = "47dac65f-02a3-470a-8dba-8ced047fe469")]
    public class TestEventInRecursion : ParentBase4
    {
        private TestEventInRecursion() { }

        public TestEventInRecursion(string name)
        {
            Name = name;
        }

        [DataMember(Order = 1)]
        public string Name { get; private set; }
    }

    public class TestAggregate : AggregateRoot<TestAggregateState>
    {
        public TestAggregate() { }

        public void PersistEvent(IEvent @event)
        {
            Apply(@event);
        }
    }

    public class TestAggregateState : AggregateRootState<TestAggregate, TestAggregateRootId>
    {
        public TestAggregateState() { persistedEvents = new List<IEvent>(); }
        public override TestAggregateRootId Id { get; set; }

        public List<IEvent> persistedEvents;

        public void When(IParent e) { persistedEvents.Add(e); }

        public void When(ParentBase e) { persistedEvents.Add(e); }
    }

    [DataContract(Name = "19b290e1-9403-4414-9558-67a270f5bcc5")]
    public class TestAggregateRootId : AggregateRootId
    {
        public TestAggregateRootId() { }

        public TestAggregateRootId(string id, string tenant) : base(tenant, "test", id) { }
    }
}
