using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerTempoBar : TempoBar
{
	protected override void Start()
	{
		base.Start();
		StageManager.Instance.OnPlayerBarStart
			.Subscribe(_i => {
				isPlaying = true;
				startBeat = _i;
			});

		StageManager.Instance.OnPlayerBarStop
			.Subscribe(_i => {
				isPlaying = false;
			});

		ProblemManager.Instance.OnProblemFinish
			.Subscribe(_i => {
				isPlaying = false;
				isBeating = false;
			});

		StageManager.Instance.OnStartStage
			.Subscribe(_i => {
				isBeating = true;
			});

		InputAgent.Instance.OnInput
			.Subscribe(_i => {
				NodeObjectManager.Instance.CreateNode(_i, transform.position);
			});
	}
}
