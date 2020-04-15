using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("ValueObject")]
    public class When_VOs_are_compared
    {
        Establish context = () =>
        {
            left = new VO("val");
            right = new VO("val");
        };

        Because of = () => result = left == right;

        It should_be_equal = () => result.ShouldBeTrue();

        static VO left;
        static VO right;
        static bool result = false;
    }
}
