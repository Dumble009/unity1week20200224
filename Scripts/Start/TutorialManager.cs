using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class TutorialManager : SingletonMonoBehaviour<TutorialManager>
{
	[SerializeField]
	GameObject tutorialRoot;

	Subject<int> tutorialSubject;
	public IObservable<int> OnTutorialFinish {
		get {
			return tutorialSubject;
		}
	}

	bool isTutorialing = false;

	protected override void Awake()
	{
		base.Awake();
		tutorialSubject = new Subject<int>();
	}

	private void Start()
	{
		tutorialRoot.SetActive(false);
		StartManager.Instance.OnStart
			.Subscribe(_i => {
				tutorialRoot.SetActive(true);
				isTutorialing = true;
			});
	}

	private void Update()
	{
		if (Input.GetButtonDown("Submit") && isTutorialing && ProblemLoader.isCached)
		{
			tutorialRoot.SetActive(false);
			tutorialSubject.OnNext(0);
			isTutorialing = false;
		}
	}
}
