using System.Xml.Serialization;

using Tracer.Core;

namespace Tracer.Serialization.Xml.Dto;

[XmlRoot("root")]
public class XmlTraceResultDto
{
	[XmlElement("thread")]
	public List<XmlThreadResultDto> Threads { get; set; } = new();

	public static XmlTraceResultDto FromDomain(TraceResult result) => new()
	{
		Threads = result.Threads.Select(XmlThreadResultDto.FromDomain).ToList()
	};
}