using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class BeatManager : SingletonMonoBehaviour<BeatManager>
{
	public int BPM { get; set; } = 120;

	bool isPlaying;
	public bool IsPlaying {
		get {
			return isPlaying;
		}
	}

	private Subject<int> beatSubject;
	public IObservable<int> OnBeat {
		get {
			return beatSubject;
		}
	}

	private Subject<float> timeInBarSubject;
	public IObservable<float> OnTimeInBar {
		get {
			return timeInBarSubject;
		}
	}

	IEnumerator beatCoroutine;

	private int beatCount;
	public int BeatCount {
		get {
			return beatCount;
		}
	}

	float lastBeatTime = 0;

	protected override void Awake()
	{
		base.Awake();
		beatSubject = new Subject<int>();
		timeInBarSubject = new Subject<float>();
	}

	private void Start()
	{
		ProblemManager.Instance.OnProblemFinish
			.Subscribe(_i => {
				StopBeat();
			});
	}

	private void Update()
	{
		if (isPlaying)
		{
			float secondPerBeat = 60.0f / BPM;
			float currentTime = Time.time;
			float floatBeat = beatCount + (currentTime - lastBeatTime) / secondPerBeat;

			timeInBarSubject.OnNext(floatBeat);
		}
	}

	public void StartBeat()
	{
		beatCount = 0;
		lastBeatTime = 0;
		if (beatCoroutine != null)
		{
			StopCoroutine(beatCoroutine);
		}
		beatCoroutine = BeatCoroutine();
		StartCoroutine(beatCoroutine);
	}

	void StopBeat()
	{
		if (beatCoroutine != null)
		{
			StopCoroutine(beatCoroutine);
		}
	}

	IEnumerator BeatCoroutine()
	{
		yield return null;
		float waitSecond = 60.0f / BPM;
		beatCount = -1;
		isPlaying = true;
		while (isPlaying)
		{
			yield return new WaitForSeconds(waitSecond);
			lastBeatTime = Time.time;
			beatCount++;
			beatSubject.OnNext(beatCount);
		}
	}
}
