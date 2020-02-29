using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using System;

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
	Dictionary<TextMeshProUGUI, string> defaultTexts;

	public int PerfectCount { get; set; } = 0;
	public int GreatCount { get; set; } = 0;
	public int GoodCount { get; set; } = 0;
	public int BadCount { get; set; } = 0;

	[SerializeField]
	TextMeshProUGUI[] rankingsTMPro;

	[SerializeField]
	GameObject resultRoot;

	Subject<int> restartSubject;
	public IObservable<int> OnRestart {
		get {
			return restartSubject;
		}
	}

	int changedIndex = -1;
	IEnumerator textFlashAnimation;

	protected override void Awake()
	{
		base.Awake();
		restartSubject = new Subject<int>();
		defaultTexts = new Dictionary<TextMeshProUGUI, string>();
	}

	private void Start()
	{
		resultRoot.SetActive(false);
		defaultTexts.Add(perfectCountTMPro, perfectCountTMPro.text);
		defaultTexts.Add(greatCountTMPro, greatCountTMPro.text);
		defaultTexts.Add(goodCountTMPro, goodCountTMPro.text);
		defaultTexts.Add(badCountTMPro, badCountTMPro.text);
		defaultTexts.Add(totalScoreTMPro, totalScoreTMPro.text);

		foreach (var ranking in rankingsTMPro)
		{
			defaultTexts.Add(ranking, ranking.text);
		}
		ProblemManager.Instance.OnProblemFinish
			.Subscribe(_i => {
				ShowResult();

				PerfectCount = 0;
				GreatCount = 0;
				GoodCount = 0;
				BadCount = 0;
			});

		ResultManager.Instance.OnRestart
			.Subscribe(_i => {
				ResetTMPros();
				if (textFlashAnimation != null)
				{
					StopCoroutine(textFlashAnimation);
					Color c = rankingsTMPro[changedIndex].color;
					c.a = 1;
					rankingsTMPro[changedIndex].color = c;
				}
			});
	}

	public void ShowResult()
	{
		resultRoot.SetActive(true);
		//SetRanking(RankingManager.Instance.Packet);

		int totalScore = PerfectCount * 1000 + GreatCount * 500 + GoodCount * 300 + BadCount * 0;

		UpdateRanking(RankingManager.Instance.Packet, totalScore);
		textFlashAnimation = RankingFlashAnimation();
		StartCoroutine(textFlashAnimation);

		RankingManager.Instance.SetScore(totalScore);

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

	public void UpdateRanking(RankingPacket packet, int totalScore)
	{
		int tmpIndex = 0;
		bool isChanged = false;
		for (int i = 0; i < RankingPacket.RankingCount; i++)
		{
			if (!isChanged && packet.Scores[i] <= totalScore)
			{
				rankingsTMPro[i].text = string.Format(rankingsTMPro[i].text, packet.Ranks[i], totalScore);
				isChanged = true;
				changedIndex = i;
			}
			else
			{
				rankingsTMPro[i].text = string.Format(rankingsTMPro[i].text, packet.Ranks[i], packet.Scores[tmpIndex]);
				tmpIndex++;
			}
		}

		if (!isChanged)
		{
			changedIndex = -1;
		}
	}

	public void Restart()
	{
		resultRoot.SetActive(false);
		restartSubject.OnNext(0);
	}

	void ResetTMPros()
	{
		foreach (var tmpro in defaultTexts.Keys)
		{
			tmpro.text = defaultTexts[tmpro];
		}
	}

	IEnumerator RankingFlashAnimation()
	{
		if (changedIndex != -1)
		{
			Color c = rankingsTMPro[changedIndex].color;
			while (true)
			{
				c.a = 0;
				rankingsTMPro[changedIndex].color = c;
				yield return new WaitForSeconds(0.5f);

				c.a = 1;
				rankingsTMPro[changedIndex].color = c;
				yield return new WaitForSeconds(0.5f);
			}
		}
		else
		{
			yield break;
		}
	}
}
