using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SingletonMonoBehaviour<StageManager>
{
	[SerializeField]
	string problemPath;

	private void Start()
	{
		Problem problem = ProblemLoader.LoadProblem(problemPath);
		ProblemManager.Instance.SetProblem(problem);
		BeatManager.Instance.StartBeat();
	}
}
