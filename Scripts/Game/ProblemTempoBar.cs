﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ProblemTempoBar : TempoBar
{
	protected override void Start()
	{
		base.Start();
		StageManager.Instance.OnStartProblem
			.Subscribe(_i => {
				isPlaying = true;
			});

		StageManager.Instance.OnEndProblem
			.Subscribe(_i => {
				isPlaying = false;
			});
	}
}
