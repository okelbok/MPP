using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization.Yaml.Dto;

internal class YamlThreadResultDto
{
	public int Id { get; set; }
	public string Time { get; set; } = string.Empty;
	public List<YamlMethodResultDto> Methods { get; set; } = new();

	public static YamlThreadResultDto FromDomain(ThreadTraceResult t) => new()
	{
		Id = t.ThreadId,
		Time = $"{t.Time}ms",
		Methods = t.Methods.Select(YamlMethodResultDto.FromDomain).ToList()
	};
}