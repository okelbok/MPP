using System.Xml.Serialization;

using Tracer.Core;

namespace Tracer.Serialization.Xml.Dto;

public class XmlMethodResultDto
{
	[XmlAttribute("name")]
	public string Name { get; set; } = string.Empty;

	[XmlAttribute("time")]
	public string Time { get; set; } = string.Empty;

	[XmlAttribute("class")]
	public string Class { get; set; } = string.Empty;

	[XmlElement("method")]
	public List<XmlMethodResultDto> Methods { get; set; } = new();

	public static XmlMethodResultDto FromDomain(MethodTraceResult m) => new()
	{
		Name = m.Name,
		Class = m.ClassName,
		Time = $"{m.Time}ms",
		Methods = m.Methods.Select(FromDomain).ToList()
	};
}