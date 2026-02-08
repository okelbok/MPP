using Tracer.Core;

namespace Tracer.Serialization.Json.Dto;

internal class JsonThreadResultDto
{
	public int Id { get; set; }
	public string Time { get; set; } = string.Empty;
	public List<JsonMethodResultDto> Methods { get; set; } = new();

	public static JsonThreadResultDto FromDomain(ThreadTraceResult t) => new()
	{
		Id = t.ThreadId,
		Time = $"{t.Time}ms",
		Methods = t.Methods.Select(JsonMethodResultDto.FromDomain).ToList()
	};
}