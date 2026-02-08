using Tracer.Core;

namespace Tracer.Serialization.Json.Dto;

internal class JsonMethodResultDto
{
	public string Name { get; set; } = string.Empty;
	public string Class { get; set; } = string.Empty;
	public string Time { get; set; } = string.Empty;
	public List<JsonMethodResultDto> Methods { get; set; } = new();

	public static JsonMethodResultDto FromDomain(MethodTraceResult m) => new()
	{
		Name = m.Name,
		Class = m.ClassName,
		Time = $"{m.Time}ms",
		Methods = m.Methods.Select(FromDomain).ToList()
	};
}