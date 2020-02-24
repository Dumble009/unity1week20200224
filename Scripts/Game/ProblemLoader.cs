using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Net;

public class ProblemLoader
{
	public static Problem LoadProblem(string path)
	{
		string jsonBody = "";
		WebClient wc = new WebClient();
		using (Stream wwwStream = wc.OpenRead(path))
		{
			StreamReader sr = new StreamReader(wwwStream);
			jsonBody = sr.ReadToEnd();
		}


		using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonBody)))
		{
			var serializer = new DataContractJsonSerializer(typeof(Problem));
			Problem p = serializer.ReadObject(ms) as Problem;

			return p;
		}
	}
}
