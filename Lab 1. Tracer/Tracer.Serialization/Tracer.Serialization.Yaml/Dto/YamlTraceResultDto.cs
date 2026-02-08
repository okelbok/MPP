using Tracer.Core;

namespace Tracer.Serialization.Yaml.Dto;

internal class YamlTraceResultDto
{
	public List<YamlThreadResultDto> Threads { get; set; } = new();

	public static YamlTraceResultDto FromDomain(TraceResult result) => new()
	{
		Threads = result.Threads.Select(YamlThreadResultDto.FromDomain).ToList()
	};
}