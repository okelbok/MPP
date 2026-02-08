using System.Reflection;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization;

public class SerializerLoader
{
	public IReadOnlyList<ITraceResultSerializer> LoadSerializers(string pluginDirectory)
	{
		var serializers = new List<ITraceResultSerializer>();

		if (!Directory.Exists(pluginDirectory))
		{
			return serializers;
		}

		string[] filePaths = Directory.GetFiles(pluginDirectory, "*.dll");

		foreach (string filePath in filePaths)
		{
			try
			{
				Assembly assembly = Assembly.LoadFrom(filePath);

				var types = assembly.GetTypes()
					.Where(t => typeof(ITraceResultSerializer).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

				foreach (Type type in types)
				{
					if (Activator.CreateInstance(type) is ITraceResultSerializer serializer)
					{
						serializers.Add(serializer);
					}
				}
			}
			catch (Exception)
			{
				// Ignore assemblies that are not valid plugins or cannot be loaded
			}
		}

		return serializers;
	}
}