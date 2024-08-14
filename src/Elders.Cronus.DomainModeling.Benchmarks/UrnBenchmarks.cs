using BenchmarkDotNet.Attributes;

namespace Elders.Cronus.DomainModeling.Benchmarks;

[MemoryDiagnoser]
public class UrnBenchmarks
{
    private const string urnString = "urn:nid:nss?+r_comp?=q_comp#f_comp";
    private static readonly Urn urn = new Urn("nid", "nss", "r_comp", "q_comp", "f_comp");
    private static readonly Urn equalUrn = new Urn("nid", "nss", "r_comp", "q_comp", "f_comp");

    private const string urnStringWithoutComponents = "urn:nid:nss";
    private static readonly Urn urnWithoutComponents = new Urn("nid", "nss");
    private static readonly Urn equalUrnWithoutComponents = new Urn("nid", "nss");

    private Urn[] urns;

    [Params(1, 1000, 10_000, 100_000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        urns = new Urn[N];
    }

    [Benchmark(Baseline = true)]
    public Urn[] Create_N_Urns_From_String()
    {
        for (int i = 0; i < N; i++)
        {
            urns[i] = new Urn(urnString);
        }

        return urns;
    }

    [Benchmark]
    public Urn[] Create_N_Urns_From_String_Without_Components()
    {
        for (int i = 0; i < N; i++)
        {
            urns[i] = new Urn(urnStringWithoutComponents);
        }

        return urns;
    }

    [Benchmark]
    public Urn[] Create_N_Urns_From_String_Parts()
    {
        for (int i = 0; i < N; i++)
        {
            urns[i] = new Urn("nid", "nss", "r_comp", "q_comp", "f_comp");
        }

        return urns;
    }

    [Benchmark]
    public Urn[] Create_N_Urns_From_String_Parts_Without_Components()
    {
        for (int i = 0; i < N; i++)
        {
            urns[i] = new Urn("nid", "nss");
        }

        return urns;
    }

    [Benchmark]
    public Urn[] Create_N_Urns_From_Span()
    {
        for (int i = 0; i < N; i++)
        {
            urns[i] = new Urn(urnString.AsSpan());
        }

        return urns;
    }

    [Benchmark]
    public Urn[] Create_N_Urns_From_Span_Without_Components()
    {
        for (int i = 0; i < N; i++)
        {
            urns[i] = new Urn(urnStringWithoutComponents.AsSpan());
        }

        return urns;
    }

    [Benchmark]
    public Urn[] Create_N_Urns_From_Span_Parts()
    {
        for (int i = 0; i < N; i++)
        {
            urns[i] = new Urn("nid".AsSpan(), "nss", "r_comp", "q_comp", "f_comp");
        }

        return urns;
    }

    [Benchmark]
    public Urn[] Create_N_Urns_From_Span_Parts_Without_Components()
    {
        for (int i = 0; i < N; i++)
        {
            urns[i] = new Urn("nid".AsSpan(), "nss");
        }

        return urns;
    }

    [Benchmark]
    public Urn[] Create_N_Urns_From_Another()
    {
        for (int i = 0; i < N; i++)
        {
            urns[i] = new Urn(urn);
        }

        return urns;
    }

    [Benchmark]
    public Urn[] Create_N_Urns_From_Another_Without_Components()
    {
        for (int i = 0; i < N; i++)
        {
            urns[i] = new Urn(urnWithoutComponents);
        }

        return urns;
    }

    [Benchmark]
    public bool Compare_Equal_Urns()
    {
        return urn.Equals(equalUrn);
    }

    [Benchmark]
    public bool Compare_Equal_Urns_Without_Components()
    {
        return urnWithoutComponents.Equals(equalUrnWithoutComponents);
    }
}
