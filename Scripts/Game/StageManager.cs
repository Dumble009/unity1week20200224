using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class StageManager : SingletonMonoBehaviour<StageManager>
{
	[SerializeField]
	string problemPath;

	Subject<int> startStageSubject;
	public IObservable<int> OnStartStage {
		get {
			return startStageSubject;
		}
	}

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

	Subject<int> startProblemBarSubject;
	public IObservable<int> OnProblemBarStart {
		get {
			return startProblemBarSubject;
		}
	}

	Subject<int> stopProblemBarSubject;
	public IObservable<int> OnProblemBarStop {
		get {
			return stopProblemBarSubject;
		}
	}

	Subject<int> startPlayerBarSubject;
	public IObservable<int> OnPlayerBarStart {
		get {
			return startPlayerBarSubject;
		}
	}

	Subject<int> stopPlayerBarSubject;
	public IObservable<int> OnPlayerBarStop {
		get {
			return stopPlayerBarSubject;
		}
	}

	IEnumerator playLoopCoroutine;

	Problem problem;

	protected override void Awake()
	{
		base.Awake();
		startStageSubject = new Subject<int>();
		startProblemSubject = new Subject<int>();
		endProblemSubject = new Subject<int>();
		startPlayingSubject = new Subject<int>();
		endPlayingSubject = new Subject<int>();
		startProblemBarSubject = new Subject<int>();
		stopProblemBarSubject = new Subject<int>();
		startPlayerBarSubject = new Subject<int>();
		stopPlayerBarSubject = new Subject<int>();

	}

	private void Start()
	{
		TutorialManager.Instance.OnTutorialFinish
			.Subscribe(_i => {
				StartStage();
			});

		ResultManager.Instance.OnRestart
			.Subscribe(_i => {
				StartStage();
			});
	}

	void StartStage()
	{
		problem = ProblemLoader.LoadProblem(problemPath);
		ProblemManager.Instance.SetProblem(problem);
		BeatManager.Instance.StartBeat();
		playLoopCoroutine = PlayLoop();
		StartCoroutine(playLoopCoroutine);

		ProblemManager.Instance.OnProblemFinish
			.Subscribe(_i => {
				StopCoroutine(playLoopCoroutine);
			});

		startStageSubject.OnNext(0);
	}

	IEnumerator PlayLoop()
	{
		int lastBeat = 0;
		while (true)
		{
			bool isWaitingProblem = true;
			BeatManager.Instance.OnBeat
				.Skip(3)
				.First()
				.Subscribe(_i => {
					isWaitingProblem = false;
					lastBeat = _i;

					startProblemBarSubject.OnNext(_i);
				});

			yield return new WaitWhile(() => isWaitingProblem);
			BeatManager.Instance.OnBeat
				.First()
				.Subscribe(_i => {
					startProblemSubject.OnNext(_i);
				});

			bool isPlayingProblem = true;
			BeatManager.Instance.OnBeat
				.Skip(3)
				.First()
				.Subscribe(_i => {
					isPlayingProblem = false;
					lastBeat = _i;
				});

			yield return new WaitWhile(() =>  isPlayingProblem);
			BeatManager.Instance.OnBeat
				.First()
				.Subscribe(_i => {
					stopProblemBarSubject.OnNext(_i);
				});
			endProblemSubject.OnNext(lastBeat);

			bool isWaitingPlaying = true;
			BeatManager.Instance.OnBeat
				.Skip(3)
				.First()
				.Subscribe(_i => {
					isWaitingPlaying = false;
					lastBeat = _i;

					startPlayerBarSubject.OnNext(_i);
				});

			yield return new WaitWhile(() => isWaitingPlaying);
			startPlayingSubject.OnNext(lastBeat);

			bool isPlayingPlayer = true;
			BeatManager.Instance.OnBeat
				.Skip(3)
				.First()
				.Subscribe(_i => {
					isPlayingPlayer = false;
					lastBeat = _i;
				});

			yield return new WaitWhile(() => isPlayingPlayer);
			BeatManager.Instance.OnBeat
				.First()
				.Subscribe(_i => {
					stopPlayerBarSubject.OnNext(_i);
					endPlayingSubject.OnNext(_i);
				});
		}
	}
}
