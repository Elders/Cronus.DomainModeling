using System;
using Machine.Specifications;

namespace Elders.Cronus.DomainModeling.Tests
{
    [Subject("GuidTenantId")]
    public class When_GuidTenantId_is_created
    {
        Establish context = () =>
        {
            id = "71de205c-80d1-4b59-baa4-87f9247350b1";
            tenant = "tenant";
            aggregateName = "arName";
            valuePart = aggregateName + ":" + id;
            urn = new TenantUrn(tenant, valuePart);
        };

        Because of = () => result = new GuidTenantId(Guid.Parse(id), aggregateName, tenant);

        It should_have_tenant_as_base_part = () => result.Urn.BasePart.ShouldEqual(urn.BasePart);

        It should_have_value_as_value_part = () => result.Urn.ValuePart.ShouldEqual(urn.ValuePart);

        It should_have_value = () => result.Urn.Value.ShouldEqual(urn.Value);

        static IUrn urn;
        static IAggregateRootId result;
        static string tenant;
        static string valuePart;
        static string aggregateName;
        static string id;
    }
}