using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Net;
using UnityEngine;

public class ProblemLoader : SingletonMonoBehaviour<ProblemLoader>
{
	[SerializeField]
	string path;
	private IEnumerator Start()
	{
		var request = UnityEngine.Networking.UnityWebRequest.Get(path);
		yield return request.SendWebRequest();

		cache = request.downloadHandler.text;
		isCached = true;
	}

	static public bool isCached = false;
	static string cache;
	public static Problem LoadProblem()
	{
		string jsonBody = cache;
		
		using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonBody)))
		{
			var serializer = new DataContractJsonSerializer(typeof(Problem));
			
			Problem p = serializer.ReadObject(ms) as Problem;

			return p;
		}
	}
}
