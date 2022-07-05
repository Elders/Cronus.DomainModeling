using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Elders.Cronus
{
    [Subject(typeof(EventHandlerRegistrations))]
    public class When__GetEventHandler__for_event
    {
        static EventHandlerRegistrations handlers;
        static DomainObjectEventHandlerMapping mapping;
        static Dictionary<Type, Action<IEvent>> arHandlers;

        static TestEvent testEvent;
        static TestEventParent testParent;
        static TestEventIParent testIParent;
        static TestEventBaseIParent testBaseIParent;
        static IEvent realEvent;
        static Action<IEvent> result;

        Establish ctx = () =>
        {
            mapping = new DomainObjectEventHandlerMapping();
            arHandlers = mapping.GetEventHandlers(() => new TestAggregateState());
            handlers = new EventHandlerRegistrations();

            foreach (var handler in arHandlers)
            {
                handlers.Register(handler.Key, handler.Value);
            }

            testEvent = new TestEvent("test");
            testParent = new TestEventParent("test");
            testIParent = new TestEventIParent("test");
            testBaseIParent = new TestEventBaseIParent("test");
        };

        class When_event_have_a_specific_handler
        {
            Because of = () => result = handlers.GetEventHandler(testEvent, out realEvent);

            It shoul_have_proper_action = () =>
            {
                Action<IEvent> eventAction = arHandlers[testEvent.GetType()];

                result.Method.ShouldEqual(eventAction.Method);
            };
        }

        class When_event_have_a__handler_by_parenttype
        {
            Because of = () => result = handlers.GetEventHandler(testParent, out realEvent);

            It shoul_have_proper_action = () =>
            {
                Action<IEvent> eventAction = arHandlers[testParent.GetType().BaseType];

                result.Method.ShouldEqual(eventAction.Method);
            };
        }

        class When_event_have_a__handler_by_parenttype_2
        {
            Because of = () => result = handlers.GetEventHandler(testBaseIParent, out realEvent);

            It shoul_have_proper_action = () =>
            {
                Action<IEvent> eventAction = arHandlers[testBaseIParent.GetType().BaseType];

                result.Method.ShouldEqual(eventAction.Method);
            };
        }

        class When_event_have_a__handler_by_interface
        {
            Because of = () => result = handlers.GetEventHandler(testIParent, out realEvent);

            It shoul_have_proper_action = () =>
            {
                Action<IEvent> eventAction = arHandlers[testIParent.GetType().GetInterfaces().First()];

                result.Method.ShouldEqual(eventAction.Method);
            };
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

    public class Parent : IEvent { }

    [DataContract(Name = "3a1ff2728-0dc3-4eb4-aa37-62a9163a1fb6")]
    public class TestEventParent : Parent
    {
        private TestEventParent() { }

        public TestEventParent(string name)
        {
            Name = name;
        }

        [DataMember(Order = 1)]
        public string Name { get; private set; }
    }

    public interface IParent : IEvent { }

    [DataContract(Name = "47dac65f-02a3-470a-8dba-8ced047fe469")]
    public class TestEventIParent : IParent
    {
        private TestEventIParent() { }

        public TestEventIParent(string name)
        {
            Name = name;
        }

        [DataMember(Order = 1)]
        public string Name { get; private set; }
    }

    public class ParentBase : IParent { }

    [DataContract(Name = "47dac65f-02a3-470a-8dba-8ced047fe469")]
    public class TestEventBaseIParent : ParentBase
    {
        private TestEventBaseIParent() { }

        public TestEventBaseIParent(string name)
        {
            Name = name;
        }

        [DataMember(Order = 1)]
        public string Name { get; private set; }
    }


    public class TestAggregate : AggregateRoot<TestAggregateState>
    {
        public TestAggregate() { }
    }

    public class TestAggregateState : AggregateRootState<TestAggregate, TestAggregateRootId>
    {
        public TestAggregateState() { }

        public override TestAggregateRootId Id { get; set; }

        public void When(TestEvent e) { }

        public void When(Parent e) { }

        public void When(IParent e) { }

        public void When(ParentBase e) { }
    }

    [DataContract(Name = "19b290e1-9403-4414-9558-67a270f5bcc5")]
    public class TestAggregateRootId : AggregateRootId
    {
        public TestAggregateRootId() { }

        public TestAggregateRootId(string id, string tenant) : base(id, "test", tenant) { }
    }
}
