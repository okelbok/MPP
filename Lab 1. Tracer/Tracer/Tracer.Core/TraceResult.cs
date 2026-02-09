namespace Tracer.Core;

public record TraceResult(IReadOnlyList<ThreadTraceResult> Threads);

public record ThreadTraceResult(int ThreadId, long Time, IReadOnlyList<MethodTraceResult> Methods);

public record MethodTraceResult(string Name, string ClassName, long Time, IReadOnlyList<MethodTraceResult> Methods);