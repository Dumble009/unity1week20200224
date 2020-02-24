using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class NodePlayer : MonoBehaviour
{
	[SerializeField]
	AudioSource audioSource;
	[SerializeField]
	float[] pitches;

	private void Awake()
	{
		if (!audioSource)
		{
			audioSource = GetComponent<AudioSource>();
		}
	}

	private void Start()
	{
		ProblemManager.Instance.OnNode.Subscribe(_i => {
			PlayNode(_i);
		});
	}

	void PlayNode(int pitch)
	{
		if (audioSource)
		{
			audioSource.pitch = pitches[pitch];
			audioSource.PlayOneShot(audioSource.clip);
		}
	}
}
