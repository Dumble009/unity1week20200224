using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ProblemBird : Bird
{
	protected override void Start()
	{
		base.Start();
		ProblemManager.Instance.OnNode
			.Subscribe(_i => {
				ChangeSprite(_i);
			});

		StageManager.Instance.OnProblemBarStop
			.Subscribe(_i => {
				spriteRenderer.sprite = def;
			});
	}
}
