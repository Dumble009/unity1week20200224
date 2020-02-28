using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ProblemTempoBar : TempoBar
{
	override protected void Start()
	{
		base.Start();
		StageManager.Instance.OnProblemBarStart
			.Subscribe(_i => {
				isPlaying = true;
				startBeat = _i;
			});

		StageManager.Instance.OnProblemBarStop
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

		ProblemManager.Instance.OnNode
			.Subscribe(_i => {
				NodeObjectManager.Instance.CreateNode(_i, transform.position);
			});
	}
}
