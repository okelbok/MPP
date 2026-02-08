namespace Tracer.Core.Impl;

internal class ThreadContext
{
	public Stack<MethodContext> CallStack { get; } = new();
	public List<MethodContext> RootMethods { get; } = new();
}