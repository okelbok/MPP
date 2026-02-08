using Tracer.Core;
using Tracer.Serialization;
using Tracer.Serialization.Abstractions;

namespace Tracer.Example;

public class Program
{
	public static void Main(string[] args)
	{
		ITracer tracer = new Core.Tracer();
		Foo foo = new Foo(tracer);

		Thread t1 = new Thread(foo.MyMethod);
		Thread t2 = new Thread(foo.MyMethod);

		t1.Start();
		t2.Start();

		t1.Join();
		t2.Join();

		TraceResult result = tracer.GetTraceResult();

		string pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
		SerializerLoader loader = new SerializerLoader();
		var serializers = loader.LoadSerializers(pluginPath);

		if (!serializers.Any())
		{
			Console.WriteLine($"No plugins found in {pluginPath}. Ensure plugins are built and copied there.");

			return;
		}

		foreach (ITraceResultSerializer serializer in serializers)
		{
			string fileName = $"result.{serializer.Format}";
			
			using FileStream fileStream = new FileStream(fileName, FileMode.Create);

			serializer.Serialize(result, fileStream);

			Console.WriteLine($"Result saved to {fileName}");
		}
	}
}

public class Foo
{
	private readonly Bar _bar;
	private readonly ITracer _tracer;

	internal Foo(ITracer tracer)
	{
		_tracer = tracer;
		_bar = new Bar(_tracer);
	}

	public void MyMethod()
	{
		_tracer.StartTrace();
		Thread.Sleep(100);
		_bar.InnerMethod();
		_tracer.StopTrace();
	}
}

public class Bar
{
	private readonly ITracer _tracer;

	internal Bar(ITracer tracer)
	{
		_tracer = tracer;
	}

	public void InnerMethod()
	{
		int delay = Random.Shared.Next(50, 200);

		_tracer.StartTrace();
		Thread.Sleep(delay);
		_tracer.StopTrace();
	}
}