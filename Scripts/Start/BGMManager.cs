using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BGMManager : MonoBehaviour
{
	AudioSource audioSource;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		StageManager.Instance.OnStartStage
			.Subscribe(_i => {
				audioSource.Stop();
			});
	}
}
