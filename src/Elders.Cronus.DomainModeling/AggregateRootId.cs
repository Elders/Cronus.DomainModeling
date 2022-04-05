using System;

namespace Elders.Cronus;

public class AggregateRootId : AggregateUrn, IAggregateRootId
{
    /// <summary>
    /// Prevents a default instance of the <see cref="AggregateRootId"/> class from being created.
    /// </summary>
    /// <remarks>Used only for serizalization.</remarks>
    protected AggregateRootId() { }

    public AggregateRootId(string idBase, string aggregateRootName, string tenant)
        : base(tenant, aggregateRootName, idBase)
    {
    }

    public AggregateRootId(string aggregateRootName, AggregateUrn urn)
        : base(urn.Tenant, urn)
    {
        if (aggregateRootName.Equals(urn.AggregateRootName, StringComparison.OrdinalIgnoreCase) == false)
            throw new ArgumentException("AggregateRootName missmatch");
    }
}

public abstract class AggregateRootId<T> : AggregateUrn, IAggregateRootId
    where T : AggregateRootId<T>
{
    static UberUrnFormatProvider urnFormatProvider = new UberUrnFormatProvider();

    protected AggregateRootId() { }

    protected AggregateRootId(string id, string rootName, string tenant) : base(tenant, rootName, id) { }

    public static T New(string tenant)
    {
        var instance = (T)System.Activator.CreateInstance(typeof(T), true);
        return instance.Construct(System.Guid.NewGuid().ToString(), tenant);
    }

    public static T New(string tenant, string id)
    {
        var instance = (T)System.Activator.CreateInstance(typeof(T), true);
        return instance.Construct(id, tenant);
    }

    protected abstract T Construct(string id, string tenant);

    new public static T Parse(string id)
    {
        var instance = (T)System.Activator.CreateInstance(typeof(T), true);

        var stringTenantUrn = AggregateUrn.Parse(id, urnFormatProvider);
        var newId = instance.Construct(stringTenantUrn.Id, stringTenantUrn.Tenant);
        if (stringTenantUrn.AggregateRootName == newId.AggregateRootName)
            return newId;
        else
            throw new System.Exception("bum");
        //todo check if ar name mateches..
    }

    public static bool TryParse(string id, out T result)
    {
        try
        {
            var instance = (T)System.Activator.CreateInstance(typeof(T), true);

            var stringTenantUrn = AggregateUrn.Parse(id, urnFormatProvider);
            var newId = instance.Construct(stringTenantUrn.Id, stringTenantUrn.Tenant);
            if (stringTenantUrn.AggregateRootName == newId.AggregateRootName)
                result = newId;
            else
                throw new System.Exception("bum");
            //todo check if ar name mateches..

            return true;
        }
        catch (Exception)
        {
            result = null;
            return false;
        }
    }
}
