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

	Problem currentProblem;
	int barIndex = 0;
	int nodeIndex = 0;

	override protected void Awake()
	{
		base.Awake();
		nodeSubject = new Subject<int>();
	}

	public void SetProblem(Problem problem)
	{
		currentProblem = problem;
		BeatManager.Instance.OnBeat
			.Where(x => x % 4 == 0 && x != 0)
			.Subscribe(_i => {
				barIndex++;
				nodeIndex = 0;
			});

		BeatManager.Instance.OnTimeInBar
			.Where(x => 
				barIndex < currentProblem.bars.Length &&
				nodeIndex < currentProblem.bars[barIndex].nodes.Length && 
				x >= currentProblem.bars[barIndex].nodes[nodeIndex].Timing)
			.Subscribe(_f => {
				nodeSubject.OnNext(currentProblem.bars[barIndex].nodes[nodeIndex].Pitch);
				Debug.Log(_f.ToString() + ":" + barIndex.ToString() + ":" + nodeIndex.ToString());
				nodeIndex++;
			});
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
