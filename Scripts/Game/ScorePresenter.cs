using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ScorePresenter : MonoBehaviour
{
	[SerializeField]
	GameObject perfect, great, good, bad;

	[SerializeField]
	AudioClip perfectClip, greatClip, goodClip, badClip;

	AudioSource audioSource;

	[SerializeField]
	float showSeconds = 1.5f;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		ScoreManager.Instance.OnCalcScore
			.Subscribe(_s => {
				ShowScore(_s);
			});
	}

	void ShowScore(Score score)
	{
		GameObject target = null;
		switch (score) {
			case Score.PERFECT:
				target = perfect;
				ResultManager.Instance.PerfectCount++;
				if (audioSource)
				{
					audioSource.PlayOneShot(perfectClip);
				}
				break;

			case Score.GREAT:
				target = great;
				ResultManager.Instance.GreatCount++;
				if (audioSource)
				{
					audioSource.PlayOneShot(greatClip);
				}
				break;

			case Score.GOOD:
				target = good;
				ResultManager.Instance.GoodCount++;
				if (audioSource)
				{
					audioSource.PlayOneShot(goodClip);
				}
				break;

			case Score.BAD:
				target = bad;
				ResultManager.Instance.BadCount++;
				if (audioSource)
				{
					audioSource.PlayOneShot(badClip);
				}
				break;

			default:
				break;
		}

		if (target)
		{
			StartCoroutine(ScoreAnimation(target));
		}
	}

	IEnumerator ScoreAnimation(GameObject targetObject) {
		targetObject.SetActive(true);

		yield return new WaitForSeconds(showSeconds);

		targetObject.SetActive(false);
	}
}
