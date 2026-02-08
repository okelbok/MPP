using System.Collections.Concurrent;
using System.Diagnostics;

using Tracer.Core.Impl;

namespace Tracer.Core;

public class Tracer : ITracer
{
	private readonly ConcurrentDictionary<int, ThreadContext> _threadContexts = new();

	public void StartTrace()
	{
		int threadId = Environment.CurrentManagedThreadId;
		var context = _threadContexts.GetOrAdd(threadId, _ => new ThreadContext());

		var stackFrame = new StackTrace(1).GetFrame(0);
		var methodBase = stackFrame?.GetMethod();

		if (methodBase == null)
		{
			return;
		}

		string methodName = methodBase.Name;
		string className = methodBase.DeclaringType?.Name ?? "Unknown";

		MethodContext methodContext = new MethodContext(methodName, className);

		if (context.CallStack.Count > 0)
		{
			MethodContext parent = context.CallStack.Peek();
			parent.Children.Add(methodContext);
		}
		else
		{
			context.RootMethods.Add(methodContext);
		}

		context.CallStack.Push(methodContext);
		methodContext.Stopwatch.Start();
	}

	public void StopTrace()
	{
		int threadId = Environment.CurrentManagedThreadId;

		if (!_threadContexts.TryGetValue(threadId, out var context))
		{
			return;
		}

		if (context.CallStack.Count == 0)
		{
			return;
		}

		MethodContext methodContext = context.CallStack.Pop();
		methodContext.Stopwatch.Stop();
	}

	public TraceResult GetTraceResult()
	{
		var threadResults = new List<ThreadTraceResult>();

		foreach (var entry in _threadContexts)
		{
			int threadId = entry.Key;
			ThreadContext context = entry.Value;

			var rootMethods = MapMethods(context.RootMethods);
			long totalTime = rootMethods.Sum(x => x.Time);

			threadResults.Add(new ThreadTraceResult(threadId, totalTime, rootMethods));
		}

		return new TraceResult(threadResults.AsReadOnly());
	}

	private IReadOnlyList<MethodTraceResult> MapMethods(List<MethodContext> methods)
	{
		var results = new List<MethodTraceResult>();

		foreach (MethodContext method in methods)
		{
			var children = MapMethods(method.Children);

			results.Add(new MethodTraceResult(
				method.MethodName,
				method.ClassName,
				method.Stopwatch.ElapsedMilliseconds,
				children));
		}

		return results.AsReadOnly();
	}
}