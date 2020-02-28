using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TempoBar : MonoBehaviour
{
	[SerializeField]
	float maxSize = 0.3f;
	[SerializeField]
	float minSize = 0.2f;

	[SerializeField]
	protected Transform startPoint;
	[SerializeField]
	protected Transform endPoint;

	protected bool isPlaying;
	protected bool isObserving;

	protected int startBeat = 0;

	protected virtual void Start()
	{
		isPlaying = false;
		BeatManager.Instance.OnTimeInBar
			.Subscribe(_f => {
				SetValue(_f);
			});
	}

	protected virtual void SetValue(float time)
	{
		float val = Mathf.Abs((time % 1.0f) - 0.5f) * 2.0f;

		float clamped = Mathf.Lerp(minSize, maxSize, val * val);
		
		Vector3 size = new Vector3(clamped, clamped, clamped);

		transform.localScale = size;

		if (isPlaying)
		{
			float posVal = ((time - startBeat) % 5.0f) / 5.0f;

			Vector3 position = Vector3.Lerp(startPoint.position, endPoint.position, posVal);

			transform.position = position;
		}
	}
}
