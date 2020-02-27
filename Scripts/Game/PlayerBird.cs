using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerBird : Bird
{
	protected override void Start()
	{
		base.Start();
		InputAgent.Instance.OnInput
			.Subscribe(_i => {
				ChangeSprite(_i);
			});

		StageManager.Instance.OnPlayerBarStop
			.Subscribe(_i => {
				spriteRenderer.sprite = def;
			});
	}
}
