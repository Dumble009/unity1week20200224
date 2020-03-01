using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Bird : MonoBehaviour
{
	[SerializeField]
	protected Sprite low, mid, high, def, perfectSprite, greatSprite, goodSprite, badSprite;
	protected SpriteRenderer spriteRenderer;

	protected virtual void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		ScoreManager.Instance.OnCalcScore
			.Subscribe(_s => {
				switch (_s) {
					case Score.PERFECT:
						if (perfectSprite)
						{
							spriteRenderer.sprite = perfectSprite;
						}
						break;

					case Score.GREAT:
						if (greatSprite)
						{
							spriteRenderer.sprite = greatSprite;
						}
						break;

					case Score.GOOD:
						if (goodSprite)
						{
							spriteRenderer.sprite = goodSprite;
						}
						break;

					case Score.BAD:
						if (badSprite)
						{
							spriteRenderer.sprite = badSprite;
						}
						break;

					default:
						break;
				}
			});

		StageManager.Instance.OnProblemBarStart
			.Subscribe(_i => {
				if (def)
				{
					spriteRenderer.sprite = def;
				}
			});
	}

	public virtual void ChangeSprite(int pitch)
	{
		if (pitch == 0)
		{
			spriteRenderer.sprite = low;
		}
		else if (pitch == 1)
		{
			spriteRenderer.sprite = mid;
		}
		else if (pitch == 2)
		{
			spriteRenderer.sprite = high;
		}
	}
}
