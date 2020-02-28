using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : SingletonMonoBehaviour<ResultManager>
{
	[SerializeField]
	TextMeshProUGUI perfectCount, greatCount, goodCount, badCount, totalScore, firstRank, secondRank, thirdRank, fourthRank, fifthRank;

	[SerializeField]
	GameObject resultRoot;

	private void Start()
	{
		resultRoot.SetActive(false);
	}

	public void ShowResult()
	{
		resultRoot.SetActive(true);
	}
}
