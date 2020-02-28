using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Net;

public class ProblemLoader
{
	static bool isCached = false;
	static string cache;
	public static Problem LoadProblem(string path)
	{
		string jsonBody = "";
		if (!isCached || string.IsNullOrEmpty(cache))
		{
			WebClient wc = new WebClient();
			using (Stream wwwStream = wc.OpenRead(path))
			{
				StreamReader sr = new StreamReader(wwwStream);
				jsonBody = sr.ReadToEnd();
			}
			cache = jsonBody;
		}
		else
		{
			jsonBody = cache;
		}


		using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonBody)))
		{
			var serializer = new DataContractJsonSerializer(typeof(Problem));
			Problem p = serializer.ReadObject(ms) as Problem;

			return p;
		}
	}
}
