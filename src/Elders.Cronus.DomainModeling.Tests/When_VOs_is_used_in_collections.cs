using System.Collections.Generic;
using System.Linq;
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

    [Subject("ValueObject")]
    public class When_VOs_is_used_in_collections
    {
        Establish context = () =>
        {
            left = new VO("val");
            collection = new HashSet<VO>();
            collection.Add(left);
            right = new VO("val");
        };

        Because of = () => result = collection.Where(x => x == right).Any();

        It should_return_an_element = () => result.ShouldBeTrue();

        static VO left;
        static VO right;
        static HashSet<VO> collection;
        static bool result = false;
    }

    internal class VO : ValueObject<VO>
    {
        VO() { }

        public VO(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}
