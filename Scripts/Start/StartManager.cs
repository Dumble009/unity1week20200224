using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class StartManager : SingletonMonoBehaviour<StartManager>
{
	[SerializeField]
	GameObject[] notations;

	[SerializeField]
	GameObject startMenuRoot;

	Subject<int> startSubject;
	public IObservable<int> OnStart {
		get {
			return startSubject;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		startSubject = new Subject<int>();
	}

	public void PushStartButton()
	{
		startMenuRoot.SetActive(false);
		foreach (var notation in notations)
		{
			notation.SetActive(true);
		}
		startSubject.OnNext(0);
	}
}
