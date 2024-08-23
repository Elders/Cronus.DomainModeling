using BenchmarkDotNet.Attributes;

namespace Elders.Cronus.DomainModeling.Benchmarks;

[MemoryDiagnoser]
public class EntityIdBenchmarks
{
    public class TestId : AggregateRootId<TestId>
    {
        public const string TestRootName = "test";

        TestId() { }
        public TestId(string tenant, string id) : base(tenant, id) { }
        public TestId(ReadOnlySpan<char> tenant, ReadOnlySpan<char> id) : base(tenant, id) { }

        public override string AggregateRootName => TestRootName;

        protected override TestId Construct(string id, string tenant) => new(tenant, id);
        protected override TestId Construct(ReadOnlySpan<char> id, ReadOnlySpan<char> tenant) => new(tenant, id);
    }

    public class TestEntityId : EntityId<TestId>
    {
        public const string TestEntityName = "testentity";

        public override string Entity_name_but_not_really_because_the_tests_are_failing => TestEntityName;

        TestEntityId() { }

        public TestEntityId(string idBase, TestId rootId) : base(idBase, rootId, TestEntityName) { }
        public TestEntityId(ReadOnlySpan<char> idBase, TestId rootId) : base(idBase, rootId) { }
    }

    private static readonly TestId testId = new("tenant", "id");
    private static readonly TestId equalTestId = new("tenant", "id");

    private static readonly TestEntityId entityId = new("id", testId);
    private static readonly TestEntityId equalEntityId = new("id", equalTestId);

    private TestEntityId[] ids;
    private EntityId[] baseEntityIds;

    [Params(1, 1000, 10_000, 100_000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        ids = new TestEntityId[N];
        baseEntityIds = new EntityId[N];
    }

    [Benchmark(Baseline = true)]
    public TestEntityId[] Create_N_Ids_From_Strings()
    {
        for (int i = 0; i < N; i++)
        {
            ids[i] = new TestEntityId("id", testId);
        }

        return ids;
    }

    [Benchmark]
    public TestEntityId[] Create_N_Ids_From_Spans()
    {
        for (int i = 0; i < N; i++)
        {
            ids[i] = new TestEntityId("id".AsSpan(), testId);
        }

        return ids;
    }

    [Benchmark]
    public EntityId[] Create_N_Base_Ids_From_String_Using_Static_Parse()
    {
        for (int i = 0; i < N; i++)
        {
            baseEntityIds[i] = EntityId.Parse("urn:tenant:test:id/testentity:entityid");
        }

        return baseEntityIds;
    }

    [Benchmark]
    public EntityId[] Create_N_Base_Ids_From_Span_Using_Static_Parse()
    {
        for (int i = 0; i < N; i++)
        {
            baseEntityIds[i] = EntityId.Parse("urn:tenant:test:id/testentity:entityid".AsSpan());
        }

        return baseEntityIds;
    }

    [Benchmark]
    public TestEntityId[] Create_N_Specific_Ids_From_Span_Using_Static_Parse()
    {
        for (int i = 0; i < N; i++)
        {
            ids[i] = EntityId.Parse<TestEntityId>("urn:tenant:test:id/testentity:entityid");
        }

        return ids;
    }

    [Benchmark]
    public bool Compare_Equal_Ids()
    {
        return entityId.Equals(equalEntityId);
    }
}
