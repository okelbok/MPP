using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

using Tracer.Serialization.Abstractions;
using Tracer.Serialization.Yaml.Dto;

namespace Tracer.Serialization.Yaml;

public class YamlTraceResultSerializer : ITraceResultSerializer
{
	public string Format => "yaml";

	public void Serialize(TraceResult traceResult, Stream to)
	{
		YamlTraceResultDto dto = YamlTraceResultDto.FromDomain(traceResult);
		var serializer = new SerializerBuilder()
			.WithNamingConvention(CamelCaseNamingConvention.Instance)
			.Build();

		using StreamWriter writer = new StreamWriter(to, leaveOpen: true);
		serializer.Serialize(writer, dto);
	}
}