using BenchmarkDotNet.Attributes;

namespace Elders.Cronus.DomainModeling.Benchmarks;

[MemoryDiagnoser]
public class AggregateRootIdBenchmarks
{
    public class TestId : AggregateRootId<TestId>
    {
        TestId() { }
        public TestId(string tenant, string id) : base(tenant, id) { }
        public TestId(ReadOnlySpan<char> tenant, ReadOnlySpan<char> id) : base(tenant, id) { }

        public override string AggregateRootName => "test";

        protected override TestId Construct(string id, string tenant) => new(tenant, id);
        protected override TestId Construct(ReadOnlySpan<char> id, ReadOnlySpan<char> tenant) => new(tenant, id);
    }

    private static readonly TestId testId = new("tenant", "id");
    private static readonly TestId equalTestId = new("tenant", "id");

    private TestId[] ids;

    [Params(1, 1000, 10_000, 100_000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        ids = new TestId[N];
    }

    [Benchmark(Baseline = true)]
    public TestId[] Create_N_Ids_From_Strings()
    {
        for (int i = 0; i < N; i++)
        {
            ids[i] = new TestId("tenant", "id");
        }

        return ids;
    }

    [Benchmark]
    public TestId[] Create_N_Ids_From_Spans()
    {
        for (int i = 0; i < N; i++)
        {
            ids[i] = new TestId("tenant".AsSpan(), "id");
        }

        return ids;
    }

    [Benchmark]
    public TestId[] Create_N_Ids_From_String_Using_Static_New()
    {
        for (int i = 0; i < N; i++)
        {
            ids[i] = TestId.New("tenant", "id");
        }

        return ids;
    }

    [Benchmark]
    public TestId[] Create_N_Ids_From_Span_Using_Static_New()
    {
        for (int i = 0; i < N; i++)
        {
            ids[i] = TestId.New("tenant".AsSpan(), "id");
        }

        return ids;
    }

    [Benchmark]
    public TestId[] Create_N_Ids_From_String_Using_Static_Parse()
    {
        for (int i = 0; i < N; i++)
        {
            ids[i] = TestId.Parse("urn:tenant:test:id");
        }

        return ids;
    }

    [Benchmark]
    public TestId[] Create_N_Ids_From_Span_Using_Static_Parse()
    {
        for (int i = 0; i < N; i++)
        {
            ids[i] = TestId.Parse("urn:tenant:test:id".AsSpan());
        }

        return ids;
    }

    [Benchmark]
    public bool Compare_Equal_Ids()
    {
        return testId.Equals(equalTestId);
    }
}
