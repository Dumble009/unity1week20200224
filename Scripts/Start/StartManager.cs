using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : SingletonMonoBehaviour<StartManager>
{
	[SerializeField]
	GameObject[] notations;

	[SerializeField]
	GameObject startMenuRoot;

	public void PushStartButton()
	{
		startMenuRoot.SetActive(false);
		foreach (var notation in notations)
		{
			notation.SetActive(true);
		}
	}
}
