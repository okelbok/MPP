using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization.Json.Dto;

internal class JsonTraceResultDto
{
	public List<JsonThreadResultDto> Threads { get; set; } = new();

	public static JsonTraceResultDto FromDomain(TraceResult result) => new()
	{
		Threads = result.Threads.Select(JsonThreadResultDto.FromDomain).ToList()
	};
}