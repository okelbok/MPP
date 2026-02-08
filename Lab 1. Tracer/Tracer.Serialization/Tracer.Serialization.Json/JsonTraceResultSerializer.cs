using System.Text.Json;

using Tracer.Core;
using Tracer.Serialization.Abstractions;
using Tracer.Serialization.Json.Dto;

namespace Tracer.Serialization.Json;

public class JsonTraceResultSerializer : ITraceResultSerializer
{
	public string Format => "json";

	public void Serialize(TraceResult traceResult, Stream to)
	{
		JsonTraceResultDto dto = JsonTraceResultDto.FromDomain(traceResult);
		var options = new JsonSerializerOptions
		{
			WriteIndented = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};

		JsonSerializer.Serialize(to, dto, options);
	}
}