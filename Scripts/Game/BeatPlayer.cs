using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BeatPlayer : MonoBehaviour
{
	[SerializeField]
	AudioSource audioSource;

	private void Awake()
	{
		if (!audioSource)
		{
			audioSource = GetComponent<AudioSource>();
		}
	}

	private void Start()
	{
		BeatManager.Instance.OnBeat.Subscribe(_i => {
			PlayBeat();
		});
	}

	void PlayBeat() {
		if (audioSource)
		{
			audioSource.PlayOneShot(audioSource.clip);
		}
	}
}
