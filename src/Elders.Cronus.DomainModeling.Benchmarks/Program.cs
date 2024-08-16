using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;

var cfg = DefaultConfig.Instance.WithSummaryStyle(SummaryStyle.Default.WithTimeUnit(TimeUnit.Millisecond));
BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, cfg);

//using Elders.Cronus;
//using Elders.Cronus.DomainModeling.Benchmarks;

//for (int i = 0; i < 100_000; i++)
//{
//    EntityIdBenchmarks.TestEntityId id = EntityId.Parse<EntityIdBenchmarks.TestEntityId>("urn:tenant:test:id/testentity:entityid");
//    EntityId entityId = EntityId.Parse("urn:tenant:test:id/testentity:entityid");
//}

//Console.WriteLine("done");
//Console.ReadKey();

//using Elders.Cronus;
//using static Elders.Cronus.DomainModeling.Benchmarks.AggregateRootIdBenchmarks;

//var testId = TestId.Parse("urn:tenant:test:id");
//testId = TestId.Parse("urn:tenant:test:id".AsSpan());
//var arId = AggregateRootId.Parse("urn:tenant:asdf:id");
//var gg = new AggregateRootId(testId, "test");
//var asd = new AggregateRootId("tenant", testId);
//Console.WriteLine(arId.Value);

//using Elders.Cronus;

//var urn = new Urn("urn:tenant:test:id");
//var arid = new AggregateRootId(urn);

//Console.WriteLine(arid.Value);

//using static Elders.Cronus.DomainModeling.Benchmarks.EntityIdBenchmarks;

//var gg = TestId.Parse("urn:tenant:test:id".AsSpan());
//var arId = new TestId("tenant", "id");
//var entityId = TestEntityId.Parse<TestEntityId>("urn:tenant:test:id/testentity:entityid");

//Console.WriteLine(entityId);
