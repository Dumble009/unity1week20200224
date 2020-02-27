using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public enum Score
{
	PERFECT,
	GREAT,
	GOOD,
	BAD
}

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
	List<PlayerNode> playerNodes;
	Subject<Score> calcScoreSubject;
	public IObservable<Score> OnCalcScore {
		get {
			return calcScoreSubject;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		calcScoreSubject = new Subject<Score>();
	}

	private void Start()
	{
		playerNodes = new List<PlayerNode>();
		StageManager.Instance.OnPlayerBarStop
			.Subscribe(_i => {
				Score score = CalcScore(ProblemManager.Instance.GetCurrentBar());
				calcScoreSubject.OnNext(score);
			});
	}

	public Score CalcScore(Bar bar)
	{
		Score result = Score.BAD;
		int badPoint = 0;
		Node[] problemNodes = bar.nodes;
		badPoint = Mathf.Abs(playerNodes.Count - problemNodes.Length);
		foreach (var playerNode in playerNodes)
		{
			bool isMatched = false;
			foreach (var problemNode in problemNodes)
			{
				if (playerNode.Pitch == problemNode.Pitch && Mathf.Abs(playerNode.Timing - problemNode.Timing) < 0.125f)
				{
					isMatched = true;
					break;
				}
			}

			if (!isMatched)
			{
				badPoint++;
			}
		}

		switch (badPoint) {
			case 0:
				result = Score.PERFECT;
				break;

			case 1:
				result = Score.GREAT;
				break;

			case 2:
				result = Score.GOOD;
				break;

			default:
				result = Score.BAD;
				break;
		}
		return result;
	}

	public void AddPlayerNode(int pitch, int timing)
	{
		PlayerNode n = new PlayerNode(pitch, timing);
		playerNodes.Add(n);
	}
}

public class PlayerNode
{
	int pitch;
	public int Pitch {
		get {
			return pitch;
		}
	}
	int timing;
	public int Timing {
		get {
			return timing;
		}
	}
	public bool isMatch;

	public PlayerNode(int _pitch, int _timing)
	{
		pitch = _pitch;
		timing = _timing;
		isMatch = false;
	}
}