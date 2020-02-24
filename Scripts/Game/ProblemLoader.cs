using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

public class ProblemLoader
{
	public static Problem LoadProblem(string path)
	{
		string jsonBody = File.ReadAllText(path);

		using(var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonBody)))
		{
			var serializer = new DataContractJsonSerializer(typeof(Problem));
			Problem p = serializer.ReadObject(ms) as Problem;

			return p;
		}
	}
}
