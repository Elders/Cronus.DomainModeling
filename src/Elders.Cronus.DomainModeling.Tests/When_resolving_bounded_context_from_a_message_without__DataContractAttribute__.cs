using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("MessageInfo")]
    public class When_resolving_bounded_context_from_a_message_without__DataContractAttribute__
    {
        Because of = () => result = typeof(IPublicEvent).GetBoundedContext("elders");

        It should_resolve_the_default_bounded_context = () => result.ShouldEqual("elders");

        static string result;

    }
}
