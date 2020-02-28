using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;

public class RankingPacket
{
	public static int RankingCount = 5;
	public int[] Ranks { get; set; }
	public int[] Scores { get; set; }

	public RankingPacket()
	{
		Ranks = new int[RankingCount];
		Scores = new int[RankingCount];
	}
}

public class ResultManager : SingletonMonoBehaviour<ResultManager>
{
	[SerializeField]
	TextMeshProUGUI perfectCountTMPro, greatCountTMPro, goodCountTMPro, badCountTMPro, totalScoreTMPro;

	public int PerfectCount { get; set; } = 0;
	public int GreatCount { get; set; } = 0;
	public int GoodCount { get; set; } = 0;
	public int BadCount { get; set; } = 0;

	[SerializeField]
	TextMeshProUGUI[] rankingsTMPro;

	[SerializeField]
	GameObject resultRoot;

	private void Start()
	{
		resultRoot.SetActive(false);
		ProblemManager.Instance.OnProblemFinish
			.Subscribe(_i => {
				ShowResult();
			});
	}

	public void ShowResult()
	{
		resultRoot.SetActive(true);
		int totalScore = PerfectCount * 1000 + GreatCount * 500 + GoodCount * 300 + BadCount * 0;
		totalScoreTMPro.text = string.Format(totalScoreTMPro.text, totalScore);

		perfectCountTMPro.text = string.Format(perfectCountTMPro.text, PerfectCount);
		greatCountTMPro.text = string.Format(greatCountTMPro.text, GreatCount);
		goodCountTMPro.text = string.Format(goodCountTMPro.text, GoodCount);
		badCountTMPro.text = string.Format(badCountTMPro.text, BadCount);
	}

	public void SetRanking(RankingPacket packet)
	{
		for (int i = 0; i < RankingPacket.RankingCount; i++)
		{
			rankingsTMPro[i].text = string.Format(rankingsTMPro[i].text, packet.Ranks[i], packet.Scores[i]);
		}
	}
}
