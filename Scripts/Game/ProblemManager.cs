using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using UniRx;
using System;

public class ProblemManager : SingletonMonoBehaviour<ProblemManager>
{
	Subject<int> nodeSubject;
	public IObservable<int> OnNode {
		get {
			return nodeSubject;
		}
	}

	Subject<int> finishProblemSubject;
	public IObservable<int> OnProblemFinish {
		get {
			return finishProblemSubject;
		}
	}

	Problem currentProblem;
	int barIndex = -1;
	int nodeIndex = 0;

	override protected void Awake()
	{
		base.Awake();
		nodeSubject = new Subject<int>();
		finishProblemSubject = new Subject<int>();
	}

	private void Start()
	{
		StageManager.Instance.OnProblemBarStart
			.Subscribe(_i => {

				barIndex++;
				if (barIndex >= currentProblem.bars.Length)
				{
					finishProblemSubject.OnNext(0);
				}
			});
	}

	public void SetProblem(Problem problem)
	{
		nodeIndex = 0;
		barIndex = -1;
		currentProblem = problem;
		StageManager.Instance.OnStartProblem
			.Subscribe(_i => {
				StartProblem(_i);
			});
	}

	void StartProblem(int startBeat)
	{
		nodeIndex = 0;

		BeatManager.Instance.OnTimeInBar
			.TakeWhile(x =>
				barIndex < currentProblem.bars.Length &&
				nodeIndex < currentProblem.bars[barIndex].nodes.Length)
			.Where(x =>
				x - startBeat >= currentProblem.bars[barIndex].nodes[nodeIndex].Timing)
			.Subscribe(_f =>
			{
				nodeSubject.OnNext(currentProblem.bars[barIndex].nodes[nodeIndex].Pitch);
				nodeIndex++;
			});
	}



	public Bar GetCurrentBar()
	{
		if (barIndex < currentProblem.bars.Length)
		{
			return currentProblem.bars[barIndex];
		}
		else
		{
			return currentProblem.bars[0];
		}
	}
}

[DataContract]
public class Problem
{
	[DataMember(Name = "bars")]
	public Bar[] bars;
}

[DataContract]
public class Bar
{
	[DataMember(Name = "nodes")]
	public Node[] nodes;
}

[DataContract]
public class Node
{
	[DataMember(Name = "timing")]
	public float Timing;
	[DataMember(Name = "pitch")]
	public int Pitch;
}
