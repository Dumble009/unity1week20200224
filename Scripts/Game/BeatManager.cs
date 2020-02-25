﻿using System.Collections;
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
		if (beatCoroutine != null)
		{
			StopCoroutine(beatCoroutine);
		}
		beatCoroutine = BeatCoroutine();
		StartCoroutine(beatCoroutine);
	}

	IEnumerator BeatCoroutine()
	{
		float waitSecond = 60.0f / BPM;
		beatCount = 0;
		isPlaying = true;
		while (isPlaying)
		{
			lastBeatTime = Time.time;
			beatSubject.OnNext(beatCount);
			yield return new WaitForSeconds(waitSecond);
			beatCount++;
		}
	}
}
