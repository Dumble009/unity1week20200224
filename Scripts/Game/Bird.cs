using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
	[SerializeField]
	protected Sprite low, mid, high, def;
	protected SpriteRenderer spriteRenderer;

	protected virtual void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
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
