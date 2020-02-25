using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class NodeObjectManager : SingletonMonoBehaviour<NodeObjectManager>
{
	[SerializeField]
	GameObject upNode, downNode, neutralNode;
	List<GameObject> nodes;

	protected void Start()
	{
		nodes = new List<GameObject>();

		StageManager.Instance.OnStartProblem
			.Subscribe(_i => {
				foreach (var go in nodes)
				{
					Destroy(go);
				}
				nodes.Clear();
			});
	}

	public void CreateNode(int pitch, Vector3 position)
	{
		if (pitch == 0)
		{
			GameObject temp = Instantiate(downNode, position, Quaternion.identity) as GameObject;
			nodes.Add(temp);
		}
		else if (pitch == 1)
		{
			GameObject temp = Instantiate(neutralNode, position, Quaternion.identity) as GameObject;
			nodes.Add(temp);
		}
		else if (pitch == 2)
		{
			GameObject temp = Instantiate(upNode, position, Quaternion.identity) as GameObject;
			nodes.Add(temp);
		}
	}
}
