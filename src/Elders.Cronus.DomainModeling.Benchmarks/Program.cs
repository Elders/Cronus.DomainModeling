#if RELEASE

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;

var cfg = DefaultConfig.Instance
    .WithSummaryStyle(SummaryStyle.Default.WithTimeUnit(TimeUnit.Millisecond))
    .StopOnFirstError();

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, cfg);

#elif DEBUG


Console.WriteLine("gg");

#endif
