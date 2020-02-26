using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class InputAgent : SingletonMonoBehaviour<InputAgent>
{
	bool isPlaying = false;

	Subject<int> inputSubject;
	public IObservable<int> OnInput {
		get {
			return inputSubject;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		inputSubject = new Subject<int>();
	}

	private void Start()
	{
		StageManager.Instance.OnStartPlaying
			.Subscribe(_i => {
				isPlaying = true;
			});

		StageManager.Instance.OnEndPlaying
			.Subscribe(_i => {
				isPlaying = false;
			});
	}

	private void Update()
	{
		if (isPlaying)
		{
			if (Input.GetButtonDown("HighPitch"))
			{
				inputSubject.OnNext(2);
			}
			else if (Input.GetButtonDown("MidPitch"))
			{
				inputSubject.OnNext(1);
			}
			else if (Input.GetButtonDown("LowPitch"))
			{
				inputSubject.OnNext(0);
			}
		}
	}
}
