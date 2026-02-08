using System.Xml.Serialization;

using Tracer.Core;

namespace Tracer.Serialization.Xml.Dto;

public class XmlThreadResultDto
{
	[XmlAttribute("id")]
	public int Id { get; set; }

	[XmlAttribute("time")]
	public string Time { get; set; } = string.Empty;

	[XmlElement("method")]
	public List<XmlMethodResultDto> Methods { get; set; } = new();

	public static XmlThreadResultDto FromDomain(ThreadTraceResult t) => new()
	{
		Id = t.ThreadId,
		Time = $"{t.Time}ms",
		Methods = t.Methods.Select(XmlMethodResultDto.FromDomain).ToList()
	};
}