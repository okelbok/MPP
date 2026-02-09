using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization.Yaml.Dto;

internal class YamlMethodResultDto
{
	public string Name { get; set; } = string.Empty;
	public string Class { get; set; } = string.Empty;
	public string Time { get; set; } = string.Empty;
	public List<YamlMethodResultDto> Methods { get; set; } = new();

	public static YamlMethodResultDto FromDomain(MethodTraceResult m) => new()
	{
		Name = m.Name,
		Class = m.ClassName,
		Time = $"{m.Time}ms",
		Methods = m.Methods.Select(FromDomain).ToList()
	};
}