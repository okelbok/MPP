using FluentAssertions;

using Tracer.Serialization.Abstractions;

namespace Tracer.Core.Tests;

public class TracerTests
{
	private readonly ITracer _tracer;

	public TracerTests()
	{
		_tracer = new Tracer();
	}

	[Fact]
	public void SimpleTrace_ShouldRecordMethod()
	{
		_tracer.StartTrace();
		Thread.Sleep(10);
		_tracer.StopTrace();

		TraceResult result = _tracer.GetTraceResult();

		result.Threads.Should().HaveCount(1);

		ThreadTraceResult thread = result.Threads[0];

		thread.Methods.Should().HaveCount(1);
		thread.Methods[0].Name.Should().Be(nameof(SimpleTrace_ShouldRecordMethod));
		thread.Methods[0].ClassName.Should().Be(nameof(TracerTests));
		thread.Methods[0].Time.Should().BeGreaterThan(0);
	}

	[Fact]
	public void NestedTrace_ShouldRecordHierarchy()
	{
		MethodA();

		TraceResult result = _tracer.GetTraceResult();
		ThreadTraceResult thread = result.Threads[0];

		thread.Methods.Should().HaveCount(1);

		MethodTraceResult methodA = thread.Methods[0];
		methodA.Name.Should().Be(nameof(MethodA));

		methodA.Methods.Should().HaveCount(1);

		MethodTraceResult methodB = methodA.Methods[0];
		methodB.Name.Should().Be(nameof(MethodB));
	}

	private void MethodA()
	{
		_tracer.StartTrace();
		MethodB();
		_tracer.StopTrace();
	}

	private void MethodB()
	{
		_tracer.StartTrace();
		Thread.Sleep(5);
		_tracer.StopTrace();
	}

	[Fact]
	public void MultipleRootMethods_ShouldBeRecordedSeparately()
	{
		_tracer.StartTrace();
		Thread.Sleep(5);
		_tracer.StopTrace();

		_tracer.StartTrace();
		Thread.Sleep(5);
		_tracer.StopTrace();

		TraceResult result = _tracer.GetTraceResult();
		var thread = result.Threads.Single();

		thread.Methods.Should().HaveCount(2);
	}

	[Fact]
	public void DeepNestedTrace_ShouldRecordThreeLevels()
	{
		Level1();

		TraceResult result = _tracer.GetTraceResult();
		var thread = result.Threads.Single();

		var m1 = thread.Methods.Single();
		var m2 = m1.Methods.Single();
		var m3 = m2.Methods.Single();

		m1.Name.Should().Be(nameof(Level1));
		m2.Name.Should().Be(nameof(Level2));
		m3.Name.Should().Be(nameof(Level3));
	}

	private void Level1() { _tracer.StartTrace(); Level2(); _tracer.StopTrace(); }
	private void Level2() { _tracer.StartTrace(); Level3(); _tracer.StopTrace(); }
	private void Level3() { _tracer.StartTrace(); Thread.Sleep(5); _tracer.StopTrace(); }

	[Fact]
	public void ReentrantMethod_ShouldCreateTwoSeparateEntries()
	{
		ActualReentrant();

		TraceResult result = _tracer.GetTraceResult();
		var thread = result.Threads.Single();

		thread.Methods.Should().HaveCount(2);
		thread.Methods.All(m => m.Name == nameof(ActualReentrant)).Should().BeTrue();
	}

	private void ActualReentrant()
	{
		_tracer.StartTrace();
		Thread.Sleep(5);
		_tracer.StopTrace();

		_tracer.StartTrace();
		Thread.Sleep(5);
		_tracer.StopTrace();
	}

	[Fact]
	public void ThreadTime_ShouldEqualSumOfRootMethods()
	{
		_tracer.StartTrace();
		Thread.Sleep(10);
		_tracer.StopTrace();

		_tracer.StartTrace();
		Thread.Sleep(20);
		_tracer.StopTrace();

		TraceResult result = _tracer.GetTraceResult();
		var thread = result.Threads.Single();

		long sum = thread.Methods.Sum(m => m.Time);
		thread.Time.Should().Be(sum);
	}

	[Fact]
	public void ParentMethod_TimeShouldBeGreaterThanOrEqualToChild()
	{
		_tracer.StartTrace();
		Thread.Sleep(5);

		_tracer.StartTrace();
		Thread.Sleep(5);
		_tracer.StopTrace();

		_tracer.StopTrace();

		TraceResult result = _tracer.GetTraceResult();
		var thread = result.Threads.Single();

		var parent = thread.Methods.Single();
		var child = parent.Methods.Single();

		parent.Time.Should().BeGreaterThanOrEqualTo(child.Time);
	}

	[Fact]
	public void MultiThread_ShouldRecordSeparateThreads()
	{
		var t1 = new Thread(() =>
		{
			_tracer.StartTrace();
			Thread.Sleep(15);
			_tracer.StopTrace();
		});

		var t2 = new Thread(() =>
		{
			_tracer.StartTrace();
			Thread.Sleep(15);
			_tracer.StopTrace();
		});

		t1.Start();
		t2.Start();
		t1.Join();
		t2.Join();

		TraceResult result = _tracer.GetTraceResult();

		result.Threads.Should().HaveCount(2);
	}
}
