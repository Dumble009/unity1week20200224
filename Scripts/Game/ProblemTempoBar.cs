﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ProblemTempoBar : TempoBar
{
	protected override void Start()
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

		ProblemManager.Instance.OnNode
			.Subscribe(_i => {
				NodeObjectManager.Instance.CreateNode(_i, transform.position);
			});
	}
}