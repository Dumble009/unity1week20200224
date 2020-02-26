using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerTempoBar : TempoBar
{
	protected override void Start()
	{
		base.Start();
		StageManager.Instance.OnStartPlaying
			.Subscribe(_i => {
				isPlaying = true;
				startBeat = _i;
			});

		StageManager.Instance.OnEndPlaying
			.Subscribe(_i => {
				isPlaying = false;
			});

		InputAgent.Instance.OnInput
			.Subscribe(_i => {
				NodeObjectManager.Instance.CreateNode(_i, transform.position);
			});
	}
}
