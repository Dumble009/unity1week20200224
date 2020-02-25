using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class StageManager : SingletonMonoBehaviour<StageManager>
{
	[SerializeField]
	string problemPath;

	Subject<int> startProblemSubject;
	public IObservable<int> OnStartProblem {
		get {
			return startProblemSubject;
		}
	}

	private void Start()
	{
		Problem problem = ProblemLoader.LoadProblem(problemPath);
		startProblemSubject = new Subject<int>();
		ProblemManager.Instance.SetProblem(problem);
		BeatManager.Instance.StartBeat();
		StartCoroutine(PlayLoop());
	}

	IEnumerator PlayLoop()
	{
		int lastBeat = 0;
		while (true)
		{
			bool isWaitingProblem = true;
			BeatManager.Instance.OnBeat
				.Where(x => x % 4 == 0 && x != 0)
				.First()
				.Subscribe(_i => {
					isWaitingProblem = false;
					lastBeat = _i;
				});

			yield return new WaitWhile(() => isWaitingProblem);

			startProblemSubject.OnNext(lastBeat + 1);

			bool isPlayingProblem = true;
			BeatManager.Instance.OnBeat
				.Where(x => x % 4 == 0 && x != 0)
				.First()
				.Subscribe(_i => {
					isPlayingProblem = false;
					lastBeat = _i;
				});

			yield return new WaitWhile(() =>  isPlayingProblem);
		}
	}
}
