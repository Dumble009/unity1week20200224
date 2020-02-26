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

	Subject<int> endProblemSubject;
	public IObservable<int> OnEndProblem {
		get {
			return endProblemSubject;
		}
	}

	Subject<int> startPlayingSubject;
	public IObservable<int> OnStartPlaying {
		get {
			return startPlayingSubject;
		}
	}

	Subject<int> endPlayingSubject;
	public IObservable<int> OnEndPlaying {
		get {
			return endPlayingSubject;
		}
	}

	protected override void Awake()
	{
		base.Awake();

		startProblemSubject = new Subject<int>();
		endProblemSubject = new Subject<int>();
		startPlayingSubject = new Subject<int>();
		endPlayingSubject = new Subject<int>();
	}

	private void Start()
	{
		Problem problem = ProblemLoader.LoadProblem(problemPath);
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
				.Skip(3)
				.Subscribe(_i => {
					isWaitingProblem = false;
					lastBeat = _i;
				});

			yield return new WaitWhile(() => isWaitingProblem);

			startProblemSubject.OnNext(lastBeat);

			bool isPlayingProblem = true;
			BeatManager.Instance.OnBeat
				.Skip(3)
				.Subscribe(_i => {
					isPlayingProblem = false;
					lastBeat = _i;
				});

			yield return new WaitWhile(() =>  isPlayingProblem);

			endProblemSubject.OnNext(lastBeat);

			bool isWaitingPlaying = true;
			BeatManager.Instance.OnBeat
				.Skip(3)
				.Subscribe(_i => {
					isWaitingPlaying = false;
					lastBeat = _i;
				});

			yield return new WaitWhile(() => isWaitingPlaying);

			startPlayingSubject.OnNext(lastBeat);

			bool isPlayingPlayer = true;
			BeatManager.Instance.OnBeat
				.Skip(3)
				.Subscribe(_i => {
					isPlayingPlayer = false;
					lastBeat = _i;
				});

			yield return new WaitWhile(() => isPlayingPlayer);
			endPlayingSubject.OnNext(lastBeat);
		}
	}
}
