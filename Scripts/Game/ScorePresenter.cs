using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ScorePresenter : MonoBehaviour
{
	[SerializeField]
	GameObject perfect, great, good, bad;

	[SerializeField]
	float showSeconds = 1.5f;

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
				break;

			case Score.GREAT:
				target = great;
				break;

			case Score.GOOD:
				target = good;
				break;

			case Score.BAD:
				target = bad;
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
