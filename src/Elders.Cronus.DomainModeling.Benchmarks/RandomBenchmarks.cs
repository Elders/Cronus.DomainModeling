namespace Elders.Cronus.DomainModeling.Benchmarks;

//[MemoryDiagnoser]
public class RandomBenchmarks
{
    private static string originalString = "1234qwer1234qwerasdfzcv1234qwer";
    private static char[] charArray = originalString.ToCharArray();

    //[Benchmark(Baseline = true)]
    public bool Span()
    {
        return charArray.AsSpan().StartsWith("123");
    }

    //[Benchmark]
    public bool String()
    {
        return originalString.StartsWith("123");
    }
}
