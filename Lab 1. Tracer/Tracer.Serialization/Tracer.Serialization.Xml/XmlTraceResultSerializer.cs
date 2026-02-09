using System.Xml.Serialization;

using Tracer.Serialization.Abstractions;
using Tracer.Serialization.Xml.Dto;

namespace Tracer.Serialization.Xml;

public class XmlTraceResultSerializer : ITraceResultSerializer
{
	public string Format => "xml";

	public void Serialize(TraceResult traceResult, Stream to)
	{
		XmlTraceResultDto dto = XmlTraceResultDto.FromDomain(traceResult);
		var serializer = new XmlSerializer(typeof(XmlTraceResultDto));

		serializer.Serialize(to, dto);
	}
}