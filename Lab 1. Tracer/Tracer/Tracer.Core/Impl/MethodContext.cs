using System.Diagnostics;

namespace Tracer.Core.Impl;

internal class MethodContext
{
	public string MethodName { get; }
	public string ClassName { get; }
	public Stopwatch Stopwatch { get; }
	public List<MethodContext> Children { get; } = new();

	public MethodContext(string methodName, string className)
	{
		MethodName = methodName;
		ClassName = className;
		Stopwatch = new Stopwatch();
	}
}