using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	public static T Instance {
		get {
			return _instance;
		}
	}

	protected virtual void Awake()
	{
		CheckInstance();
	}

	protected virtual void CheckInstance()
	{
		_instance = this as T;
	}
}
